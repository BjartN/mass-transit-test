namespace Shared
{
    public class RabbitConfig
    {
        public static string UserName = "";
        public static string Password = "";

        public static string HostAddress = "rabbitmq://localhost";
        public static string SharedQueueName = "Shared_Queue";

        public static string FullQueueName()
        {
            return $"{RabbitConfig.HostAddress}/{RabbitConfig.SharedQueueName}";
        }
    }
}