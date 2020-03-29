using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 
using RabbitMQ.Client;
using TlhPlatform.Infrastructure.RabbitMQ;
using TlhPlatform.Infrastructure.Utilities;
using TlhPlatform.Product.ServerHost.Model;

namespace TlhPlatform.Product.ServerHost.Consumer
{
    partial class Rabbit
    {
        private static readonly string simple_Queue_Name = "test_simple_queue";
        /// <summary>
        /// 第一种模式 （简单队列） 一个生产者 一个消费者
        /// </summary>
        public static void Test1()
        {

            //创建连接
            IConnection connection = ConnectionUtils.GetConnection(new RabbitOptions());
            //创建通道
            var channel = connection.CreateModel();
            //创建声明一个队列 
            channel.QueueDeclare(simple_Queue_Name, false, false, false, null);
            var msg = new Message
            {
                MessageID = "1",

            };
            var sendBytes = Encoding.UTF8.GetBytes(ConnectionUtils.getJsonByObject(msg));
            channel.BasicPublish("", simple_Queue_Name, null, sendBytes);

            channel.Close();
            connection.Close();

        }
    }
}
