using System;
using System.Collections.Generic;
using System.Text;

namespace TlhPlatform.Core.Common
{
    public class WebLogError
    {
        public DateTime? LogTime { get; set; }

        public string LogSource { get; set; }

        public string LogLeve { get; set; }

        public string LogContext { get; set; }

        public string ExceptionMessage { get; set; }
        public string ExceptionType { get; set; }
        public string ExceptionMenthon { get; set; }
        public string ExceptionSource { get; set; }
        public string ExceptionStack { get; set; }
        public string ExceptionString { get; set; }


    }
}
