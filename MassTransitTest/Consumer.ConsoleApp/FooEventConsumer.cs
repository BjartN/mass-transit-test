using System;
using System.Threading.Tasks;
using MassTransit;
using Shared;

namespace Consumer.ConsoleApp
{
    public class FooEventConsumer :  IConsumer<FooEvent>
    {
        public async Task Consume(ConsumeContext<FooEvent> context)
        {
            await Console.Out.WriteLineAsync($"Getting event: {context.Message.Message}");
        }
    }
}