namespace OpenTokFs.RequestOptions

open OpenTokFs.RequestTypes

/// <summary>
/// An object that provides parameters for starting an OpenTok archive, using reasonable defaults.
/// </summary>
type ArchiveStartRequest(sessionId: string) =
    member __.SessionId = sessionId
    member val Layout: OpenTokVideoLayout = OpenTokVideoLayout.BestFit with get, set
    member val HasAudio: bool = true with get, set
    member val HasVideo: bool = true with get, set
    member val Name: string = null with get, set
    member val OutputMode: string = "composed" with get, set
    member val Resolution: string = "640x480" with get, set

    member body.AsSerializableObject() =
        seq {
            let o x = x :> obj

            yield ("sessionId", o body.SessionId)
            yield ("hasAudio", o body.HasAudio)
            yield ("hasVideo", o body.HasVideo)
            yield ("name", o body.Name)
            yield ("outputMode", o body.OutputMode)
            if body.OutputMode <> "individual" then
                yield ("layout", o body.Layout)
                yield ("resolution", o body.Resolution)
        }
        |> dict