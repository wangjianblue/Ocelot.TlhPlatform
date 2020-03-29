//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace TlhPlatform.Infrastructure.Log
//{
//    /// <summary>
//    /// 日志记录接口
//    /// </summary>
//    public class Logger
//    {
//        NLog.Logger _logger;


//        /// <summary>
//        /// 构造器
//        /// </summary>
//        /// <param name="logger"></param>
//        private Logger(NLog.Logger logger)
//        {
//            _logger = logger;

//        }
//        /// <summary>
//        /// 构造器
//        /// </summary>
//        /// <param name="logger"></param>
//        public Logger(string name) : this(LogManager.GetLogger(name))
//        {

//        }

//        public static Logger Default { get; private set; }

//        public static bool IsDebugEnabled { get; set; }
//        /// <summary>
//        /// 构造器
//        /// </summary>
//        /// <param name="logger"></param>
//        static Logger()
//        {

//            Default = new Logger(NLog.LogManager.GetCurrentClassLogger());
//        }

//        #region Debug
//        /// <summary>
//        /// Debug调试日志
//        /// </summary>
//        /// <param name="msg"></param>
//        /// <param name="args"></param>
//        public void Debug(string msg, params object[] args)
//        {
//            if (!_logger.IsDebugEnabled)
//                return;
//            _logger.Debug(msg, args);
//        }
//        /// <summary>
//        /// 调试日志
//        /// </summary>
//        /// <param name="msg"></param>
//        /// <param name="err"></param>
//        public void Debug(string msg, Exception err)
//        {
//            if (!_logger.IsDebugEnabled)
//                return;
//            _logger.Debug(err, msg);
//        }
//        #endregion

//        #region Info
//        /// <summary>
//        /// 访问日志
//        /// </summary>
//        /// <param name="msg"></param>
//        /// <param name="args"></param>
//        public void Info(string msg, params object[] args)
//        {
//            if (!_logger.IsInfoEnabled)
//                return;
//            _logger.Info(msg, args);
//        }
//        /// <summary>
//        /// 访问日志
//        /// </summary>
//        /// <param name="msg"></param>
//        /// <param name="args"></param>
//        public void Info(string msg, Exception err)
//        {
//            if (!_logger.IsInfoEnabled)
//                return;
//            _logger.Info(err, msg);
//        }
//        #endregion

//        #region Warn
//        /// <summary>
//        /// 警告日志
//        /// </summary>
//        /// <param name="msg"></param>
//        /// <param name="args"></param>
//        public void Warn(string msg, params object[] args)
//        {
//            if (!_logger.IsWarnEnabled)
//                return;
//            _logger.Warn(msg, args);
//        }
//        /// <summary>
//        /// 警告日志
//        /// </summary>
//        /// <param name="msg"></param>
//        /// <param name="args"></param>
//        public void Warn(string msg, Exception err)
//        {
//            if (!_logger.IsWarnEnabled)
//                return;
//            _logger.Warn(err, msg);
//        }
//        #endregion

//        #region Trace
//        /// <summary>
//        /// 警告日志
//        /// </summary>
//        /// <param name="msg"></param>
//        /// <param name="args"></param>
//        public void Trace(string msg, params object[] args)
//        {
//            if (!_logger.IsTraceEnabled)
//                return;
//            _logger.Trace(msg, args);
//        }
//        /// <summary>
//        /// 警告日志
//        /// </summary>
//        /// <param name="msg"></param>
//        /// <param name="args"></param>
//        public void Trace(string msg, Exception err)
//        {
//            if (!_logger.IsTraceEnabled)
//                return;
//            _logger.Trace(err, msg);
//        }
//        #endregion

//        #region Error
//        /// <summary>
//        /// 错误日志
//        /// </summary>
//        /// <param name="msg"></param>
//        /// <param name="args"></param>
//        public void Error(string msg, params object[] args)
//        {
//            if (!_logger.IsErrorEnabled)
//                return;
//            _logger.Error(msg, args);
//        }
//        /// <summary>
//        /// 错误日志
//        /// </summary>
//        /// <param name="msg"></param>
//        /// <param name="args"></param>
//        public void Error(string msg, Exception err)
//        {
//            if (!_logger.IsErrorEnabled)
//                return;
//            _logger.Error(err, msg);
//        }
//        #endregion

//        #region Fatal
//        public void Fatal(string msg, params object[] args)
//        {
//            if (!_logger.IsFatalEnabled)
//                return;
//            _logger.Fatal(msg, args);
//        }

//        public void Fatal(string msg, Exception err)
//        {
//            if (!_logger.IsFatalEnabled)
//                return;
//            _logger.Fatal(err, msg);
//        }
//        #endregion

//        #region Custom
//        /// <summary>
//        /// 数据库写入日志
//        /// </summary>
//        /// <param name="log"></param>
//        public void Process(Log log)
//        {
//            var _dblogger = LogManager.GetLogger("DbLogger");
//            LogEventInfo ei = new LogEventInfo();
//            ei.Properties["Desc"] = "我是自定义消息";
//            _dblogger.Info(ei);
//            _dblogger.Debug(ei);
//            _dblogger.Trace(ei);

//        }
//        #endregion

//        /// <summary>
//        /// Flush any pending log messages (in case of asynchronous targets).
//        /// </summary>
//        /// <param name="timeoutMilliseconds">Maximum time to allow for the flush. Any messages after that time will be discarded.</param>
//        public void Flush(int? timeoutMilliseconds = null)
//        {
//            if (timeoutMilliseconds != null)
//                NLog.LogManager.Flush(timeoutMilliseconds.Value);

//            NLog.LogManager.Flush();
//        }

//    }
//}
