using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace TlhPlatform.Core.Data.Core
{
    public class VOCsOptionsSetup : IConfigureOptions<VOCsOptions>
    {
        private readonly IConfiguration _configuration;
             
        public VOCsOptionsSetup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public void Configure(VOCsOptions options)
        {
            //IConfigurationSection section = _configuration.GetSection("VOCs:Jwt");
            //JwtOptions jwt = section.Get<JwtOptions>();
            //if(jwt!=null)
            //{
            //    if(jwt.Secret==null)
            //    {
            //        jwt.Secret = _configuration["VOCs:Jwt:Secret"];
            //    }
            //    options.Jwt = jwt;
            //}
        }
    }
}
