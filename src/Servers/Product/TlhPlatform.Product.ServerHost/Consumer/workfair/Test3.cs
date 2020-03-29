using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TlhPlatform.Infrastructure.RabbitMQ;
using TlhPlatform.Infrastructure.Utilities;

namespace ConsoleApp1.ps
{
    public partial class Rabbit
    {
        private static readonly string workfair_Queue_Name = "test_workfair_queue";
        /// <summary>
        /// 第三种模式 （公平分发 能者多劳） 一个生产者多个消费者
        /// </summary>
        public static void Test3()
        {
            //创建连接
            IConnection connection = ConnectionUtils.GetConnection(new RabbitOptions());
            //创建通道
            var channel = connection.CreateModel();
            //创建声明一个队列 
            channel.QueueDeclare(workfair_Queue_Name, false, false, false, null);
            // 一次只处理一个消息 
            channel.BasicQos(0, 1, false);
            #region 组装数据
            for (int i = 0; i < 50; i++)
            {
                var msg = "hello_" + i;
                var sendBytes = Encoding.UTF8.GetBytes(ConnectionUtils.getJsonByObject(msg));
                channel.BasicPublish("", workfair_Queue_Name, null, sendBytes);
            }
            #endregion
            channel.Close();
            connection.Close();
        }

    }
}
