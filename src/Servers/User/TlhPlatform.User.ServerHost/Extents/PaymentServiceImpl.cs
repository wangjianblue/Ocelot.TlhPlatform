using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Manulife.DNC.MSAD.Contracts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Thrift.Server;
using Thrift.Transport;


namespace TlhPlatform.User.ServerHost.Extents
{
    public class PaymentServiceImpl : PaymentService.Iface
    {
        public TrxnResult Save(TrxnRecord trxn)
        {
            
            Console.WriteLine("Log : TrxnName:{0}, TrxnAmount:{1}, Remark:{2}", trxn.TrxnName, trxn.TrxnAmount, trxn.Remark);
            return TrxnResult.SUCCESS;
        }
    }

    public static class ApplicationExtenssion
    {
        public static IApplicationBuilder UseThriftServer(this IApplicationBuilder appBuilder)
        {
            //上面的代码用的是TThreadPoolServer，网上的代码均采用TSimpleServer，通过反编译比较TSimpleServer、TThreadedServer、TThreadPoolServer，发现TSimpleServer只能同时响应一个客户端，TThreadedServer则维护了一个clientQueue，clientQueue最大值是100，TThreadPoolServer则用的是用线程池响应多个客户请求，生产环境绝不能用TSimpleServer。
            var orderService = new PaymentServiceImpl();
            PaymentService.Processor processor = new PaymentService.Processor(orderService);
            TServerTransport transport = new TServerSocket(8800);
            TServer server = new TThreadPoolServer(processor, transport);

            var services = appBuilder.ApplicationServices.CreateScope().ServiceProvider;

            var lifeTime = services.GetService<IApplicationLifetime>();
            lifeTime.ApplicationStarted.Register(() =>
            {
                server.Serve();
            });
            lifeTime.ApplicationStopped.Register(() =>
            {
                server.Stop();
                transport.Close();
            });

            return appBuilder;
        }
    }
}
