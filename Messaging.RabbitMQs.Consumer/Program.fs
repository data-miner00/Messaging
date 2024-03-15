open RabbitMQ.Client
open RabbitMQ.Client.Events
open System.Text
open System
open System.Text.Json
open Messaging.Core

do printfn "Messaging Receiver starting..."

let connectionString = "amqp://guest:guest@localhost:5672"
let appName = "Message Receiver"

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
channel.BasicQos(0 |> uint32, 1 |> uint16, false)

let listener (args: BasicDeliverEventArgs) =
    let body = args.Body.ToArray()
    let message = Encoding.UTF8.GetString(body)
    let event = JsonSerializer.Deserialize<Customer>(message)

    channel.BasicAck(args.DeliveryTag, false)

    printfn "%s" event.Name
    printfn "Message received: %s" message

let consumer = new EventingBasicConsumer(channel)
consumer.Received.Add(listener)

let consumerTag = channel.BasicConsume(queueName, false, consumer)

Console.ReadLine() |> ignore

channel.BasicCancel(consumerTag)

connection.Dispose()
channel.Dispose()
