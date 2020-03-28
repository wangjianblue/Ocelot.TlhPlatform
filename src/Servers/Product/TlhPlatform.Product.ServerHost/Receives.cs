using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RabbitMQ.Client.Events;
using TlhPlatform.Infrastructure.RabbitMQ;
using TlhPlatform.Infrastructure.Utilities;

namespace TlhPlatform.Product.ServerHost
{
    /// <summary>
    /// 消费端
    /// </summary>
    public class Receives
    {
        private static readonly string workfair_Queue_Name = "test_workfair_queue";
        private static IConnection connection;

        public Receives(RabbitOptions rabbitOptions)
        {
            connection = ConnectionUtils.GetConnection(rabbitOptions);
        }

        #region 订单消息处理

        /// <summary>
        ///  消息一
        /// </summary>
        public void Order_Receive1()
        {
            var channel = connection.CreateModel();
            channel.QueueDeclare(workfair_Queue_Name, true, false, false, null);
            channel.BasicQos(0, 1, false);
            EventingBasicConsumer consumer = new EventingBasicConsumer(channel);
            consumer.Received += (ch, ea) =>
            {
                var sendBytes = Encoding.UTF8.GetString(ea.Body);

                Console.WriteLine($"收到消息[Order_Receive1]： {sendBytes}");
                try
                {
                    Thread.Sleep(2000);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    channel.BasicAck(ea.DeliveryTag, false);
                }

            };
            channel.BasicConsume(workfair_Queue_Name, false, consumer);
            Console.WriteLine("[Order_Receive1]消费者已启动");
        }

        /// <summary>
        ///  消息二
        /// </summary>
        public void Order_Receive2()
        {

            var channel = connection.CreateModel();
            channel.QueueDeclare(workfair_Queue_Name, true, false, false, null);
            channel.BasicQos(0, 1, false);
            EventingBasicConsumer consumer = new EventingBasicConsumer(channel);
            consumer.Received += (ch, ea) =>
            {
                var sendBytes = Encoding.UTF8.GetString(ea.Body);

                Console.WriteLine($"收到消息[Order_Receive2]： {sendBytes}");
                try
                {
                    Thread.Sleep(1000);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    channel.BasicAck(ea.DeliveryTag, false);
                }
            };
            channel.BasicConsume(workfair_Queue_Name, false, consumer);

            Console.WriteLine("[Order_Receive2]消费者已启动");
        }

        #endregion
    }
}
