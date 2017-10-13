using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using Agebull.Common.DataModel.Redis;
using Gboxt.Common.DataModel;
using Gboxt.Common.DataModel.BusinessLogic;
using Gboxt.Common.DataModel.MySql;
using Gboxt.Common.Workflow.BusinessLogic;
using Gboxt.Common.Workflow.DataAccess;

namespace Gboxt.Common.Workflow
{
    /// <summary>
    ///     ��ʾһ��������ҵ���߼�����
    /// </summary>
    public class AutoJobBusinessLogicBase<TData, TAccess> : BusinessLogicByAudit<TData, TAccess>
        where TData : AutoJobEntity, IIdentityData, IWorkflowData, IHistoryData, IAuditData, IStateData, new()
        where TAccess : HitoryTable<TData>, new()
    {
        /// <summary>
        /// ����
        /// </summary>
        protected AutoJobBusinessLogicBase()
        {
            unityStateChanged = true;
        }

        /// <summary>
        ///     ���뵱ǰ����������
        /// </summary>
        public override TData Details(int id)
        {
            var data = base.Details(id);
            CheckCanAudit(data);
            return data;
        }

        private UserJobDataAccess _userJobDataAccess;

        /// <summary>
        /// �û��������ݷ�����
        /// </summary>
        protected UserJobDataAccess UserJobDataAccess => _userJobDataAccess ?? (_userJobDataAccess = new UserJobDataAccess
        {
            DataBase = Access.DataBase
        });
        /// <summary>
        /// ��������Ϣ
        /// </summary>
        /// <param name="data"></param>
        public void CheckCanAudit(TData data)
        {
            if (data == null || data.AuditState < AuditStateType.Submit)
                return;
            var jobs = UserJobDataAccess.All(p => p.EntityType == EntityType && p.DataId == data.Id && p.JobType == UserJobType.Audit);
            if (jobs.Count == 0)
                return;
            data.ToUsers = jobs.Select(p => $"{p.ToUserName}({p.JobStatus.ToCaption() })").LinkToString("��");
            var me = jobs.Any(p => p.ToUserId == BusinessContext.Current.LoginUserId &&
                                   p.JobStatus == JobStatusType.None);
            if (me)
                data.CanAudit = true;
        }


        /// <summary>
        ///     �ܷ�ͨ�����)���ж�
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected override bool CanAuditPass(TData data)
        {
            if (!CheckJobCanDo(data))
                return false;
            return base.CanAuditPass(data);
        }
        /// <summary>
        /// �����������
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private bool CheckJobCanDo(TData data)
        {
            var jobs = UserJobDataAccess.All(p => p.EntityType == EntityType && p.DataId == data.Id &&
                                                  p.JobType == UserJobType.Audit);
            if (jobs.Count == 0)
            {
                return true;
            }
            if (jobs.All(p => p.ToUserId != BusinessContext.Current.LoginUserId))
            {
                BusinessContext.Current.LastMessage = $"�㲻��{data.Title}��������֮��";
                return false;
            }
            return true;
        }
        /// <summary>
        /// �ύ
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected override bool DoSubmit(TData data)
        {
            var supper = data as ILevelAuditData;
            if (supper != null)
                supper.DepartmentLevel -= 1;
            return base.DoSubmit(data);
        }

        /// <summary>
        ///     ִ����˵���չ����
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected override bool DoAuditPass(TData data)
        {
            var jobs = UserJobDataAccess.All(p => p.EntityType == EntityType && p.DataId == data.Id && p.JobType == UserJobType.Audit);
            var nomy = jobs.Where(p => p.ToUserId != BusinessContext.Current.LoginUserId).ToList();
            if (nomy.Count(p => p.JobStatus == JobStatusType.Succeed) != nomy.Count)
            {
                data.AuditState = AuditStateType.Submit;
                return base.DoAuditPass(data);
            }
            var supper = data as ILevelAuditData;
            if (supper == null || supper.LastLevel >= supper.DepartmentLevel)
                return base.DoAuditPass(data);
            if (supper.DepartmentLevel > 1)
                supper.DepartmentLevel -= 1;
            var users = GetUpAuditUsers(supper.DepartmentLevel);
            if (users.Length == 0)
                return base.DoAuditPass(data);
            UserJobData job = jobs.FirstOrDefault();
            if (job == null)
            {
                job = new UserJobData
                {
                    EntityType = EntityType,
                    LinkId = data.Id,
                    Title = data.Title,
                    JobType = UserJobType.Audit,
                    FromUserId = BusinessContext.Current.LoginUserId,
                    FromUserName = BusinessContext.Current.LoginUser.RealName
                };
            }
            else
            {
                job.JobStatus = JobStatusType.None;
                job.DataState = DataStateType.None;
                job.IsFreeze = false;
            }
            var bl = new UserJobBusinessLogic();
            foreach (var user in users)
            {
                job.Id = 0;
                job.ToUserId = user;
                bl.AddNew(job);
            }
            data.AuditState = AuditStateType.Submit;
            return base.DoAuditPass(data);
        }

        /// <summary>
        /// ȡ����һ���ύ��
        /// </summary>
        private int[] GetUpAuditUsers(int level)
        {
            using (var proxy = new RedisProxy(RedisProxy.DbSystem))
            {
                return proxy.Get($"audit:page:users:ids:{BusinessContext.Current.PageItem.Id}:{level}").ToIntegers();
            }
        }
        /// <summary>
        ///     �ڲ�����ִ����ɺ�Ĵ���(unityStateChanged������Ϊtrueʱ�����������--�������ܵĿ���)
        /// </summary>
        /// <param name="data">����</param>
        /// <param name="cmd">����</param>
        protected override void OnInnerCommand(TData data, BusinessCommandType cmd)
        {
            UserJobTrigger<TData>.OnDataChanged(data, EntityType, cmd, BusinessContext.Current.LoginUserId);
        }

    }
}