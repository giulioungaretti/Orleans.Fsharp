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

    type HelloGrainInSameFile () =
        inherit AbstractHelloGrain ()
        interface IWillWork
        override this.SayHello (greeting : string) : Task<string> =
            greeting |> sprintf "You said: %s, I say: Hello!" |> Task.FromResult
