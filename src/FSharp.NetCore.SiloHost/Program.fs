open Microsoft.Extensions.Logging
open Orleans
open Orleans.Hosting
open System
open FSharp.Control.Tasks


let buildSiloHost () =
      let builder = SiloHostBuilder()
      builder
        .UseLocalhostClustering()
        .ConfigureApplicationParts(fun parts ->
          parts.AddFromDependencyContext().WithCodeGeneration() |> ignore
        )
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
