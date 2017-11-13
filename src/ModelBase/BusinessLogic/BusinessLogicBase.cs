// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // ����:
// // ����:Agebull.DataModel
// // ����:2016-06-12
// // �޸�:2016-06-16
// // *****************************************************/

#region ����

using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using MySql.Data.MySqlClient;
using System.Linq.Expressions;
using System.Web;
using Agebull.Common.Logging;
using Gboxt.Common.WebUI;

#endregion

namespace Gboxt.Common.DataModel.BusinessLogic
{
    /// <summary>
    /// ҵ���߼��������
    /// </summary>
    /// <typeparam name="TData">���ݶ���</typeparam>
    /// <typeparam name="TAccess">���ݷ��ʶ���</typeparam>
    public class BusinessLogicBase<TData, TAccess>
    where TData : EditDataObject, IIdentityData, new()
    where TAccess : class, IDataTable<TData>, new()
    {
        #region ����֧�ֶ���

        /// <summary>
        ///     ʵ������
        /// </summary>
        public virtual int EntityType => 0;

        private TAccess _access;

        /// <summary>
        ///     ���ݷ��ʶ���
        /// </summary>
        public TAccess Access => _access ?? (_access = CreateAccess());

        /// <summary>
        ///     ���ݷ��ʶ���
        /// </summary>
        protected virtual TAccess CreateAccess()
        {
            var access = new TAccess();
            return access;
        }
        /// <summary>
        /// ����
        /// </summary>
        protected BusinessLogicBase()
        {
        }
        #endregion

        #region ��������

        /// <summary>
        ///     ִ��ID����ִ��Ĳ���������ҳ���,����ϵ�ID��
        /// </summary>
        public bool DoByIds(string ids, Func<int, bool> func, Action onEnd = null)
        {
            return LoopIds(ids, func, onEnd);
        }

        /// <summary>
        ///     ִ��ID����ִ��Ĳ���������ҳ���,����ϵ�ID��
        /// </summary>
        public bool LoopIds(string ids, Func<int, bool> func, Action onEnd = null)
        {
            if (string.IsNullOrEmpty(ids))
            {
                return false;
            }
            using (Access.DataBase.CreateDataBaseScope())
            {
                using (var scope = Access.DataBase.CreateTransactionScope())
                {
                    var sids = ids.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    if (sids.Length == 0)
                    {
                        return false;
                    }
                    foreach (var sid in sids)
                    {
                        int id;
                        if (!int.TryParse(sid, out id))
                        {
                            return false;
                        }
                        if (!func(id))
                        {
                            return false;
                        }
                    }
                    onEnd?.Invoke();
                    scope.SetState(true);
                }
            }
            return true;
        }

        /// <summary>
        ///     ִ��ID����ִ��Ĳ���������ҳ���,����ϵ�ID��
        /// </summary>
        public bool DoByIds(string ids, Func<TData, bool> func, Action onEnd = null)
        {
            return LoopIdsToData(ids, func, onEnd);
        }

        /// <summary>
        ///     ִ��ID����ִ��Ĳ���������ҳ���,����ϵ�ID��
        /// </summary>
        public bool LoopIdsToData(string ids, Func<TData, bool> func, Action onEnd = null)
        {
            if (string.IsNullOrEmpty(ids))
            {
                return false;
            }
            using (Access.DataBase.CreateDataBaseScope())
            {
                using (var scope = Access.DataBase.CreateTransactionScope())
                {
                    var sids = ids.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    if (sids.Length == 0)
                    {
                        return false;
                    }
                    foreach (var sid in sids)
                    {
                        int id;
                        if (!int.TryParse(sid, out id))
                        {
                            return false;
                        }
                        var data = Access.LoadByPrimaryKey(id);
                        if (data == null || !func(data))
                        {
                            return false;
                        }
                    }
                    onEnd?.Invoke();
                    scope.SetState(true);
                }
            }
            return true;
        }
        #endregion

        #region ������

        /// <summary>
        ///     ȡ���б�����
        /// </summary>
        public List<TData> All()
        {
            return Access.All();
        }
        
        /// <summary>
        ///     ��ȡ����
        /// </summary>
        /// <param name="lambda">��ѯ���ʽ</param>
        /// <returns>�Ƿ��������</returns>
        public List<TData> All(Expression<Func<TData, bool>> lambda)
        {
            return Access.All(lambda);
        }

        /// <summary>
        ///     ���뵱ǰ����������
        /// </summary>
        public TData FirstOrDefault(Expression<Func<TData, bool>> lambda)
        {
            return Access.FirstOrDefault(lambda);
        }

        /// <summary>
        ///     ���뵱ǰ����������
        /// </summary>
        public virtual TData Details(long id)
        {
            return id == 0 ? null : Access.LoadByPrimaryKey(id);
        }

        #endregion
    }

    /// <summary>
    /// ֧�ֽ��������ҵ���߼��������
    /// </summary>
    /// <typeparam name="TData">���ݶ���</typeparam>
    /// <typeparam name="TAccess">���ݷ��ʶ���</typeparam>
    public class UiBusinessLogicBase<TData, TAccess> : BusinessLogicBase<TData, TAccess>
    where TData : EditDataObject, IIdentityData, new()
        where TAccess : class, IDataTable<TData>, new()
    {
        /// <summary>
        /// ��ǰ����
        /// </summary>
        public HttpRequest Request { get; set; }

        #region ������

        /// <summary>
        ///     ȡ���б�����
        /// </summary>
        public EasyUiGridData<TData> PageData(int page, int limit, string condition, params DbParameter[] args)
        {
            return PageData(page, limit, null, false, condition, args);
        }

        /// <summary>
        ///     ȡ���б�����
        /// </summary>
        public EasyUiGridData<TData> PageData(int page, int limit, string sort, bool desc, string condition,
            params DbParameter[] args)
        {
            using (Access.DataBase.CreateDataBaseScope())
            {
                //using (MySqlReaderScope<TData>.CreateScope(Access, Access.SimpleFields, Access.SimpleLoad))
                {
                    var data = Access.PageData(page, limit, sort, desc, condition, args);
                    var count = (int)Access.Count(condition, args);
                    return new EasyUiGridData<TData>
                    {
                        Data = data,
                        Total = count
                    };
                }
            }
        }

        /// <summary>
        ///     ��ҳ��ȡ
        /// </summary>
        public EasyUiGridData<TData> PageData(int page, int limit, Expression<Func<TData, bool>> lambda)
        {
            using (Access.DataBase.CreateDataBaseScope())
            {
                //using (MySqlReaderScope<TData>.CreateScope(Access, Access.SimpleFields, Access.SimpleLoad))
                {
                    if (limit <= 0 || limit >= 999)
                    {
                        limit = 30;
                    }
                    var data = Access.PageData(page, limit, lambda);
                    var count = (int)Access.Count(lambda);
                    return new EasyUiGridData<TData>
                    {
                        Data = data,
                        Total = count
                    };
                }
            }
        }
        
        #endregion

        #region д����

        /// <summary>
        ///     �ڲ�����ִ����ɺ�Ĵ���
        /// </summary>
        /// <param name="data">����</param>
        /// <param name="cmd">����</param>
        protected virtual void OnInnerCommand(TData data, BusinessCommandType cmd)
        {

        }


        /// <summary>
        ///     �ڲ�����ִ����ɺ�Ĵ���
        /// </summary>
        /// <param name="id">����</param>
        /// <param name="cmd">����</param>
        protected virtual void OnInnerCommand(long id, BusinessCommandType cmd)
        {

        }


        /// <summary>
        ///     �Ƿ����ִ�б������
        /// </summary>
        /// <param name="data">����</param>
        /// <param name="isAdd">�Ƿ�Ϊ����</param>
        /// <returns>���Ϊ����ֹ��������</returns>
        protected virtual bool CanSave(TData data, bool isAdd)
        {
            return true;
        }

        /// <summary>
        ///     ����ǰ�Ĳ���
        /// </summary>
        /// <param name="data">����</param>
        /// <param name="isAdd">�Ƿ�Ϊ����</param>
        /// <returns>���Ϊ����ֹ��������</returns>
        protected virtual bool PrepareSave(TData data, bool isAdd)
        {
            if (data.__IsFromUser && !PrepareSaveByUser(data, isAdd))
                return false;
            return OnSaving(data, isAdd);
        }

        /// <summary>
        ///     ������ɺ�Ĳ���
        /// </summary>
        /// <param name="data">����</param>
        /// <param name="isAdd">�Ƿ�Ϊ����</param>
        /// <returns>���Ϊ����ֹ��������</returns>
        protected virtual bool LastSaved(TData data, bool isAdd)
        {
            if (data.__IsFromUser && !LastSavedByUser(data, isAdd))
                return false;
            return OnSaved(data, isAdd);
        }

        /// <summary>
        ///     ���û��༭�����ݵı���ǰ����
        /// </summary>
        /// <param name="data">����</param>
        /// <param name="isAdd">�Ƿ�Ϊ����</param>
        /// <returns>���Ϊ����ֹ��������</returns>
        protected virtual bool PrepareSaveByUser(TData data, bool isAdd)
        {
            return true;
        }

        /// <summary>
        ///     ���û��༭�����ݵı���ǰ����
        /// </summary>
        /// <param name="data">����</param>
        /// <param name="isAdd">�Ƿ�Ϊ����</param>
        /// <returns>���Ϊ����ֹ��������</returns>
        protected virtual bool LastSavedByUser(TData data, bool isAdd)
        {
            return true;
        }

        /// <summary>
        ///     ����ǰ�Ĳ���
        /// </summary>
        /// <param name="data">����</param>
        /// <param name="isAdd">�Ƿ�Ϊ����</param>
        /// <returns>���Ϊ����ֹ��������</returns>
        protected virtual bool OnSaving(TData data, bool isAdd)
        {
            return true;
        }

        /// <summary>
        ///     ������ɺ�Ĳ���
        /// </summary>
        /// <param name="data">����</param>
        /// <param name="isAdd">�Ƿ�Ϊ����</param>
        /// <returns>���Ϊ����ֹ��������</returns>
        protected virtual bool OnSaved(TData data, bool isAdd)
        {
            return true;
        }

        /// <summary>
        ///     ����
        /// </summary>
        public virtual bool Save(TData data)
        {
            return data.Id == 0 ? AddNew(data) : Update(data);
        }
        /// <summary>
        ///     ����
        /// </summary>
        public virtual bool AddNew(TData data)
        {
            using (var scope = Access.DataBase.CreateTransactionScope())
            {
                if (!CanSave(data, true))
                {
                    return false;
                }
                if (!PrepareSave(data, true))
                {
                    return false;
                }
                if (!data.__EntityStatusNull && data.__EntityStatus.IsExist)
                    Access.Update(data);
                else
                    Access.Insert(data);
                var result = LastSaved(data, true);
                scope.SetState(true);
                return result;
            }
        }

        /// <summary>
        ///     ���¶���
        /// </summary>
        public virtual bool Update(TData data)
        {
            if (data.Id == 0)
            {
                return AddNew(data);
            }
            using (var scope = Access.DataBase.CreateTransactionScope())
            {
                if (!CanSave(data, true))
                {
                    return false;
                }
                if (!PrepareSave(data, false))
                {
                    return false;
                }
                Access.Update(data);
                var result = LastSaved(data, false);
                scope.SetState(true);
                return result;
            }
        }


        #endregion

        #region ɾ��

        /// <summary>
        ///     ɾ������
        /// </summary>
        public bool Delete(IEnumerable<long> lid)
        {
            using (Access.DataBase.CreateDataBaseScope())
            using (var scope = Access.DataBase.CreateTransactionScope())
            {
                foreach (var id in lid)
                {
                    if (!Delete(id))
                        return false;
                }
                scope.SetState(true);
            }
            return true;
        }

        /// <summary>
        ///     ɾ������
        /// </summary>
        public bool Delete(long id)
        {
            using (var scope = Access.DataBase.CreateTransactionScope())
            {
                if (!PrepareDelete(id))
                {
                    return false;
                }
                if (!DoDelete(id))
                    return false;
                OnDeleted(id);
                LogRecorder.MonitorTrace("Delete");
                OnInnerCommand(id, BusinessCommandType.Delete);
                scope.SetState(true);
                return true;
            }
        }

        /// <summary>
        ///     ɾ���������
        /// </summary>
        protected virtual bool DoDelete(long id)
        {
            return Access.DeletePrimaryKey(id) == 1;
        }

        /// <summary>
        ///     ɾ������ǰ�ô���
        /// </summary>
        protected virtual bool PrepareDelete(long id)
        {
            return true;
        }

        /// <summary>
        ///     ɾ��������ô���
        /// </summary>
        protected virtual void OnDeleted(long id)
        {

        }
        #endregion
    }
}