using Hangfire.Server;
using System;
using TlhPlatform.Infrastructure.MongoDB;

namespace TlhPlatform.Infrastructure.Hangfire
{
    /// <summary>
    /// 发送邮件（扩展其他业务请参照此格式填写处理逻辑）
    /// </summary>
    public class MailTask:BaseSchedule
    {
        public MailTask(IMongoRepository mongoRepository):base(mongoRepository)
        { }
        public void HandleMailSchedule(string msgId, PerformContext context)
        {
            SetMsgStateRuning(msgId);//更新任务状态为运行
            try
            {
                UpdateJobId(msgId, context.BackgroundJob.Id);
                //执行业务处理逻辑
                //发送邮件
                System.Threading.Thread.Sleep(10000);
                //-------------------
                SetMsgStateSuccess(msgId);//更新任务状态为成功
            }
            catch(Exception ex)
            {
                SetMsgStateFailed(msgId);//更新任务状态为失败
                throw ex;
            }
        }
    }
}
