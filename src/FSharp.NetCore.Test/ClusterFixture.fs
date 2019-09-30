module Tests.ClusterFixture

open System
open Orleans.TestingHost
open Orleans.Hosting
open Orleans

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

type TestSiloConf ()=
    interface ISiloBuilderConfigurator with
        member _.Configure(builder:ISiloHostBuilder) =
                builder
                    .AddMemoryGrainStorageAsDefault()
                    .UseLocalhostClustering()
                    .ConfigureApplicationParts(fun parts ->
                        assemblies |> Seq.iter(fun assembly -> parts.AddApplicationPart(assembly).WithCodeGeneration().WithReferences() |> ignore))
                    |> ignore

type ClusterFixture()  =
    let builder = new TestClusterBuilder()
    do builder.AddSiloBuilderConfigurator<TestSiloConf>()

    let cluster =  builder.Build()
    do cluster.Deploy()

    member this.Cluster = cluster

    interface IDisposable with
        member this.Dispose(): unit =
            this.Cluster.StopAllSilos()
