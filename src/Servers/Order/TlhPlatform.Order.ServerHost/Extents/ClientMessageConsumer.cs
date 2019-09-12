using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyNetQ.AutoSubscribe;
using TlhPlatform.Infrastructure.EasyNetQ;

namespace TlhPlatform.Order.ServerHost.Extents
{
    public class ClientMessageConsumer : IConsumeAsync<ClientMessage> 
    {
        [AutoSubscriberConsumer(SubscriptionId = "ClientMessageService.Order")]
        public Task ConsumeAsync(ClientMessage message)
        {
            Console.ForegroundColor = System.ConsoleColor.Red;
            Console.WriteLine("Consume one message from RabbitMQ : {0}, I will send one email to client.", message.ClientName);
            Console.ResetColor();

            return Task.CompletedTask;
        }
    }
}
