module Tests.ClusterFixture

open System
open Orleans.TestingHost
open Orleans.Hosting
open Orleans


type TestSiloConf ()=
    interface ISiloBuilderConfigurator with
        member __.Configure(builder:ISiloHostBuilder) =
                builder
                    .AddMemoryGrainStorageAsDefault()
                    .UseLocalhostClustering()
                    .ConfigureApplicationParts(fun parts ->
                        parts.AddFromDependencyContext().WithCodeGeneration() |> ignore
                        )
                    |> ignore

type ClusterFixture()  =
    let builder = TestClusterBuilder()
    do builder.AddSiloBuilderConfigurator<TestSiloConf>()

    let cluster =  builder.Build()
    do cluster.Deploy()

    member this.Cluster = cluster

    interface IDisposable with
        member this.Dispose(): unit =
            this.Cluster.StopAllSilos()
