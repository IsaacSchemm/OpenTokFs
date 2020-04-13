namespace OpenTokFs.RequestOptions

open System
open System.Runtime.InteropServices

/// <summary>
/// An object that provides information about an RTMP destination for an OpenTok broadcast.
/// </summary>
type RtmpDestination(serverUrl: string, streamName: string, [<Optional;DefaultParameterValue(null: string)>] id: string) =
    member __.Id = id
    member __.ServerUrl = serverUrl
    member __.StreamName = streamName

    member internal this.AsSerializableObject() =
        seq {
            let o x = x :> obj
            if not (String.IsNullOrEmpty this.Id) then
                yield ("id", o this.Id)
            yield ("serverUrl", o this.ServerUrl)
            yield ("streamName", o this.StreamName)
        }
        |> dict