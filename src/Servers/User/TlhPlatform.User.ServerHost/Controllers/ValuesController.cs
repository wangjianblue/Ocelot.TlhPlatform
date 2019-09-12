using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Consul;
using DotNetCore.CAP;
using EasyNetQ;
using Exceptionless;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TlhPlatform.Infrastructure.EasyNetQ;
using TlhPlatform.Infrastructure.Extents;
using Channel = System.Threading.Channels.Channel;

namespace TlhPlatform.User.ServerHost
{


    [Route("api/[controller]")] 
    public class ValuesController : ControllerBase
    {
        private static int _count = 0;
        private readonly IBus _bus;
       

        public ValuesController(IBus bus)
        {
            this._bus = bus;
        }
        // GET api/values
        [HttpGet] 
   
        public IEnumerable<string> Get()
        {
             
            _count++;
            Console.WriteLine($"Get...{_count}");
            if (_count <= 3)
            {
                System.Threading.Thread.Sleep(5000);
            }

            return new string[] { $"ClinetService: {DateTime.Now.ToString()} {Environment.MachineName} " +
                                  $"OS: {Environment.OSVersion.VersionString}" };
        }
        [NonAction]
        public virtual async Task<string> GetAllProductsFallBackAsync(string productType)
        {
            Console.WriteLine($"-->>FallBack : Starting get product type : {productType}");

            return $"OK for FallBack  {productType}";
        }
        // GET api/values/5
        [HttpGet("{id}")]
        [HystrixCommand(nameof(GetAllProductsFallBackAsync),
            IsEnableCircuitBreaker = true,
            ExceptionsAllowedBeforeBreaking = 3,
            MillisecondsOfBreak = 1000 * 5)]
        public ActionResult<string> Get(int id)
        {
            try
            {
                var message = new ClientMessage
                {
                    ClientId = id,
                    ClientName = "user_service",
                    Sex = "男",
                    Age = 29,
                    SmokerCode = "N",
                    Education = "Master",
                    YearIncome = 100000
                };
                _bus.Publish(message);

                Console.WriteLine($"-->>Starting get product type : {id}");
                string str = null;

                str.ToString();
                throw new Exception();
            }
            catch (Exception e)
            {
               e.ToExceptionless().Submit();
            } 
            // to do : using HttpClient to call outer service to get product list

            return $"OK {id}";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
