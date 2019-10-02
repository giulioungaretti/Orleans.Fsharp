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
          let g = this.cluster.GrainFactory.GetGrain<IConcreteHelloDifferentProject> 0L
          task {
             let!  res =  g.SayHello("Hi")
             Assert.Equal("You said: Hi, I say: concrete differnt project!", res)
          }
      [<Fact>]
      member this.``ciao`` () =
          let primaryKey = Guid.NewGuid();
          let g = this.cluster.GrainFactory.GetGrain<IConcreteHelloSameProject> 0L
          task {
             let!  res =  g.SayHello("Hi")
             Assert.Equal("You said: Hi, I say: concrete same project!", res)
          }

      [<Fact>]
      member this.``also Hello`` () =
          let primaryKey = Guid.NewGuid();
          let g = this.cluster.GrainFactory.GetGrain<IOverrideHelloSameProject> 0L
          task {
             let!  res =  g.SayHello("Hi")
             Assert.Equal("You said: Hi, I say: override same proeject!", res)
          }

      [<Fact>]
      member this.``also Hello different proejct`` () =
          let primaryKey = Guid.NewGuid();
          let g = this.cluster.GrainFactory.GetGrain<IOverrideHelloDifferentProject> 0L
          task {
             let!  res =  g.SayHello("Hi")
             Assert.Equal("You said: Hi, I say: override different project!", res)
          }

      [<Fact>]
      member this.``Do type param break stuff?`` () =
          let primaryKey = Guid.NewGuid();
          let g = this.cluster.GrainFactory.GetGrain<IActualEvent> 0L
          task {
             let!  res =  g.Event<string>()
             Assert.Equal("You said: Hi, I say: override different project!", res)
          }
