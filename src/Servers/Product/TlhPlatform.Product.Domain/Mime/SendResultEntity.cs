﻿using System;
using System.Collections.Generic;
using System.Text;

namespace TlhPlatform.Product.Domain.Mime
{
    /// <summary>
    ///  邮件发送结果
    /// </summary>
    public class SendResultEntity
    {
        /// <summary>
        /// 结果信息
        /// </summary>
        public string ResultInformation { get; set; } = "发送成功！";

        /// <summary>
        /// 结果状态
        /// </summary>
        public bool ResultStatus { get; set; } = true;
    }
}
