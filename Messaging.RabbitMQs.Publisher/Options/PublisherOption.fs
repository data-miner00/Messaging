namespace Messaging.RabbitMQs.Publisher.Options

type public PublisherOption =
    { EventsToBePublished: int;
      ConnectionString: string }