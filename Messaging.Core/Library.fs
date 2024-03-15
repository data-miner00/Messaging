namespace Messaging.Core

type public Customer = class
    val Id: string
    val Name: string
    val Email: string

    new (id, name, email) as this =
        { Id = id; Name = name; Email = email }
        then
            printfn "Instantiated new customer with %s %s %s" this.Id this.Name this.Email
end
