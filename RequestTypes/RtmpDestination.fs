namespace OpenTokFs.RequestTypes

open System.Runtime.InteropServices

type IRtmpDestination =
    abstract member Id: string
    abstract member ServerUrl: string
    abstract member StreamName: string

/// <summary>
/// An object that provides information about an RTMP destination for an OpenTok broadcast.
/// </summary>
type RtmpDestination(serverUrl: string, streamName: string, [<Optional;DefaultParameterValue(null: string)>] id: string) =
    member __.Id = id
    member __.ServerUrl = serverUrl
    member __.StreamName = streamName
    interface IRtmpDestination with
        member this.Id = this.Id
        member this.ServerUrl = this.ServerUrl
        member this.StreamName = this.StreamName