using Quartz;
using System;
using System.Collections.Generic;

namespace TlhPlatform.Infrastructure.Quartz.Entity
{
    public class JobInfoEntity
    {
        /// <summary>
        /// 任务组名
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// 任务信息
        /// </summary>
        public List<JobInfo> JobInfoList { get; set; } = new List<JobInfo>();
    }

    public class JobInfo
    {
        /// <summary>
        /// 任务组名
        /// </summary>
        public string GroupName { get; set; }
        /// <summary>
        /// 任务名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 下次执行时间
        /// </summary>
        public DateTime? NextFireTime { get; set; }

        /// <summary>
        /// 上次执行时间
        /// </summary>
        public DateTime? PreviousFireTime { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime BeginTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// 上次执行的异常信息
        /// </summary>
        public string LastErrMsg { get; set; }

        /// <summary>
        /// 任务状态
        /// </summary>
        public TriggerState TriggerState { get; set; }
        /// <summary>
        /// Job类型
        /// </summary>
        public string JobType { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }


        /// <summary>
        /// 触发器类型
        /// </summary>
        public TriggerTypeEnum TriggerType { get; set; }

        /// <summary>
        /// 显示状态
        /// </summary>
        public string DisplayState
        {
            get
            {
                var state = string.Empty;
                switch (TriggerState)
                {
                    case TriggerState.Normal:
                        state = "正常";
                        break;
                    case TriggerState.Paused:
                        state = "暂停";
                        break;
                    case TriggerState.Complete:
                        state = "完成";
                        break;
                    case TriggerState.Error:
                        state = "异常";
                        break;
                    case TriggerState.Blocked:
                        state = "阻塞";
                        break;
                    case TriggerState.None:
                        state = "不存在";
                        break;
                    default:
                        state = "未知";
                        break;
                }
                return state;
            }
        }

        /// <summary>
        /// 时间间隔
        /// </summary>
        public string Interval { get; set; }

        /// <summary>
        /// job调用外部的url
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 状态， 0 暂停任务；1 启用任务
        /// </summary>
        public int JobStatus { get; set; }

        /// <summary>
        /// 任务所在DLL对应的程序集名称
        /// </summary>
        public string AssemblyName { get; set; }

        /// <summary>
        /// 任务所在类
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        /// 执行次数
        /// </summary>
        public int? RunTimes { get; set; }

        /// <summary>
        /// Cron表达式
        /// </summary>
        public string Cron { get; set; }

        /// <summary>
        /// 执行间隔时间，单位秒（如果有Cron，则IntervalSecond失效）
        /// </summary>
        public int? IntervalSecond { get; set; }

        /// <summary>
        /// 请求API的地址
        /// </summary>
        public string RequestUrl { get; set; }
    }
}
