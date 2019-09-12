﻿using System;
using System.Collections.Generic;
using System.Text;
using EasyNetQ;

namespace TlhPlatform.Infrastructure.EasyNetQ
{
    [Queue("Qka.Client", ExchangeName = "Qka.Client")]
    public class ClientMessage
    {
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public string Sex { get; set; }
        public int Age { get; set; }
        // N: Non-Smoker, S: Smoker
        public string SmokerCode { get; set; }
        // Bachelor, Master, Doctor
        public string Education { get; set; }
        public decimal YearIncome { get; set; }
    }
}
