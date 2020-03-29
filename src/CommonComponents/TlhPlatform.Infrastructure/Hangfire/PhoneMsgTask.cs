using Hangfire.Server;
using System;
using System.Collections.Generic;
using System.Text;
using TlhPlatform.Infrastructure.MongoDB;

namespace TlhPlatform.Infrastructure.Hangfire
{
    /// <summary>
    /// 发送手机短信
    /// </summary>
    public class PhoneMsgTask : BaseSchedule
    {
        public PhoneMsgTask(IMongoRepository mongoRepository) : base(mongoRepository)
        { }
        public void HandlePhoneMsgSchedule(string msgId, PerformContext context)
        {
            SetMsgStateRuning(msgId);//更新任务状态为运行
            try
            {
                UpdateJobId(msgId, context.BackgroundJob.Id);
                //执行业务处理逻辑
                //发送手机短信
                System.Threading.Thread.Sleep(10000);
                //-------------------
                SetMsgStateSuccess(msgId);//更新任务状态为成功
            }
            catch (Exception ex)
            {
                SetMsgStateFailed(msgId);//更新任务状态为失败
                throw ex;
            }
        }
    }
}
