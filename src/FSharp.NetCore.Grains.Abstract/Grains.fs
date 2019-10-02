namespace FSharp.NetCore

module Grains =

    open System.Threading.Tasks
    open Orleans
    open FSharp.NetCore.Interfaces
    open FSharp.Control.Tasks

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

    [<AbstractClass>]
    type Event<'T> () =
        inherit Grain ()

        abstract member Event: unit -> 'T

        interface IEvent with
            member this.Event<'s> () : Task<'s> =
                task {
                    let a = this.Event()
                    return box (a) :?> 's
                }
