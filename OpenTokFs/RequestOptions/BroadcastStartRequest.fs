namespace OpenTokFs.RequestOptions

open System
open OpenTokFs.RequestTypes

/// <summary>
/// An object that provides parameters for starting an OpenTok broadcast, using reasonable defaults.
/// Note that neither HLS nor RTMP is enabled by default; you will have to turn one or both of them on.
/// </summary>
type BroadcastStartRequest(sessionId: string) =
    member __.SessionId = sessionId
    member val Layout: OpenTokVideoLayout = OpenTokVideoLayout.BestFit with get, set
    member val Duration: TimeSpan = TimeSpan.FromHours 2.0 with get, set
    member val Hls: bool = false with get, set
    member val Rtmp: OpenTokRtmpDestination seq = Seq.empty with get, set
    member val Resolution: string = "640x480" with get, set

    member internal body.AsSerializableObject() =
        let o x = x :> obj

        let outputs =
            seq {
                if body.Hls then
                    yield ("hls", new obj())
                yield ("rtmp", o body.Rtmp)
            }
            |> dict

        seq {
            yield ("sessionId", o body.SessionId)
            yield ("layout", o body.Layout)
                        
            let maxDuration = int body.Duration.TotalSeconds
            yield ("maxDuration", o maxDuration)
        
            yield ("outputs", o outputs)
            yield ("resolution", o body.Resolution)
        }
        |> dict