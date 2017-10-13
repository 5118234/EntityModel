using System;

namespace Gboxt.Common.DataModel.MySql
{
    /// <summary>
    /// ����״̬����
    /// </summary>
    /// <typeparam name="TData">ʵ��</typeparam>
    public abstract class HitoryTable<TData> : DataStateTable<TData>
        where TData : EditDataObject, IStateData, IHistoryData, IIdentityData, new()
    {
        /// <summary>
        /// �����ͬʱִ�е�SQL
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        protected override string AfterUpdateSql(string condition)
        {
            if (BusinessContext.Current.IsSystemMode)
                return null;
            var filter= string.IsNullOrEmpty(condition) ? null : " WHERE " + condition;
            return $@"UPDATE `{WriteTableName}` 
SET {FieldDictionary["LastReviserID"]}={BusinessContext.Current.LoginUserId},
{FieldDictionary["LastModifyDate"]}=NOW(){filter};";
        }
        
        /// <summary>
        ///     ����ǰ����
        /// </summary>
        /// <param name="entity">����Ķ���</param>
        /// <param name="operatorType">��������</param>
        protected override void OnPrepareSave(DataOperatorType operatorType, TData entity)
        {
            if (operatorType == DataOperatorType.Insert)
            {
                entity.AddDate = DateTime.Now;
                entity.AuthorID = BusinessContext.Current.LoginUserId;
            }
        }
    }
}