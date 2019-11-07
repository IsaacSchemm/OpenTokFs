namespace OpenTokFs.Types

open System

/// An RTMP destination for an OpenTok broadcast.
[<AllowNullLiteral>]
type OpenTokRtmpStream() =
    /// An optional ID (if one was set when the broadcast was started).
    member val Id: string = null with get, set
    /// The RTMP server URL.
    member val ServerUrl: string = "" with get, set
    /// The RTMP stream name or stream key.
    member val StreamName: string = "" with get, set
    /// The status of the RTMP stream.
    member val Status: string = "" with get, set

/// Streaming endpoints for an OpenTok broadcast.
[<AllowNullLiteral>]
type OpenTokBroadcastStreams() =
    /// The HLS stream URL, if any.
    member val Hls: string = null with get, set
    /// An array of RTMP endpoints that are being broadcasted to.
    member val Rtmp: OpenTokRtmpStream[] = Array.empty with get, set

/// An OpenTok broadcast.
[<AllowNullLiteral>]
type OpenTokBroadcast() =
    /// The unique ID for the broadcast.
    member val Id: string = "" with get, set
    /// The OpenTok session ID.
    member val SessionId: string = "" with get, set
    /// The OpenTok API key.
    member val ProjectId: int = 0 with get, set
    /// The start time of the broadcast (in milliseconds since Jan 1 1970 00:00:00 UTC).
    member val CreatedAt: int64 = 0L with get, set
    /// Information about the HLS and RTMP endpoints of the broadcast (if any). Not available on all requests.
    member val BroadcastUrls: OpenTokBroadcastStreams = null with get, set
    /// The time the broadcast was stopped. Not available on all requests.
    member val UpdatedAt: int64 = 0L with get, set
    /// The status of the broadcast. Not available on all requests.
    member val Status: string = null with get, set
    /// The resolution of the broadcast.
    member val Resolution: string = "" with get, set

    /// Gets the creation time of the archive as a DateTimeOffset object.
    member this.GetCreationTime() = DateTimeOffset.FromUnixTimeMilliseconds this.CreatedAt
    /// Gets the time the broadcast was stopped as a DateTimeOffset object. Not available on all requests.
    member this.GetUpdatedTime() = DateTimeOffset.FromUnixTimeMilliseconds this.UpdatedAt