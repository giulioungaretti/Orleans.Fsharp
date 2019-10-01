module Grains

open System.Threading.Tasks

open Orleans
open FSharp.NetCore.Grains
open FSharp.NetCore.Interfaces

type HelloGrainConcreteDifferentProject() =
    inherit Grain ()
    interface IConcreteHelloDifferentProject with
        member this.SayHello (greeting : string) : Task<string> =
            greeting |> sprintf "You said: %s, I say: concrete differnt project!" |> Task.FromResult

type HelloGrainOverrideDifferntProject () =
    inherit AbstractHelloGrain ()
    interface IOverrideHelloDifferentProject
    override this.SayHello (greeting : string) : Task<string> =
        greeting |> sprintf "You said: %s, I say: override different project!" |> Task.FromResult
