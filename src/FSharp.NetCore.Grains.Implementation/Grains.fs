module Grains

open System.Threading.Tasks

open FSharp.NetCore.Grains
open FSharp.NetCore.Interfaces


type HelloGrainInDifferntFile () =
    inherit AbstractHelloGrain ()
    interface IWillFail
    override this.SayHello (greeting : string) : Task<string> =
        greeting |> sprintf "You said: %s, I say: Hello!" |> Task.FromResult