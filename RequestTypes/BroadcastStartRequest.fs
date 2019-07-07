namespace ISchemm.OpenTokFs.RequestTypes

open System

type IRtmpDestination =
    abstract member Id: string
    abstract member ServerUrl: string
    abstract member StreamName: string

type RtmpDestination(serverUrl: string, streamName: string) =
    member val Id: string = null with get, set
    interface IRtmpDestination with
        member this.Id = this.Id
        member __.ServerUrl = serverUrl
        member __.StreamName = streamName

type IBroadcastStartRequest =
    abstract member SessionId: string
    abstract member LayoutType: string
    abstract member LayoutStylesheet: string
    abstract member Duration: TimeSpan
    abstract member Hls: bool
    abstract member Rtmp: seq<IRtmpDestination>
    abstract member Resolution: string

type BroadcastStartRequest(sessionId: string) =
    member val LayoutType = "bestFit" with get, set
    member val LayoutStylesheet: string = null with get, set
    member val Duration = TimeSpan.FromHours 2.0 with get, set
    member val Hls = false with get, set
    member val Rtmp = Seq.empty<RtmpDestination> with get, set
    member val Resolution = "640x480" with get, set
    interface IBroadcastStartRequest with
        member __.SessionId = sessionId
        member this.LayoutType = this.LayoutType
        member this.LayoutStylesheet = this.LayoutStylesheet
        member this.Duration = this.Duration
        member this.Hls = this.Hls
        member this.Rtmp = seq { for x in this.Rtmp do yield x }
        member this.Resolution = this.Resolution