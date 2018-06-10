// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // ����:
// // ����:Agebull.DataModel
// // ����:2016-06-12
// // �޸�:2016-06-16
// // *****************************************************/

#region ����

#endregion

namespace Gboxt.Common.DataModel.MySql
{
    /// <summary>
    ///     Sqlʵ�������(֧�ֱ��ػ���,���ڷ�ֹ�������������ݲ�һ��)
    /// </summary>
    /// <typeparam name="TData">ʵ��</typeparam>
    /// <typeparam name="TMySqlDataBase">���ڵ����ݿ����,��ͨ��Ioc�Զ�����</typeparam>
    public abstract class CacheTable<TData, TMySqlDataBase> : MySqlTable<TData, TMySqlDataBase>
        where TData : EditDataObject, IIdentityData, new()
        where TMySqlDataBase : MySqlDataBase
    {
        /// <summary>
        ///     ������ȡ
        /// </summary>
        public override TData LoadByPrimaryKey(object key)
        {
            var data = MySqlDataBase.DataBase.GetData<TData>(TableId, (int) key);
            return data ?? base.LoadByPrimaryKey(key);
        }

        /// <summary>
        ///     ������ͬ������
        /// </summary>
        /// <param name="entity"></param>
        protected override TData OnEntityLoaded(TData entity)
        {
            return MySqlDataBase.DataBase.TryAddToCache(TableId, entity.Id, entity);
        }
    }
}