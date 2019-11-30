using System;
using System.Threading.Tasks;
using MassTransit;
using Shared;

namespace Publisher.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Task t = MainAsync();
            t.Wait();

            Console.ReadKey();
        }

        public static async Task MainAsync()
        {
            int messageCounter = 1;

            var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.AutoDelete = true;
                cfg.Host(new Uri(RabbitConfig.HostAddress), host =>
                {
                    host.Username(RabbitConfig.UserName);
                    host.Password(RabbitConfig.Password);
                });
            });
            await busControl.StartAsync();

            try
            {
                do
                {
                    ConsoleKeyInfo value = await Task.Run(() =>
                    {
                        Console.WriteLine("Press key to issue message");
                        Console.WriteLine("1 to send");
                        Console.WriteLine("2 to publish");
                        Console.WriteLine("3 to quit");
                        Console.Write("> ");
                        return Console.ReadKey();
                    });

                    if (value.KeyChar == '1')
                    {
                        var sendEndPoint = await busControl.GetSendEndpoint(new Uri(RabbitConfig.FullQueueName()));

                        await sendEndPoint.Send<FooCommand>(new FooCommand
                        {
                            Message = $"Send of foo message # {messageCounter++}"
                        });
                    }
                    else if (value.KeyChar == '2')
                    {
                        await busControl.Publish(new FooEvent
                        {
                            Message = $"Publish of foo message # {messageCounter++}"
                        });
                    }
                    else
                    {
                        break;
                    }

                    Console.WriteLine();
                    Console.WriteLine();
                } while (true);
            }
            finally
            {
                await busControl.StopAsync();
            }
        }
    }
}
