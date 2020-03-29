using Quartz;
using System.Threading;
using System.Threading.Tasks;

namespace TlhPlatform.Infrastructure.Quartz.Listener
{
    /// <summary>
    /// 任务调度监听器
    /// </summary>
    class SchedulerListener : ISchedulerListener
    {
        /// <summary>
        /// 任务被动态添加时执行
        /// </summary>
        /// <param name="jobDetail"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task JobAdded(IJobDetail jobDetail, CancellationToken cancellationToken = default(CancellationToken))
        {
            //具体动作自己根据需要实现
            return Task.FromResult(0);
        }

        /// <summary>
        /// 任务被删除时执行
        /// </summary>
        /// <param name="jobKey"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task JobDeleted(JobKey jobKey, CancellationToken cancellationToken = default(CancellationToken))
        {
            //具体动作自己根据需要实现
            return Task.FromResult(0);
        }

        /// <summary>
        /// 任务被中断时执行
        /// </summary>
        /// <param name="jobKey"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task JobInterrupted(JobKey jobKey, CancellationToken cancellationToken = default(CancellationToken))
        {
            //具体动作自己根据需要实现
            return Task.FromResult(0);
        }

        /// <summary>
        /// 任务被暂停时执行
        /// </summary>
        /// <param name="jobKey"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task JobPaused(JobKey jobKey, CancellationToken cancellationToken = default(CancellationToken))
        {
            //具体动作自己根据需要实现
            return Task.FromResult(0);
        }

        /// <summary>
        /// 任务被恢复时执行
        /// </summary>
        /// <param name="jobKey"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task JobResumed(JobKey jobKey, CancellationToken cancellationToken = default(CancellationToken))
        {
            //具体动作自己根据需要实现
            return Task.FromResult(0);
        }

        /// <summary>
        /// 任务被部署时执行
        /// </summary>
        /// <param name="trigger"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task JobScheduled(ITrigger trigger, CancellationToken cancellationToken = default(CancellationToken))
        {
            //具体动作自己根据需要实现
            return Task.FromResult(0);
        }

        /// <summary>
        /// 一组任务被暂停时执行
        /// </summary>
        /// <param name="jobGroup"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task JobsPaused(string jobGroup, CancellationToken cancellationToken = default(CancellationToken))
        {
            //具体动作自己根据需要实现
            return Task.FromResult(0);
        }

        /// <summary>
        /// 一组任务被恢复时执行
        /// </summary>
        /// <param name="jobGroup"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task JobsResumed(string jobGroup, CancellationToken cancellationToken = default(CancellationToken))
        {
            //具体动作自己根据需要实现
            return Task.FromResult(0);
        }

        /// <summary>
        /// 任务被卸载时执行
        /// </summary>
        /// <param name="triggerKey"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task JobUnscheduled(TriggerKey triggerKey, CancellationToken cancellationToken = default(CancellationToken))
        {
            //具体动作自己根据需要实现
            return Task.FromResult(0);
        }

        /// <summary>
        /// scheduler出现异常时执行
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="cause"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task SchedulerError(string msg, SchedulerException cause, CancellationToken cancellationToken = default(CancellationToken))
        {
            //具体动作自己根据需要实现
            return Task.FromResult(0);
        }

        /// <summary>
        /// scheduler被设为standBy等候模式时执行
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task SchedulerInStandbyMode(CancellationToken cancellationToken = default(CancellationToken))
        {
            //具体动作自己根据需要实现
            return Task.FromResult(0);
        }

        /// <summary>
        /// scheduler关闭时执行
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task SchedulerShutdown(CancellationToken cancellationToken = default(CancellationToken))
        {
            //具体动作自己根据需要实现
            return Task.FromResult(0);
        }

        /// <summary>
        /// scheduler正在关闭时执行
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task SchedulerShuttingdown(CancellationToken cancellationToken = default(CancellationToken))
        {
            //具体动作自己根据需要实现
            return Task.FromResult(0);
        }

        /// <summary>
        /// scheduler启动时执行
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task SchedulerStarted(CancellationToken cancellationToken = default(CancellationToken))
        {
            //具体动作自己根据需要实现
            return Task.FromResult(0);
        }

        /// <summary>
        /// scheduler正在启动时执行
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task SchedulerStarting(CancellationToken cancellationToken = default(CancellationToken))
        {
            //具体动作自己根据需要实现
            return Task.FromResult(0);
        }

        /// <summary>
        /// scheduler中所有数据包括jobs, triggers和calendars都被清空时执行
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task SchedulingDataCleared(CancellationToken cancellationToken = default(CancellationToken))
        {
            //具体动作自己根据需要实现
            return Task.FromResult(0);
        }

        /// <summary>
        /// 触发器完成了它的使命，光荣退休时执行
        /// </summary>
        /// <param name="trigger"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task TriggerFinalized(ITrigger trigger, CancellationToken cancellationToken = default(CancellationToken))
        {
            //具体动作自己根据需要实现
            return Task.FromResult(0);
        }

        /// <summary>
        /// 触发器暂停时执行
        /// </summary>
        /// <param name="triggerKey"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task TriggerPaused(TriggerKey triggerKey, CancellationToken cancellationToken = default(CancellationToken))
        {
            //具体动作自己根据需要实现
            return Task.FromResult(0);
        }

        /// <summary>
        /// 触发器恢复时执行
        /// </summary>
        /// <param name="triggerKey"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task TriggerResumed(TriggerKey triggerKey, CancellationToken cancellationToken = default(CancellationToken))
        {
            //具体动作自己根据需要实现
            return Task.FromResult(0);
        }

        /// <summary>
        /// 一组触发器暂停时执行
        /// </summary>
        /// <param name="triggerGroup"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task TriggersPaused(string triggerGroup, CancellationToken cancellationToken = default(CancellationToken))
        {
            //具体动作自己根据需要实现
            return Task.FromResult(0);
        }

        /// <summary>
        /// 一组触发器恢复时执行
        /// </summary>
        /// <param name="triggerGroup"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task TriggersResumed(string triggerGroup, CancellationToken cancellationToken = default(CancellationToken))
        {
            //具体动作自己根据需要实现
            return Task.FromResult(0);
        }
    }
}
