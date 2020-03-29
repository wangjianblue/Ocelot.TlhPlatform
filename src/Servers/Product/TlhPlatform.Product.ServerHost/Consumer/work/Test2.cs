using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using TlhPlatform.Infrastructure.RabbitMQ;
using TlhPlatform.Infrastructure.Utilities;

namespace TlhPlatform.Product.ServerHost.Consumer
{
    public  partial class Rabbit
    {
        private static readonly string work_Queue_Name = "test_work_queue";
        /// <summary>
        /// 第二种模式 （Work queues工作队列） 一个生产者多个消费者
        /// </summary>
        public static void Test2()
        {
            //创建连接
            IConnection connection = ConnectionUtils.GetConnection(new RabbitOptions());
            //创建通道
            var channel = connection.CreateModel();
            //创建声明一个队列 
            channel.QueueDeclare(work_Queue_Name, false, false, false, null);
            #region 组装数据
            for (int i = 0; i < 50; i++)
            {
                var msg = "hello_" + i;
                var sendBytes = Encoding.UTF8.GetBytes(ConnectionUtils.getJsonByObject(msg));
                channel.BasicPublish("", work_Queue_Name, null, sendBytes);
            }
            #endregion
            channel.Close();
            connection.Close();
        }
    }
}
