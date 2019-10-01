namespace FSharp.NetCore

module Interfaces =

    open System.Threading.Tasks

    type IHello =
        inherit Orleans.IGrainWithIntegerKey
        abstract member SayHello : string -> Task<string>

    type IOverrideHelloSameProject=
        inherit IHello

    type IOverrideHelloDifferentProject=
        inherit IHello

    type IConcreteHelloSameProject=
        inherit IHello

    type IConcreteHelloDifferentProject=
        inherit IHello
