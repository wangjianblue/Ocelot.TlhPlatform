using Quartz;
using System.Collections.Generic;
using System.Threading.Tasks;
using TlhPlatform.Infrastructure.Quartz.Entity;

namespace TlhPlatform.Infrastructure.Quartz.Common
{
    public interface ISchedulerCenter
    {
        /// <summary>
        /// 添加一个工作调度
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<BaseResult> AddScheduleJobAsync(ScheduleEntity entity);
        /// <summary>
        /// 获取所有Job（详情信息 - 初始化页面调用）
        /// </summary>
        /// <param name="compayId"></param>
        /// <returns></returns>
        Task<List<JobInfo>> GetAllJobAsync(string compayId);
        /// <summary>
        /// 获取所有Job信息（简要信息 - 刷新数据的时候使用）
        /// </summary>
        /// <returns></returns>
        Task<List<JobInfoEntity>> GetAllJobBriefInfoAsync();
        /// <summary>
        /// 获取job日志
        /// </summary>
        /// <param name="jobKey"></param>
        /// <returns></returns>
        Task<List<string>> GetJobLogsAsync(JobKey jobKey);
        /// <summary>
        /// 获取Job异常日志
        /// </summary>
        /// <param name="jobKey"></param>
        /// <returns></returns>
        Task<List<string>> GetJobErrorsAsync(JobKey jobKey);
        /// <summary>
        /// 查询任务
        /// </summary>
        /// <param name="jobGroup"></param>
        /// <param name="jobName"></param>
        /// <returns></returns>
        Task<ScheduleEntity> QueryJobAsync(string jobGroup, string jobName);
        /// <summary>
        /// 恢复运行暂停的任务
        /// </summary>
        /// <param name="jobName">任务名称</param>
        /// <param name="jobGroup">任务分组</param>
        /// <returns></returns>
        Task<BaseResult> ResumeJobAsync(string jobGroup, string jobName);
        /// <summary>
        /// 开启调度器
        /// </summary>
        /// <returns></returns>
        Task<bool> StartScheduleAsync();
        /// <summary>
        /// 暂停/删除 指定的计划
        /// </summary>
        /// <param name="jobGroup">任务分组</param>
        /// <param name="jobName">任务名称</param>
        /// <param name="isDelete">停止并删除任务</param>
        /// <returns></returns>
        Task<BaseResult> StopOrDelScheduleJobAsync(string jobGroup, string jobName, bool isDelete = false);
        /// <summary>
        /// 停止任务调度
        /// </summary>
        /// <returns></returns>
        Task<bool> StopScheduleAsync();
        /// <summary>
        /// 立即执行
        /// </summary>
        /// <param name="jobKey"></param>
        /// <returns></returns>
        Task<bool> TriggerJobAsync(JobKey jobKey);
    }
}