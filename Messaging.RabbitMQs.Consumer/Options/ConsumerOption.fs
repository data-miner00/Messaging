namespace Messaging.RabbitMQs.Consumer.Options

type public ConsumerOption =
    { ConnectionString: string;
      BatchConsumeCount: int }
