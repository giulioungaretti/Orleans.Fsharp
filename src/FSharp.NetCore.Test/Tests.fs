namespace Tests


module Facts =
    open System
    open FSharp.Control.Tasks

    open Xunit
    open Tests.ClusterFixture

    open FSharp.NetCore.Interfaces

    [<Literal>]
    let Name = "ClusterCollection"

    [<CollectionDefinition(Name)>]
    type ClusterCollection()=
      interface ICollectionFixture<ClusterFixture>

    [<Collection(Name)>]
    type Tests(fixture: ClusterFixture) =
      member this.cluster = fixture.Cluster

      [<Fact>]
      member this.``Hello`` () =
          let primaryKey = Guid.NewGuid();
          let g = this.cluster.GrainFactory.GetGrain<IWillWork> 0L
          task {
             let!  res =  g.SayHello("Hi")
             Assert.Equal("You said: Hi, I say: Hello!", res)
          }

      [<Fact>]
      member this.``also Hello`` () =
          let primaryKey = Guid.NewGuid();
          let g = this.cluster.GrainFactory.GetGrain<IWillFail> 0L
          task {
             let!  res =  g.SayHello("Hi")
             Assert.Equal("You said: Hi, I say: Hello!", res)
          }
