using System;
using System.Threading.Tasks;
using MassTransit;
using Shared;

namespace Consumer.ConsoleApp
{
    class Program
    {
        private static string uniqueId = DateTime.UtcNow.ToString("dd-mm-ss-fff");

        static void Main(string[] args)
        {
            if (args.Length > 0)
                uniqueId = args[0];

            Task t = MainAsync();
            t.Wait();

            Console.ReadKey();
        }

        public static async Task MainAsync()
        {
            var busControl = Configure();
            busControl.StartAsync();
            Console.ReadKey();
        }

        private static IBusControl Configure()
        {
            var queueName = $"Consumer_Queue_{uniqueId}";
            Console.WriteLine($"Consuming using {nameof(FooEventConsumer)} and  {nameof(FooCommandConsumer)} in queue {queueName}");

            var eventBus = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.AutoDelete = true;

                cfg.Host(new Uri(RabbitConfig.HostAddress), host =>
                {
                    host.Username(RabbitConfig.UserName);
                    host.Password(RabbitConfig.Password);
                });

                cfg.ReceiveEndpoint(queueName, e =>
                {
                    //To recieve events, you need to configure the recieve endpoint with a unique queue name for each consumer
                    e.Consumer<FooEventConsumer>();
                });

                cfg.ReceiveEndpoint(RabbitConfig.SharedQueueName, e =>
                {
                    //To recieve commands, you need to configure the recieve endpoint the same queue name as the command is published to
                    e.Consumer<FooCommandConsumer>();
                });
            });

            return eventBus;
        }
    }

}
