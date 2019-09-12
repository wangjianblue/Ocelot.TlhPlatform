using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AspectCore.DynamicProxy;

namespace TlhPlatform.Infrastructure.Extents
{
    public class CustomInterceptorAttribute : AbstractInterceptorAttribute
    {
        public override async Task Invoke(AspectContext context, AspectDelegate next)
        {
            try
            { 
                Console.WriteLine("Before service call");
                await next(context);
            }
            catch (Exception)
            {

                Console.WriteLine("Service threw an exception");
                throw;
            }
            finally
            {
                Console.WriteLine("After service call");
            }
        }
    }
}
