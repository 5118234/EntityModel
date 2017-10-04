// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // ����:
// // ����:Agebull.DataModel
// // ����:2016-06-07
// // �޸�:2016-06-16
// // *****************************************************/

#region ����

using System;
using System.Configuration;
using System.Runtime.Serialization;
using Newtonsoft.Json;

#endregion

namespace Gboxt.Common.DataModel
{
    /// <summary>
    ///     ʵ�����
    /// </summary>
    [DataContract, Serializable]
    public abstract class DataObjectBase : NotificationObject, IDataObject
    {
        #region ʵ�����֧��

        /// <summary>
        ///     ����ֵ
        /// </summary>
        /// <param name="source">���Ƶ�Դ�ֶ�</param>
        public void CopyValue(IDataObject source)
        {
            var entity = source as DataObjectBase;
            if (entity != null)
            {
                CopyValueInner(entity);
            }
        }

        /// <summary>
        ///     ��������ֵ
        /// </summary>
        /// <param name="property"></param>
        /// <param name="value"></param>
        public void SetValue(string property, object value)
        {
            SetValueInner(property, value);
        }

        /// <summary>
        ///     ��������ֵ
        /// </summary>
        /// <param name="property"></param>
        public object GetValue(string property)
        {
            return GetValueInner(property);
        }

        /// <summary>
        ///     ��������ֵ
        /// </summary>
        /// <param name="property"></param>
        public TValue GetValue<TValue>(string property)
        {
            return GetValueInner<TValue>(property);
        }

        /// <summary>
        ///     ��������ֵ
        /// </summary>
        /// <param name="property"></param>
        /// <param name="value"></param>
        public void SetValue(int property, object value)
        {
            SetValueInner(property, value);
        }

        /// <summary>
        ///     ��������ֵ
        /// </summary>
        /// <param name="property"></param>
        public object GetValue(int property)
        {
            return GetValueInner(property);
        }

        /// <summary>
        ///     ��������ֵ
        /// </summary>
        /// <param name="property"></param>
        public TValue GetValue<TValue>(int property)
        {
            return GetValueInner<TValue>(property);
        }

        #endregion

        #region �ڲ�ʵ��

        /// <summary>
        ///     ����ֵ
        /// </summary>
        /// <param name="source">���Ƶ�Դ�ֶ�</param>
        protected abstract void CopyValueInner(DataObjectBase source);

        /// <summary>
        ///     ��������ֵ
        /// </summary>
        /// <param name="property"></param>
        /// <param name="value"></param>
        protected abstract void SetValueInner(string property, object value);

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
        protected abstract void SetValueInner(int property, object value);

        /// <summary>
        ///     ��������ֵ
        /// </summary>
        /// <param name="property"></param>
        protected abstract object GetValueInner(int property);

        /// <summary>
        ///     ��������ֵ
        /// </summary>
        /// <param name="property"></param>
        protected virtual TValue GetValueInner<TValue>(string property)
        {
            return (TValue) GetValue(property);
        }

        /// <summary>
        ///     ��������ֵ
        /// </summary>
        /// <param name="property"></param>
        protected virtual TValue GetValueInner<TValue>(int property)
        {
            return (TValue) GetValue(property);
        }

        #endregion

        #region ���ݰ汾����

        private static byte _version;

        /// <summary>
        ///     ʵ���ʽ�汾��
        /// </summary>
        public static byte EntityVersion
        {
            get
            {
                if (_version > 0)
                {
                    return _version;
                }
                var ev = ConfigurationManager.AppSettings["EntityVersion"];
                return _version = string.IsNullOrWhiteSpace(ev) ? (byte) 1 : byte.Parse(ev);
            }
        }

#if WEB

        /// <summary>
        ///     ����ҳ���ѡ������
        /// </summary>
        [JsonProperty("IsSelected")]
        public bool __IsSelected { get; set; }
#endif

        #endregion
    }
}