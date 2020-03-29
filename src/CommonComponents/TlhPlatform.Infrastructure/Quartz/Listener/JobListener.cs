using Quartz;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace TlhPlatform.Infrastructure.Quartz.Listener
{
    /// <summary>
    /// JOB监听器，可以根据job实现并绑定任务即可
    /// </summary>
    public class JobListener : IJobListener
    {
        public string Name => "JobListener";
        public static int count = 0;
        //job开始执行之前调用
        public async Task JobExecutionVetoed(IJobExecutionContext context, CancellationToken cancellationToken = default(CancellationToken))
        {
            //具体动作自己根据需要实现
            await Console.Out.WriteLineAsync("job开始执行之前调用");
        }

        /// <summary>
        /// job每次执行之后调用
        /// </summary>
        /// <param name="context"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task JobToBeExecuted(IJobExecutionContext context, CancellationToken cancellationToken = default(CancellationToken))
        {
            //具体动作自己根据需要实现
            await Console.Out.WriteLineAsync("job每次执行之后调用");
        }

        //job执行结束之后调用
        public async Task JobWasExecuted(IJobExecutionContext context, JobExecutionException jobException, CancellationToken cancellationToken = default(CancellationToken))
        {
            count++;
            await Console.Out.WriteLineAsync("job执行结束之后调用  " + count);
            //通知客户端获取任务状态
            //var client = ServiceLocator.Instance.GetService<ISignalRClientManage>();
            //client.TaskScheduleClientInit().InvokeAsync("RefreshAllTaskState");
            //具体动作自己根据需要实现
        }
    }
}
