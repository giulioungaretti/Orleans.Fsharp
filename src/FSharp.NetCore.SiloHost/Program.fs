open Microsoft.Extensions.Logging
open Orleans
open Orleans.Hosting
open System
open FSharp.Control.Tasks


open FSharp.NetCore.Interfaces
open FSharp.NetCore.Grains
open Grains

let buildSiloHost () =
      let builder = new SiloHostBuilder()
      builder
        .UseLocalhostClustering()
        .ConfigureApplicationParts(fun parts ->
          parts.AddApplicationPart(typeof<HelloGrainInDifferntFile>.Assembly)
                  .AddApplicationPart(typeof<HelloGrainInSameFile>.Assembly)
                  .AddApplicationPart(typeof<IHello>.Assembly)
                  .AddApplicationPart(typeof<IWillFail>.Assembly)
                  .AddApplicationPart(typeof<IWillWork>.Assembly)
                  .AddFromAppDomain().WithCodeGeneration() |> ignore )
        .ConfigureLogging(fun logging -> logging.AddConsole() |> ignore)
        .Build()

[<EntryPoint>]
let main _ =
    let t = task {
        let host = buildSiloHost ()
        do! host.StartAsync ()

        printfn "Press any keys to terminate..."
        Console.Read() |> ignore

        do! host.StopAsync()

        printfn "SiloHost is stopped"
    }

    t.Wait()

    0
