using System;

namespace Agebull.SystemAuthority.Organizations
{
    /// <summary>
    ///     ��������
    /// </summary>
    public enum OrganizationType
    {
        /// <summary>
        ///     δȷ��
        /// </summary>
        None,
        /// <summary>
        ///     ��������
        /// </summary>
        Area,
        /// <summary>
        ///     ����
        /// </summary>
        Organization,
        
        /// <summary>
        ///     ����
        /// </summary>
        Department
    }

    /// <summary>
    /// ְλ�����ӽ�����
    /// </summary>
    public enum PositionDataScopeType
    {
        /// <summary>
        /// Ĭ��ֵ��û���κ�Ȩ����
        /// </summary>
        None,
        /// <summary>
        /// ���ޱ��˵�����
        /// </summary>
        Self,
        /// <summary>
        /// �����ŵ�����
        /// </summary>
        Department,
        /// <summary>
        /// �����ż��¼�������
        /// </summary>
        DepartmentAndLower,
        /// <summary>
        /// ����˾������
        /// </summary>
        Company,
        /// <summary>
        /// ����˾���¼�������
        /// </summary>
        CompanyAndLower,

        /// <summary>
        /// �Զ���
        /// </summary>
        Custom
    }
    public static class EnumHelperOrg
    {


        /// <summary>
        ///     Ȩ�޷�Χö����������ת��
        /// </summary>
        public static string ToCaption(this PositionDataScopeType value)
        {
            switch (value)
            {
                case PositionDataScopeType.None:
                    return "û���κ�Ȩ����";
                case PositionDataScopeType.Self:
                    return "���ޱ��˵�����";
                case PositionDataScopeType.Department:
                    return "�����ŵ�����";
                case PositionDataScopeType.DepartmentAndLower:
                    return "�����ż��¼�������";
                case PositionDataScopeType.Company:
                    return "�����������";
                case PositionDataScopeType.CompanyAndLower:
                    return "�������¼������벿�ŵ�����";
                case PositionDataScopeType.Custom:
                    return "�Զ���";
                default:
                    return "Ȩ�޷�Χö������(δ֪)";
            }
        }

        /// <summary>
        ///     ������������ת��
        /// </summary>
        public static string ToCaption(this OrganizationType value)
        {
            switch (value)
            {
                case OrganizationType.None:
                    return "δȷ��";
                case OrganizationType.Area:
                    return "��������";
                case OrganizationType.Organization:
                    return "����";
                case OrganizationType.Department:
                    return "����";
                default:
                    return "��������(δ֪)";
            }
        }
    }
}