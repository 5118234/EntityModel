// // /*****************************************************
// // (c)2016-2016 Copy right www.gboxt.com
// // ����:
// // ����:Agebull.DataModel
// // ����:2016-06-07
// // �޸�:2016-06-16
// // *****************************************************/

#region ����

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;

#endregion

namespace Gboxt.Common.DataModel
{
    /// <summary>
    ///     ����������һЩ��չ
    /// </summary>
    public static class CommonExtend
    {
        /// <summary>
        ///     ��ȷ��ȫ��תΪС��
        /// </summary>
        /// <param name="obj">����</param>
        /// <param name="def">�޷�ת��ʱ��ȱʡֵ</param>
        /// <returns>С��</returns>
        public static decimal ToDecimal(this object obj, decimal def = 0)
        {
            if (obj == null)
            {
                return def;
            }
            if (obj is decimal)
            {
                return (decimal) obj;
            }
            decimal re;
            return decimal.TryParse(obj.ToString().Trim(), out re) ? re : def;
        }

        /// <summary>
        ///     ��ȷ��ȫ��תΪС��
        /// </summary>
        /// <param name="str">�ı�����</param>
        /// <param name="def">�޷�ת��ʱ��ȱʡֵ</param>
        /// <returns>С��</returns>
        public static decimal ToDecimal(this string str, decimal def = 0)
        {
            if (str == null)
            {
                return def;
            }
            decimal re;
            return decimal.TryParse(str.Trim(), out re) ? re : def;
        }

        /// <summary>
        ///     ��ȷ��ȫ��תΪ����
        /// </summary>
        /// <param name="obj">����</param>
        /// <param name="def">�޷�ת��ʱ��ȱʡֵ</param>
        /// <returns>����</returns>
        public static int ToInteger(this object obj, int def = 0)
        {
            if (obj == null)
            {
                return def;
            }
            if (obj is int)
            {
                return (int) obj;
            }
            int re;
            return int.TryParse(obj.ToString().Trim(), out re) ? re : def;
        }

        /// <summary>
        ///     ��ȷ��ȫ��תΪ����
        /// </summary>
        /// <param name="str">�ı�����</param>
        /// <param name="def">�޷�ת��ʱ��ȱʡֵ</param>
        /// <returns>����</returns>
        public static int ToInteger(this string str, int def = 0)
        {
            if (str == null)
            {
                return def;
            }
            int re;
            return int.TryParse(str.Trim(), out re) ? re : def;
        }

        /// <summary>
        ///     ��ȷ��ȫ��תΪ��������
        /// </summary>
        /// <param name="str">�ı�����(������,�ֿ�)</param>
        /// <returns>��������</returns>
        public static int[] ToIntegers(this string str)
        {
            return str?.Trim().Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray() ?? new int[0];
        }

        /// <summary>
        ///     ��ȷ��ȫ��תΪ�ı�
        /// </summary>
        /// <param name="obj">����</param>
        /// <param name="def">�޷�ת��ʱ��ȱʡֵ</param>
        /// <returns>�ı�</returns>
        public static string ToSafeString(this object obj, string def = "")
        {
            return obj == null ? def : obj.ToString();
        }

        /// <summary>
        /// ��ȡö�����͵�ֵ����������
        /// </summary>
        /// <param name="enumObj">Ŀ��ö�ٶ���</param>
        /// <returns>ֵ�������ļ���</returns>
        public static Dictionary<int, string> GetEnumValAndDescList(this Enum enumObj)
        {
            Dictionary<int, string> enumDic = new Dictionary<int, string>();
            Type enumType = enumObj.GetType();
            List<int> enumValues = enumType.GetEnumValues().Cast<int>().ToList();

            enumValues.ForEach(item =>
            {
                int key = (int)item;
                string text = enumType.GetEnumName(key);
                string descText = enumType.GetField(text).GetCustomAttributes(typeof(DescriptionAttribute),
                    false).Cast<DescriptionAttribute>().FirstOrDefault()?.Description;

                text = string.IsNullOrWhiteSpace(descText) ? text : descText;

                enumDic.Add(key, text);
            });

            return enumDic;
        }

        /// <summary>
        /// ��ȡ�ض�ö��ֵ������
        /// </summary>
        /// <param name="enumObj">Ŀ��ö�ٶ���</param>
        /// <param name="val">ö��ֵ</param>
        /// <returns>ö��ֵ������</returns>
        public static string GetEnumValDesc(this Enum enumObj, object val)
        {
            Type enumType = enumObj.GetType();
            string text = enumType.GetEnumName(val);
            string descText = enumType.GetField(text).GetCustomAttributes(typeof(DescriptionAttribute),
                    false).Cast<DescriptionAttribute>().FirstOrDefault()?.Description;
            text = string.IsNullOrWhiteSpace(descText) ? text : descText;

            return text;
        }

        #region ���ʼ�

        /// <summary>
        /// �����ʼ�
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="reciver"></param>
        /// <param name="content"></param>
        public static void SendMail(string subject, string content, string reciver)
        {
            SendMail(subject, content, new List<string> { reciver });
        }

        /// <summary>
        /// �����ʼ�
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="recivers"></param>
        /// <param name="content"></param>
        public static void SendMail(string subject, string content, List<string> recivers)
        {
            var emailAcount = ConfigurationManager.AppSettings["EmailAcount"];
            var emailPassword = ConfigurationManager.AppSettings["EmailPassword"];
            var from = ConfigurationManager.AppSettings["FromEmail"];
            var smtp = ConfigurationManager.AppSettings["EmailSmtpAddress"];
            int port;
            if (!int.TryParse(ConfigurationManager.AppSettings["EmailSmtpPort"], out port))
                port = 25;
            MailMessage message = new MailMessage
            {
                From = new MailAddress(@from)//���÷�����,��������Ҫ�����õ��ʼ����ͷ�����������һ��
            };

            foreach (var reciver in recivers)
                message.To.Add(reciver);//�����ռ���

            message.Subject = subject; //�����ʼ�����

            message.Body = content;//�����ʼ�����

            SmtpClient client = new SmtpClient(smtp, port);//�����ʼ����ͷ�����
            //���÷����˵������˺ź�����
            client.Credentials = new NetworkCredential(emailAcount, emailPassword);
            //����ssl,Ҳ���ǰ�ȫ����
            client.EnableSsl = true;
            //�����ʼ�
            client.Send(message);
        }

        #endregion
    }
}