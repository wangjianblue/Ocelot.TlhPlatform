using Quartz;
using Quartz.Impl;
using Quartz.Impl.AdoJobStore;
using Quartz.Impl.AdoJobStore.Common;
using Quartz.Impl.Matchers;
using Quartz.Impl.Triggers;
using Quartz.Simpl;
using Quartz.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TlhPlatform.Infrastructure.Quartz.Entity;

namespace TlhPlatform.Infrastructure.Quartz.Common
{
    /// <summary>
    /// 调度中心
    /// </summary>
    public class SchedulerCenter : ISchedulerCenter
    {
        /// <summary>
        /// 任务调度对象
        /// </summary>
        private IScheduler Scheduler;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="DBString"></param>
        /// <param name="start">是否启动调度</param>
        public SchedulerCenter(string DBString)
        {
            if (Scheduler != null)
            {
                return;
            }
            //MySql存储
            DBConnectionManager.Instance.AddConnectionProvider("default", new DbProvider("MySql", DBString));
            var serializer = new JsonObjectSerializer();
            serializer.Initialize();
            var jobStore = new JobStoreTX
            {
                
                DataSource = "default",
                TablePrefix = "QRTZ_",
                InstanceId = "AUTO",
                DriverDelegateType = typeof(MySQLDelegate).AssemblyQualifiedName, //MySql存储
                ObjectSerializer = serializer,
                //指定调度引擎设置触发器超时的"临界值" 
                //参考：https://www.cnblogs.com/daxin/p/3919927.html
                MisfireThreshold = new TimeSpan(12, 0, 0)
            };
            
            //绑定线程池线程数
            DirectSchedulerFactory.Instance.CreateScheduler("Vocs" + "Scheduler", "AUTO", new DefaultThreadPool() { ThreadCount = 20 }, jobStore);

            Scheduler = SchedulerRepository.Instance.Lookup("Vocs" + "Scheduler").Result;
            
            
            //JobListener listener = new JobListener();
            //Scheduler.ListenerManager.AddJobListener(listener, GroupMatcher<JobKey>.AnyGroup());//Job监听
            //Scheduler.ListenerManager.AddTriggerListener(new TriggerListener(), GroupMatcher<TriggerKey>.AnyGroup());//Trigger监听
            //Scheduler.ListenerManager.AddSchedulerListener(new SchedulerListener());//scheduler监听

            //if(start)
            //Scheduler.Start();//默认开始调度器 
        }

        /// <summary>
        /// 添加一个工作调度
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<BaseResult> AddScheduleJobAsync(ScheduleEntity entity)
        {
            var result = new BaseResult();
            try
            {
                //检查任务是否已存在
                var jobKey = new JobKey(entity.JobName, entity.JobGroup);
                if (await Scheduler.CheckExists(jobKey))
                {
                    result.Code = 500;
                    result.Msg = "任务已存在";
                    return result;
                }
                //http请求配置
                var httpDir = new Dictionary<string, string>()
                {
                    { "RequestUrl",entity.RequestUrl},
                    { "RequestParameters",entity.RequestParameters},
                    { "RequestType", ((int)entity.RequestType).ToString()},
                    { "CompanyId",entity.CompanyId},
                    { "ClassName",entity.ClassName}
                };
                IJobDetail job = null;

                if (!string.IsNullOrEmpty(entity.JobType) && !string.IsNullOrEmpty(entity.ClassName))
                {
                    ////反射获取任务执行类  
                    //var jobType = FileHelper.GetAbsolutePath(entity.JobType, entity.JobType + "." + entity.ClassName);

                    //job = new JobDetailImpl(entity.JobName, entity.JobGroup, jobType).GetJobBuilder()
                    //  .SetJobData(new JobDataMap(httpDir))
                    //  .WithDescription(entity.Description)
                    //  .WithIdentity(entity.JobName, entity.JobGroup)
                    //  .Build();
                }
                

                // 创建触发器
                ITrigger trigger;
                //校验是否正确的执行周期表达式
                if (entity.TriggerType == TriggerTypeEnum.Cron)//CronExpression.IsValidExpression(entity.Cron))
                {
                    trigger = CreateCronTrigger(entity);
                }
                else
                {
                    trigger = CreateSimpleTrigger(entity);
                }

                // 告诉Quartz使用我们的触发器来安排作业
                await Scheduler.ScheduleJob(job, trigger);
                result.Code = 200;
            }
            catch (Exception ex)
            {
                result.Code = 505;
                result.Msg = ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 暂停/删除 指定的计划
        /// </summary>
        /// <param name="jobGroup">任务分组</param>
        /// <param name="jobName">任务名称</param>
        /// <param name="isDelete">停止并删除任务</param>
        /// <returns></returns>
        public async Task<BaseResult> StopOrDelScheduleJobAsync(string jobGroup, string jobName, bool isDelete = false)
        {
            BaseResult result;
            try
            {
                await Scheduler.PauseJob(new JobKey(jobName, jobGroup));
                if (isDelete)
                {
                    await Scheduler.DeleteJob(new JobKey(jobName, jobGroup));
                }
                result = new BaseResult
                {
                    Code = 200,
                    Msg = "停止任务计划成功！"
                };
            }
            catch (Exception ex)
            {
                result = new BaseResult
                {
                    Code = 505,
                    Msg = "停止任务计划失败"
                };
            }
            return result;
        }

        /// <summary>
        /// 恢复运行暂停的任务
        /// </summary>
        /// <param name="jobName">任务名称</param>
        /// <param name="jobGroup">任务分组</param>
        /// <returns></returns>
        public async Task<BaseResult> ResumeJobAsync(string jobGroup, string jobName)
        {
            BaseResult result = new BaseResult();
            try
            {
                //检查任务是否存在
                var jobKey = new JobKey(jobName, jobGroup);
                if (await Scheduler.CheckExists(jobKey))
                {
                    //任务已经存在则暂停任务
                    
                    await Scheduler.ResumeJob(jobKey);
                    result.Msg = "恢复任务计划成功！";

                    //Log.Logger.Default.Error(string.Format("任务“{0}”恢复运行", jobName));

                }
                else
                {
                    result.Msg = "任务不存在";
                }
                result.Code = 200;
            }
            catch (Exception ex)
            {
                result.Msg = "恢复任务计划失败！";
                result.Code = 505;
                //Log.Logger.Default.Error(string.Format("恢复任务失败！{0}", ex));
            }
            return result;
        }

        /// <summary>
        /// 查询任务
        /// </summary>
        /// <param name="jobGroup"></param>
        /// <param name="jobName"></param>
        /// <returns></returns>
        public async Task<ScheduleEntity> QueryJobAsync(string jobGroup, string jobName)
        {
            var entity = new ScheduleEntity();
            var jobKey = new JobKey(jobName, jobGroup);
            var jobDetail = await Scheduler.GetJobDetail(jobKey);
            var triggersList = await Scheduler.GetTriggersOfJob(jobKey);
            var triggers = triggersList.AsEnumerable().FirstOrDefault();
            var intervalSeconds = (triggers as SimpleTriggerImpl)?.RepeatInterval.TotalSeconds;
            entity.RequestUrl = jobDetail.JobDataMap.GetString(Constant.REQUESTURL);
            entity.BeginTime = triggers.StartTimeUtc.LocalDateTime;
            entity.EndTime = triggers.EndTimeUtc?.LocalDateTime;
            entity.IntervalSecond = intervalSeconds.HasValue ? Convert.ToInt32(intervalSeconds.Value) : 0;
            entity.JobGroup = jobGroup;
            entity.JobName = jobName;

            entity.JobType = jobDetail.JobType.Assembly.GetName().Name;
            entity.ClassName = jobDetail.JobDataMap.GetString("ClassName");
            entity.Cron = (triggers as CronTriggerImpl)?.CronExpressionString;
            entity.RunTimes = (triggers as SimpleTriggerImpl)?.RepeatCount;
            entity.TriggerType = triggers is SimpleTriggerImpl ? TriggerTypeEnum.Simple : TriggerTypeEnum.Cron;
            entity.RequestType = (RequestTypeEnum)int.Parse(jobDetail.JobDataMap.GetString(Constant.REQUESTTYPE));
            entity.RequestParameters = jobDetail.JobDataMap.GetString(Constant.REQUESTPARAMETERS);
            entity.Description = jobDetail.Description;
            return entity;
        }

        /// <summary>
        /// 立即执行
        /// </summary>
        /// <param name="jobKey"></param>
        /// <returns></returns>
        public async Task<bool> TriggerJobAsync(JobKey jobKey)
        {
            await Scheduler.TriggerJob(jobKey);
            return true;
        }

        /// <summary>
        /// 获取job日志
        /// </summary>
        /// <param name="jobKey"></param>
        /// <returns></returns>
        public async Task<List<string>> GetJobLogsAsync(JobKey jobKey)
        {
            var jobDetail = await Scheduler.GetJobDetail(jobKey);
            return jobDetail.JobDataMap[Constant.LOGLIST] as List<string>;
        }

        /// <summary>
        /// 获取Job异常日志
        /// </summary>
        /// <param name="jobKey"></param>
        /// <returns></returns>
        public async Task<List<string>> GetJobErrorsAsync(JobKey jobKey)
        {
            var jobDetail = await Scheduler.GetJobDetail(jobKey);
            return jobDetail.JobDataMap[Constant.EXCEPTION] as List<string>;
        }

        /// <summary>
        /// 获取所有Job（详情信息 - 初始化页面调用）
        /// </summary>
        /// <param name="compayId"></param>
        /// <returns></returns>
        public async Task<List<JobInfo>> GetAllJobAsync(string compayId)
        {
            List<JobKey> jboKeyList = new List<JobKey>();
            List<JobInfoEntity> jobInfoList = new List<JobInfoEntity>();
            List<JobInfo> GetAlljobInfo = new List<JobInfo>();
            var groupNames = await Scheduler.GetJobGroupNames();
            foreach (var groupName in groupNames.OrderBy(t => t))
            {
                jboKeyList.AddRange(await Scheduler.GetJobKeys(GroupMatcher<JobKey>.GroupEquals(groupName)));
                jobInfoList.Add(new JobInfoEntity() { GroupName = groupName });
            }
            foreach (var jobKey in jboKeyList.OrderBy(t => t.Name))
            {
                var jobDetail = await Scheduler.GetJobDetail(jobKey);
                var t = jobDetail.JobDataMap.GetString(Constant.COMPANYID);
                if (t != null && t != compayId)//不等于系统管理员
                {
                    continue;
                }

                var triggersList = await Scheduler.GetTriggersOfJob(jobKey);
                var triggers = triggersList.AsEnumerable().FirstOrDefault();

                var interval = string.Empty;
                if (triggers is SimpleTriggerImpl)
                    interval = (triggers as SimpleTriggerImpl)?.RepeatInterval.ToString();
                else
                    interval = (triggers as CronTriggerImpl)?.CronExpressionString;

                foreach (var jobInfo in jobInfoList)
                {
                    if (jobInfo.GroupName == jobKey.Group)
                    {
                        var job = new JobInfo()
                        {
                            GroupName = jobKey.Group,
                            Name = jobKey.Name,
                            TriggerType = triggers is SimpleTriggerImpl ? TriggerTypeEnum.Simple : TriggerTypeEnum.Cron,
                            LastErrMsg = jobDetail.JobDataMap.GetString(Constant.EXCEPTION),
                            RequestUrl = jobDetail.JobDataMap.GetString(Constant.REQUESTURL),
                            TriggerState = await Scheduler.GetTriggerState(triggers.Key),
                            PreviousFireTime = triggers.GetPreviousFireTimeUtc()?.LocalDateTime,
                            NextFireTime = triggers.GetNextFireTimeUtc()?.LocalDateTime,
                            BeginTime = triggers.StartTimeUtc.LocalDateTime,
                            Interval = interval,
                            EndTime = triggers.EndTimeUtc?.LocalDateTime,
                            Description = jobDetail.Description
                        };
                        jobInfo.JobInfoList.Add(job);

                        GetAlljobInfo.Add(job);
                        continue;
                    }
                }
            }
            return GetAlljobInfo;
        }

        /// <summary>
        /// 获取所有Job信息（简要信息 - 刷新数据的时候使用）
        /// </summary>
        /// <returns></returns>
        public async Task<List<JobInfoEntity>> GetAllJobBriefInfoAsync()
        {
            List<JobKey> jboKeyList = new List<JobKey>();
            List<JobInfoEntity> jobInfoList = new List<JobInfoEntity>();
            var groupNames = await Scheduler.GetJobGroupNames();
            foreach (var groupName in groupNames.OrderBy(t => t))
            {
                jboKeyList.AddRange(await Scheduler.GetJobKeys(GroupMatcher<JobKey>.GroupEquals(groupName)));
                jobInfoList.Add(new JobInfoEntity() { GroupName = groupName });
            }
            foreach (var jobKey in jboKeyList.OrderBy(t => t.Name))
            {
                var jobDetail = await Scheduler.GetJobDetail(jobKey);
                var triggersList = await Scheduler.GetTriggersOfJob(jobKey);
                var triggers = triggersList.AsEnumerable().FirstOrDefault();

                foreach (var jobInfo in jobInfoList)
                {
                    if (jobInfo.GroupName == jobKey.Group)
                    {
                        jobInfo.JobInfoList.Add(new JobInfo()
                        {
                            Name = jobKey.Name,
                            LastErrMsg = jobDetail.JobDataMap.GetString(Constant.EXCEPTION),
                            TriggerState = await Scheduler.GetTriggerState(triggers.Key),
                            PreviousFireTime = triggers.GetPreviousFireTimeUtc()?.LocalDateTime,
                            NextFireTime = triggers.GetNextFireTimeUtc()?.LocalDateTime,
                        });
                        continue;
                    }
                }
            }
            return jobInfoList;
        }

        /// <summary>
        /// 开启调度器
        /// </summary>
        /// <returns></returns>
        public async Task<bool> StartScheduleAsync()
        {
            await Scheduler.Start();
            //开启调度器
            //if (!Scheduler.InStandbyMode)
            //{
            //    await Scheduler.Start();
            //    Log.Logger.Default.Error("任务调度启动！");

            //}
            return Scheduler.InStandbyMode;
        }

        /// <summary>
        /// 停止任务调度
        /// </summary>
        public async Task<bool> StopScheduleAsync()
        {
            //判断调度是否已经关闭
            if (!Scheduler.InStandbyMode)
            {
                //等待任务运行完成
                await Scheduler.Standby(); //TODO  注意：Shutdown后Start会报错，所以这里使用暂停。

                //Logger.Default.Error("任务调度暂停！");
            }
            return !Scheduler.InStandbyMode;
        }

        /// <summary>
        /// 创建类型Simple的触发器
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private ITrigger CreateSimpleTrigger(ScheduleEntity entity)
        {
            //作业触发器
            if (entity.RunTimes.HasValue && entity.RunTimes > 0)
            {
                return TriggerBuilder.Create()
               .WithIdentity(entity.JobName, entity.JobGroup)
               .StartAt(entity.BeginTime)//开始时间
               .EndAt(entity.EndTime)//结束数据
               .WithSimpleSchedule(x => x
                   .WithIntervalInSeconds(entity.IntervalSecond.Value)//执行时间间隔，单位秒
                   .WithRepeatCount(entity.RunTimes.Value))//执行次数、默认从0开始
                   .ForJob(entity.JobName, entity.JobGroup)//作业名称
               .Build();
            }
            else
            {
                return TriggerBuilder.Create()
               .WithIdentity(entity.JobName, entity.JobGroup)
               .StartAt(entity.BeginTime)//开始时间
               .EndAt(entity.EndTime)//结束数据
               .WithSimpleSchedule(x => x
                   .WithIntervalInSeconds(entity.IntervalSecond.Value)//执行时间间隔，单位秒
                   .RepeatForever())//无限循环
                   .ForJob(entity.JobName, entity.JobGroup)//作业名称
               .Build();
            }
        }

        /// <summary>
        /// 创建类型Cron的触发器
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private ITrigger CreateCronTrigger(ScheduleEntity entity)
        {
            //忽略掉调度暂停过程中没有执行的调度
            //https://www.jianshu.com/p/aa51844523c9
           // CronScheduleBuilder cronScheduleBuilder = CronScheduleBuilder.CronSchedule(entity.Cron).WithMisfireHandlingInstructionDoNothing();
            //ITrigger cronTrigger = TriggerBuilder.Create().WithIdentity(entity.JobName, entity.JobGroup).WithSchedule(cronScheduleBuilder).Build();


            // 作业触发器
            return TriggerBuilder.Create()
                   .WithIdentity(entity.JobName, entity.JobGroup)
                   .StartAt(entity.BeginTime)//开始时间
                   .EndAt(entity.EndTime)//结束时间
                   .WithCronSchedule(entity.Cron)//指定cron表达式
                   .ForJob(entity.JobName, entity.JobGroup)//作业名称
                   .Build();
        }
    }
}
