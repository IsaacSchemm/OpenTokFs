namespace OpenTokFs.RequestTypes

open System.Runtime.InteropServices

/// <summary>
/// An object that provides information about an RTMP destination for an OpenTok broadcast.
/// </summary>
type RtmpDestination(serverUrl: string, streamName: string, [<Optional;DefaultParameterValue(null: string)>] id: string) =
    member __.Id = id
    member __.ServerUrl = serverUrl
    member __.StreamName = streamName