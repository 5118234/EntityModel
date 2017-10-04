// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // ����:
// // ����:Agebull.DataModel
// // ����:2016-06-12
// // �޸�:2016-06-16
// // *****************************************************/

#region ����

using System;
using Gboxt.Common.DataModel.MySql;

#endregion

namespace Gboxt.Common.DataModel.BusinessLogic
{
    /// <summary>
    /// ������ʷ��¼��ҵ���߼�����
    /// </summary>
    /// <typeparam name="TData">���ݶ���</typeparam>
    /// <typeparam name="TAccess">���ݷ��ʶ���</typeparam>
    public class BusinessLogicByHistory<TData, TAccess> : BusinessLogicByStateData<TData, TAccess>
        where TData : EditDataObject, IIdentityData, IHistoryData,  IStateData, new()
        where TAccess : HitoryTable<TData>, new()
    {
        /// <summary>
        ///     ��������״̬
        /// </summary>
        /// <param name="data"></param>
        protected override bool DoResetState(TData data)
        {
            if (data == null)
                return false;
            data.AddDate = DateTime.MinValue;
            data.AuthorID = 0;
            data.LastModifyDate = DateTime.MinValue;
            data.LastReviserID = 0;
            return base.DoResetState(data);
        }
    }
}