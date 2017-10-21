using Gboxt.Common.DataModel;
using System.Collections.Generic;

namespace Agebull.Common.DataModel
{
    /// <summary>
    ///     ʵ�����
    /// </summary>
    public abstract class EntityBase : NotificationObject
    {
        #region ʵ�����

        /// <summary>
        /// ����ֵ
        /// </summary>
        /// <param name="entity">���Ƶ�Դ�ֶ�</param>
        public void CopyValue(EntityBase entity)
        {
            if(entity != null)
                CopyValueInner(entity);
        }

        /// <summary>
        ///     ��������ֵ
        /// </summary>
        /// <param name="property"></param>
        /// <param name="value"></param>
        public void SetValue(string property, object value)
        {
            property = property?.ToLower();
            SetValueInner(property, value);
        }

        /// <summary>
        ///     ��������ֵ
        /// </summary>
        /// <param name="property"></param>
        public object GetValue(string property)
        {
            property = property?.ToLower();
            return GetValueInner(property);
        }

        /// <summary>
        ///     ��������ֵ
        /// </summary>
        /// <param name="property"></param>
        public TValue GetValue<TValue>(string property)
        {
            property = property?.ToLower();
            return GetValueInner<TValue>(property);
        }
        #endregion

        #region �ڲ�ʵ��

        /// <summary>
        /// ����ֵ
        /// </summary>
        /// <param name="source">���Ƶ�Դ�ֶ�</param>
        protected abstract void CopyValueInner(EntityBase source);
        
        /// <summary>
        ///     ��������ֵ
        /// </summary>
        /// <param name="property"></param>
        protected virtual TValue GetValueInner<TValue>(string property)
        {
            return (TValue)GetValue(property);
        }
        /// <summary>
        ///     ��������ֵ
        /// </summary>
        /// <param name="property"></param>
        protected abstract object GetValueInner(string property);

        /// <summary>
        ///     ��������ֵ
        /// </summary>
        /// <param name="property"></param>
        /// <param name="value"></param>
        protected abstract void SetValueInner(string property, object value);

        #endregion

        #region �޸��б�

        /// <summary>
        /// �޸��б�
        /// </summary>
        private readonly List<string> _modifiedList = new List<string>();

        /// <summary>
        ///     �����޸Ĵ���
        /// </summary>
        /// <param name="propertyName">��������</param>
        protected override void OnPropertyChangedInner(string propertyName)
        {
            base.OnPropertyChangedInner(propertyName);
            if (!_modifiedList.Contains(propertyName))
                _modifiedList.Add(propertyName);
        }

        #endregion
    }
}