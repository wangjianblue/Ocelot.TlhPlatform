using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyNetQ;
using Manulife.DNC.MSAD.Contracts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Thrift.Protocol;
using Thrift.Transport;
using TlhPlatform.Infrastructure.EasyNetQ;

namespace TlhPlatform.Order.Application
{
    [Route("api/[controller]")]
    //[Authorize]
    public class ValuesController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public ActionResult<string> Get()
        {



            var item = new
            {
                Id = "Order",
                Content = $"Order的关联的商品明细",
            };
            return JsonConvert.SerializeObject(item);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            var item = new
            {
                Id = "Order",
                Content = $"Order的关联的商品明细Get{id}",
            };
            return JsonConvert.SerializeObject(item);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]  string value)
        {
            // RPC - use Thrift

            using (TTransport transport = new TSocket("localhost", 61601))
            {
                using (TProtocol protocol = new TBinaryProtocol(transport))
                {
                    using (var serviceClient = new PaymentService.Client(protocol))
                    {
                        transport.Open();
                        TrxnRecord record = new TrxnRecord
                        {
                            TrxnId = 1212,
                            TrxnName = "测试内容",
                            TrxnType = "Trxn 类型",
                            Remark = "备注",
                            TrxnAmount = 121
                        };
                        var result = serviceClient.Save(record);

                        var p = result == 0 ? "Trxn Success" : "Trxn Failed";
                    }
                }
            }







        }
        private long GenerateTrxnId()
        {
            return 10000001;
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
