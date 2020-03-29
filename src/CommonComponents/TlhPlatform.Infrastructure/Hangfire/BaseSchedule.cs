
using System;
using TlhPlatform.Infrastructure.MongoDB;
using TlhPlatform.Infrastructure.MongoDB.Base;
 

namespace TlhPlatform.Infrastructure.Hangfire
{
    public abstract class BaseSchedule
    {
        static IMongoRepository _mongoRepository;
        public BaseSchedule(IMongoRepository mongoRepository)
        {
            _mongoRepository = mongoRepository;
        }
        /// <summary>
        /// 修改任务消息状态为运行
        /// </summary>
        /// <param name="msgId"></param>
        public static void SetMsgStateRuning(string msgId)
        {
            SetMsgState(msgId, MsgState.Runing);
        }
        /// <summary>
        /// 修改任务消息状态为成功
        /// </summary>
        /// <param name="msgId"></param>
        public static void SetMsgStateSuccess(string msgId)
        {
            SetMsgState(msgId, MsgState.Success);
        }
        /// <summary>
        /// 修改任务消息状态为失败
        /// </summary>
        /// <param name="msgId"></param>
        public static void SetMsgStateFailed(string msgId)
        {
            SetMsgState(msgId, MsgState.Failed);
        }
        /// <summary>
        /// 更新任务JobId
        /// </summary>
        /// <param name="msgId"></param>
        /// <param name="jobId"></param>
        public static void UpdateJobId(string msgId, string jobId)
        {
            TaskMessage msg;
            do
            {
                msg = _mongoRepository.Get<TaskMessage>(x => x.Id == msgId);
                if (msg == null)
                {
                    System.Threading.Thread.Sleep(1000);
                }
            }
            while (msg == null);
            msg.LastJobID = jobId;
            _mongoRepository.Update<TaskMessage>(x => x.Id == msgId, msg);
        }
        private static void SetMsgState(string msgId, MsgState state)
        {
            TaskMessage msg;
            DateTime start = DateTime.Now;
            do
            {
                msg = _mongoRepository.Get<TaskMessage>(x => x.Id == msgId);
                if (msg == null)
                {
                    System.Threading.Thread.Sleep(1000);
                }
                if ((DateTime.Now - start).TotalMinutes > 2)
                {
                    throw new Exception("从MongoDB获取不到消息实体");
                }
            }
            while (msg == null);
            msg.State = state;
            if (state == MsgState.Runing)
            {
                msg.RunCount += 1;
                msg.LastRunTime = DateTime.Now;
            }
            _mongoRepository.Update<TaskMessage>(x => x.Id == msgId, msg);
           
        }
    }
}
