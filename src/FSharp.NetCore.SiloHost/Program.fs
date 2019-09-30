open Microsoft.Extensions.Logging
open Orleans
open Orleans.Hosting
open System
open FSharp.Control.Tasks


open FSharp.NetCore.Interfaces
open FSharp.NetCore.Grains
open Grains

let assemblies = [|
                    typeof<HelloGrainInDifferntFile>.Assembly
                    typeof<HelloGrainInSameFile>.Assembly
                    typeof<IHello>.Assembly
                    typeof<IWillFail>.Assembly
                    typeof<IWillWork>.Assembly
                |]

let buildSiloHost () =
      let builder = new SiloHostBuilder()
      builder
        .UseLocalhostClustering()
        .ConfigureApplicationParts(fun parts ->
          assemblies
          |> Seq.iter(fun assembly -> parts.AddApplicationPart(assembly).WithCodeGeneration() |> ignore))
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
