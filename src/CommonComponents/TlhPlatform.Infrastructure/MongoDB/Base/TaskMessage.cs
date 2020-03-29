using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace TlhPlatform.Infrastructure.MongoDB.Base
{
    /// <summary>
    /// 任务消息实体
    /// </summary>
    public class TaskMessage : MongoEntity
    {
        /// <summary>
        /// 交换机
        /// </summary>
        public string Exchange { get; set; }
        /// <summary>
        /// 交换Key
        /// </summary>
        public string Route { get; set; }
        /// <summary>
        /// 发送时间
        /// </summary>
        public DateTime SendTime { get; set; }

        public Dictionary<string, object> Headers { get; set; }

        public byte[] Body { get; set; }

        public MsgState State { get; set; }

        public int RunCount { get; set; }

        public DateTime LastRunTime { get; set; }

        public string LastJobID { get; set; }

        public MsgType MsgType { get; set; }

        public RunType RunType { get; set; }

        /// <summary>
        /// 延迟时间(延迟执行时有效)
        /// </summary>
        public TimeSpan DelayTime { get; set; }
        /// <summary>
        /// 定期执行参数(定期执行时有效，长度固定为6，参数顺序月、日、小时、分钟、秒、周<周天=0 周一=1 依次类推>)
        /// </summary>
        public int[] RecurringParam { get; set; }
    }
    /// <summary>
    /// 状态
    /// </summary>
    public enum MsgState
    {
        [Description("等待处理")]
        Await = 1,
        [Description("运行中")]
        Runing = 2,
        [Description("成功")]
        Success = 3,
        [Description("失败")]
        Failed = 4
    }
    /// <summary>
    /// 类型
    /// </summary>
    public enum MsgType
    {
        [Description("发送邮件")]
        SendMail = 1,
        [Description("发送短信")]
        SendSMS = 2,
        [Description("计算排放量")]
        CalculatedDischarge = 3,
    }
    /// <summary>
    /// 执行方式
    /// </summary>
    public enum RunType
    {
        [Description("单次执行")]
        Oncy = 1,
        [Description("定期执行")]
        Regular = 2,
        [Description("延迟执行")]
        Delay = 3
    }
}