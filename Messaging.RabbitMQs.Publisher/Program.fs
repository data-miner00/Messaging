open RabbitMQ.Client
open System.Text
open System
open Messaging.RabbitMQs.Publisher
open System.Text.Json
open Microsoft.Extensions.Configuration
open Messaging.RabbitMQs.Publisher.Options

do printfn "Messaging Publisher starting..."

let builder = ConfigurationBuilder()

let config = builder.AddJsonFile("settings.json").Build()
let option = config.GetSection(nameof(PublisherOption)).Get<PublisherOption>()

let events = EventGenerator.generateEvents option.EventsToBePublished
let serializedEvents = events |> List.map JsonSerializer.Serialize

let connectionString = option.ConnectionString
let appName = "Message Publisher"

let factory = new ConnectionFactory()
factory.Uri <- new Uri(connectionString)
factory.ClientProvidedName <- appName

let connection = factory.CreateConnection()
let channel = connection.CreateModel()

let exchangeName = "MessageExchange"
let routingKey = "sample-routing-key"
let queueName = "CustomerQueue"

channel.ExchangeDeclare(exchangeName, ExchangeType.Direct)
channel.QueueDeclare(queueName, false, false, false, null) |> ignore
channel.QueueBind(queueName, exchangeName, routingKey, null)

let messageBodyBytesList = serializedEvents |> List.map Encoding.UTF8.GetBytes

for messageBodyBytes in messageBodyBytesList do
    channel.BasicPublish(exchangeName, routingKey, null, messageBodyBytes)

connection.Dispose()
channel.Dispose()
