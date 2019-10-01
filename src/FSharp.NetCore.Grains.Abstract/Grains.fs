namespace FSharp.NetCore

module Grains =

    open System.Threading.Tasks
    open Orleans
    open FSharp.NetCore.Interfaces

    [<AbstractClass>]
    type AbstractHelloGrain () =
        inherit Grain ()

        abstract member SayHello: string -> Task<string>

        interface IHello with
            member this.SayHello (greeting : string) : Task<string> =
                this.SayHello greeting

    type HelloGrainConcreteSameProject() =
        inherit Grain ()
        interface IConcreteHelloSameProject with
            member this.SayHello (greeting : string) : Task<string> =
                greeting |> sprintf "You said: %s, I say: concrete same project!" |> Task.FromResult

    type HelloGrainOverrideSameProject () =
        inherit AbstractHelloGrain ()
        interface IOverrideHelloSameProject
        override this.SayHello (greeting : string) : Task<string> =
            greeting |> sprintf "You said: %s, I say: override same proeject!" |> Task.FromResult
