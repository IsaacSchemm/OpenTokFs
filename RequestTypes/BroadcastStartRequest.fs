﻿namespace OpenTokFs.RequestTypes

open System

/// <summary>
/// Any object that provides parameters for starting an OpenTok broadcast.
/// </summary>
type IBroadcastStartRequest =
    abstract member SessionId: string
    abstract member LayoutType: string
    abstract member LayoutStylesheet: string
    abstract member Duration: TimeSpan
    abstract member Hls: bool
    abstract member Rtmp: seq<RtmpDestination>
    abstract member Resolution: string

/// <summary>
/// An object that provides parameters for starting an OpenTok broadcast, using reasonable defaults.
/// Note that neither HLS nor RTMP is enabled by default; you will have to turn one or both of them on.
/// </summary>
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
        member this.Rtmp = this.Rtmp
        member this.Resolution = this.Resolution