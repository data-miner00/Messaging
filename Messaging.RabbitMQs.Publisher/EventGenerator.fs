namespace Messaging.RabbitMQs.Publisher

open System
open Bogus
open Messaging.Core

module EventGenerator =
    let private faker = Faker "en"

    let private generateCustomer () =
        let randomId = Guid.NewGuid().ToString()
        let randomFirstName = faker.Name.FirstName()
        let randomLastName = faker.Name.LastName()
        let randomName = sprintf "%s %s" randomFirstName randomLastName
        let randomEmail = faker.Internet.Email(randomFirstName, randomLastName).ToLowerInvariant()

        Customer(randomId, randomName, randomEmail)

    let public generateEvents (count: int) =
        [for i = 1 to count do generateCustomer()]
