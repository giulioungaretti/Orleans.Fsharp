namespace Tests.ClusterFixture

open System
open Orleans.TestingHost
open Orleans.Hosting
open Orleans

open FSharp.NetCore.Interfaces
open FSharp.NetCore.Grains
open Grains

type TestSiloConf ()=
    interface ISiloBuilderConfigurator with
        member _.Configure(builder:ISiloHostBuilder) =
                builder
                    .AddMemoryGrainStorageAsDefault()
                    .UseLocalhostClustering()
                    .ConfigureApplicationParts(fun parts ->
                      parts.AddApplicationPart(typeof<HelloGrainInDifferntFile>.Assembly)
                              .AddApplicationPart(typeof<HelloGrainInSameFile>.Assembly)
                              .AddApplicationPart(typeof<IHello>.Assembly)
                              .AddApplicationPart(typeof<IWillFail>.Assembly)
                              .AddApplicationPart(typeof<IWillWork>.Assembly)
                              .AddFromAppDomain().WithCodeGeneration() |> ignore )
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
