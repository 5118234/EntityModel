// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // ����:
// // ����:Agebull.DataModel
// // ����:2016-06-12
// // �޸�:2016-06-16
// // *****************************************************/

#region ����

using System;
using System.Linq.Expressions;
using Gboxt.Common.DataModel.MySql;

#endregion

namespace Gboxt.Common.DataModel.BusinessLogic
{
    /// <summary>
    /// ��������״̬��ҵ���߼�����
    /// </summary>
    /// <typeparam name="TData">���ݶ���</typeparam>
    /// <typeparam name="TAccess">���ݷ��ʶ���</typeparam>
    public class BusinessLogicByStateData<TData, TAccess> : UiBusinessLogicBase<TData, TAccess>
        where TData : EditDataObject, IIdentityData, IStateData, new()
        where TAccess : MySqlTable<TData>, new()
    {
        #region ��������


        /// <summary>
        ///     ���ö���
        /// </summary>
        public bool Enable(string sels)
        {
            return DoByIds(sels, Enable);
        }

        /// <summary>
        ///     ���ö���
        /// </summary>
        public bool Disable(string sels)
        {
            return DoByIds(sels, Disable);
        }

        /// <summary>
        ///     ���ö���
        /// </summary>
        public bool Lock(string sels)
        {
            return DoByIds(sels, Lock);
        }
        #endregion

        #region ����״̬�߼�

        /// <summary>
        ///     �Ƿ����ִ�б������
        /// </summary>
        /// <param name="data">����</param>
        /// <param name="isAdd">�Ƿ�Ϊ����</param>
        /// <returns>���Ϊ����ֹ��������</returns>
        protected override bool CanSave(TData data, bool isAdd)
        {
            return !data.IsFreeze && data.DataState < DataStateType.Discard;
        }

        /// <summary>
        ///     ɾ������ǰ�ô���
        /// </summary>
        protected override bool PrepareDelete(int id)
        {
            if (Access.Any(p => p.Id == id && (p.IsFreeze || p.DataState == DataStateType.Disable || p.DataState == DataStateType.Enable)))
                return false;
            return base.PrepareDelete(id);
        }
        /// <summary>
        ///     ɾ���������
        /// </summary>
        protected override bool DoDelete(int id)
        {
            using (MySqlDataBaseScope.CreateScope(Access.DataBase))
            {
                if (!Access.Any(p => p.Id == id && p.DataState == DataStateType.Delete))
                    return Access.SetValue(p => p.DataState, DataStateType.Delete, p => p.Id == id && p.DataState == DataStateType.None) > 0;
                if (BusinessContext.Current.CanDoCurrentPageAction("physical_delete"))
                    return Access.PhysicalDelete(id);
                BusinessContext.Current.LastMessage = "����������ִ������ɾ������";
                return false;
            }
        }

        /// <summary>
        ///     ɾ��������ô���
        /// </summary>
        protected override void OnDeleted(int id)
        {
            if (unityStateChanged)
            {
                var data = Access.LoadData(id);
                OnStateChanged(data, BusinessCommandType.Delete);
            }
            base.OnDeleted(id);
        }

        #endregion

        #region ����״̬�޸�

        /// <summary>
        ///     ������ɺ�Ĳ���
        /// </summary>
        /// <param name="data">����</param>
        /// <param name="isAdd">�Ƿ�Ϊ����</param>
        /// <returns>���Ϊ����ֹ��������</returns>
        protected override bool LastSaved(TData data, bool isAdd)
        {
            if (unityStateChanged)
                OnStateChanged(data, isAdd ? BusinessCommandType.AddNew : BusinessCommandType.Update);
            return base.LastSaved(data, isAdd);
        }

        /// <summary>
        /// �Ƿ�ͳһ����״̬�仯
        /// </summary>
        protected bool unityStateChanged = false;

        /// <summary>
        ///     ״̬�ı���ͳһ����(unityStateChanged������Ϊtrueʱ�����������--�������ܵĿ���)
        /// </summary>
        /// <param name="data">����</param>
        /// <param name="cmd">����</param>
        protected void OnStateChanged(TData data, BusinessCommandType cmd)
        {
            if (unityStateChanged)
            {
                OnInnerCommand(data, cmd);
                DoStateChanged(data);
            }
        }

        /// <summary>
        ///     �ڲ�����ִ����ɺ�Ĵ���
        /// </summary>
        /// <param name="id">����</param>
        /// <param name="cmd">����</param>
        protected sealed override void OnInnerCommand(int id, BusinessCommandType cmd)
        {
            if (unityStateChanged)
                OnInnerCommand(Access.LoadByPrimaryKey(id), cmd);
        }
        /// <summary>
        ///     ״̬�ı���ͳһ����(unityStateChanged������Ϊtrueʱ�����������--�������ܵĿ���)
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected virtual void DoStateChanged(TData data)
        {
        }

        #endregion

        #region ״̬����
        /// <summary>
        ///     ��������״̬
        /// </summary>
        /// <param name="data"></param>
        public bool ResetState(TData data)
        {
            if (data == null)
                return false;
            if (!DoResetState(data))
                return false;
            Access.Update(data);
            if (unityStateChanged)
                OnStateChanged(data, BusinessCommandType.Reset);
            return true;
        }

        /// <summary>
        ///     ��������״̬
        /// </summary>
        /// <param name="data"></param>
        protected virtual bool DoResetState(TData data)
        {
            data.DataState = DataStateType.None;
            data.IsFreeze = false;
            return true;
        }

        /// <summary>
        ///     ��������״̬
        /// </summary>
        /// <param name="id"></param>
        public virtual bool Reset(int id)
        {
            return SetDataState(id, DataStateType.None, p => p.Id == id && p.DataState >= DataStateType.Discard);
        }

        /// <summary>
        ///     ���ö���
        /// </summary>
        public virtual bool Enable(int id)
        {
            if (Access.LoadValue(p => p.IsFreeze, id))
            {
                Access.SetValue(p => p.IsFreeze, false, id);
                return true;
            }
            return SetDataState(id, DataStateType.Enable, p => p.Id == id && (p.DataState == DataStateType.Disable || p.DataState == DataStateType.None));
        }

        /// <summary>
        ///     ���ö���
        /// </summary>
        public virtual bool Disable(int id)
        {
            return SetDataState(id, DataStateType.Disable, p => p.Id == id && p.DataState == DataStateType.Enable);
        }
        /// <summary>
        ///     ���ö���
        /// </summary>
        public virtual bool Discard(int id)
        {
            return SetDataState(id, DataStateType.Discard, p => p.Id == id && p.DataState == DataStateType.None);
        }
        /// <summary>
        ///     ��������
        /// </summary>
        public virtual bool Lock(int id)
        {
            using (MySqlDataBaseScope.CreateScope(Access.DataBase))
            {
                if (!Access.Any(p => p.Id == id && p.DataState < DataStateType.Discard && !p.IsFreeze))
                    return false;
                Access.SetValue(p => p.IsFreeze, true, id);
                Access.SetValue(p => p.DataState, DataStateType.Enable, id);
                if (!unityStateChanged)
                    return true;
                OnStateChanged(Access.LoadByPrimaryKey(id), BusinessCommandType.Lock);
                return true;
            }
        }

        /// <summary>
        ///     �޸�״̬
        /// </summary>
        protected bool SetDataState(int id, DataStateType state, Expression<Func<TData, bool>> filter)
        {
            using (MySqlDataBaseScope.CreateScope(Access.DataBase))
            {
                if (!Access.ExistPrimaryKey(id) || !Access.Any(filter))
                    return false;
                Access.SetValue(p => p.DataState, state, id);
                if (!unityStateChanged)
                    return true;
                OnStateChanged(Access.LoadByPrimaryKey(id), BusinessCommandType.SetState);
                return true;
            }
        }
        #endregion
    }
}