open System
open System.Threading.Tasks
open Microsoft.Extensions.Logging
open Orleans
open Orleans.Hosting
open FSharp.Control.Tasks

open FSharp.NetCore.Interfaces

let buildClient () =
      let builder = ClientBuilder()
      builder
        .UseLocalhostClustering()
        .ConfigureApplicationParts(fun parts ->
          parts.AddFromDependencyContext().WithCodeGeneration() |> ignore
          )
        .ConfigureLogging(fun logging -> logging.AddConsole() |> ignore)
        .Build()

let worker (client : IClusterClient) =
    task {
        let friend = client.GetGrain<IConcreteHelloDifferentProject> 0L
        let! response = friend.SayHello ("Good morning, my friend!")
        printfn "%s" response
    }

[<EntryPoint>]
let main _ =
    let t = task {
        use client = buildClient()
        do! client.Connect( fun (ex: Exception) -> task {
            do! Task.Delay(1000)
            return true
        })
        printfn "Client successfully connect to silo host"
        do! worker client
    }

    t.Wait()

    Console.ReadKey() |> ignore

    0
