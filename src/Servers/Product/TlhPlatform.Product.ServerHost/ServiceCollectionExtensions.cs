using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNetCore.CAP;
using TlhPlatform.Infrastructure.RabbitMQ;

namespace TlhPlatform.Product.ServerHost
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds and configures the consistence services for the consistency.
        /// </summary>
        /// <param name="services">The services available in the application.</param>
        /// <param name="setupAction">An action to configure the <see cref="CapOptions" />.</param>
        /// <returns>An <see cref="CapBuilder" /> for application services.</returns>
        public static Receives AddReceives(this IServiceCollection services, Action<RabbitOptions> configure)
        {
            var options = new RabbitOptions();
            configure(options);
            return new Receives(options);
        }
    }
}
