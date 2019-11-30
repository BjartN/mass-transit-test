using System;
using System.Threading.Tasks;
using MassTransit;
using Shared;

namespace Consumer.ConsoleApp
{
    public class FooCommandConsumer : IConsumer<FooCommand>
    {
        public async Task Consume(ConsumeContext<FooCommand> context)
        {
            await Console.Out.WriteLineAsync($"Getting command: {context.Message.Message}");
        }
    }
}