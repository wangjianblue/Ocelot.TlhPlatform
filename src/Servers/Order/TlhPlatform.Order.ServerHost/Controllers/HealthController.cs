using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
 
namespace TlhPlatform.Order.Application
{
    [Produces("application/json")]
    [Route("api/Health")]
    public class HealthController : ControllerBase
    {
        [HttpGet]
        public ActionResult Get() => Ok("ok");

    }
}