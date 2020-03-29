using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyNetQ;
using RabbitMQ.Client;
using TlhPlatform.Infrastructure.RabbitMQ;
using TlhPlatform.Infrastructure.Utilities;
using TlhPlatform.Product.ServerHost.Model;

namespace TlhPlatform.Product.ServerHost.Consumer.ps
{
    partial  class Rabbit
    {
        private static readonly string Exchange_Name = "test_exchange_queue";
        /// <summary>
        /// 第一种模式 （简单队列） 一个生产者 一个消费者
        /// </summary>
        public static void Test4()
        {

            //创建连接
            IConnection connection = ConnectionUtils.GetConnection(new RabbitOptions() { });

            //创建通道
            var channel = connection.CreateModel();
            //创建声明一个队列  direct,fanout,topic,header 但是 header模式在实际使用中较少，所以这里只介绍前三种模式
            //Fanout Exchange 转发消息是最快的。
            channel.ExchangeDeclare(Exchange_Name, "fanout");
            var msg = new Message
            {
                MessageID = "1", 
            };
            var sendBytes = Encoding.UTF8.GetBytes(ConnectionUtils.getJsonByObject(msg));
            channel.BasicPublish(Exchange_Name,"" , null, sendBytes);
            
            channel.Close();
            connection.Close();

        }
    }
}
