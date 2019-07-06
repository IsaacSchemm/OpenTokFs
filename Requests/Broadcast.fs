namespace ISchemm.OpenTokFs.Requests

open System
open System.IO
open System.Runtime.InteropServices
open Newtonsoft.Json
open ISchemm.OpenTokFs
open ISchemm.OpenTokFs.Types

module Broadcast =
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
        member val Rtmp = new ResizeArray<RtmpDestination>() with get, set
        member val Resolution = "640x480" with get, set
        interface IBroadcastStartRequest with
            member __.SessionId = sessionId
            member this.LayoutType = this.LayoutType
            member this.LayoutStylesheet = this.LayoutStylesheet
            member this.Duration = this.Duration
            member this.Hls = this.Hls
            member this.Rtmp = seq { for x in this.Rtmp do yield x }
            member this.Resolution = this.Resolution

    /// <summary>
    /// Get details on broadcasts that are currently in progress. Completed broadcasts are not included.
    /// </summary>
    let AsyncList (credentials: IOpenTokCredentials) (paging: OpenTokPagingParameters) (sessionId: string option) = async {
        let query = seq {
            yield paging.Offset |> sprintf "offset=%d"
            if paging.Count.HasValue then
                yield paging.Count.Value |> sprintf "count=%d"
            match sessionId with
            | Some s -> yield s |> Uri.EscapeDataString |> sprintf "sessionId=%s"
            | None -> ()
        }

        let req = OpenTokAuthentication.BuildRequest credentials "broadcast" query
        use! resp = req.AsyncGetResponse()

        use s = resp.GetResponseStream()
        use sr = new StreamReader(s)
        let! json = sr.ReadToEndAsync() |> Async.AwaitTask

        return JsonConvert.DeserializeObject<OpenTokList<OpenTokBroadcast>> json
    }

    /// <summary>
    /// Get details on broadcasts that are currently in progress. Completed broadcasts are not included.
    /// </summary>
    let ListAsync credentials paging ([<Optional;DefaultParameterValue(null)>] sessionId: string) =
        sessionId
        |> Option.ofObj
        |> AsyncList credentials paging
        |> Async.StartAsTask

    let AsyncStart (credentials: IOpenTokCredentials) (body: IBroadcastStartRequest) = async {
        match (body.LayoutType, String.IsNullOrEmpty body.LayoutStylesheet) with
        | ("custom", false) -> ()
        | ("custom", true) -> failwithf "The 'custom' layout type requires a stylesheet."
        | (_, false) -> failwithf "Stylesheets can only be used with the 'custom' layout type."
        | (_, true) -> ()

        let requestObject = {
            sessionId = body.SessionId
            layout = {
                ``type`` = body.LayoutType
                stylesheet = body.LayoutStylesheet
            }
            maxDuration = body.Duration.TotalSeconds |> int
            outputs = {
                hls = if body.Hls then (new obj()) else null
                rtmp = seq {
                    for r in body.Rtmp do
                        yield {
                            id = r.Id
                            serverUrl = r.ServerUrl
                            streamName = r.StreamName
                        }
                }
            }
            resolution = body.Resolution
        }

        let req = OpenTokAuthentication.BuildRequest credentials "broadcast" Seq.empty
        req.Method <- "POST"
        req.ContentType <- "application/json"

        do! async {
            use! rs = req.GetRequestStreamAsync() |> Async.AwaitTask
            use sw = new StreamWriter(rs)
            do! requestObject |> JsonConvert.SerializeObject |> sw.WriteLineAsync |> Async.AwaitTask
        }

        use! resp = req.AsyncGetResponse()

        use s = resp.GetResponseStream()
        use sr = new StreamReader(s)
        let! json = sr.ReadToEndAsync() |> Async.AwaitTask

        return json
    }

    let StartAsync credentials body =
        AsyncStart credentials body
        |> Async.StartAsTask

    let AsyncStop (credentials: IOpenTokCredentials) (broadcastId: string) = async {
        let path = broadcastId |> Uri.EscapeDataString |> sprintf "broadcast/%s/stop"
        let req = OpenTokAuthentication.BuildRequest credentials path Seq.empty
        req.Method <- "POST"

        use! resp = req.AsyncGetResponse()

        use s = resp.GetResponseStream()
        use sr = new StreamReader(s)
        let! json = sr.ReadToEndAsync() |> Async.AwaitTask

        return json
    }

    let StopAsync credentials broadcastId =
        AsyncStop credentials broadcastId
        |> Async.StartAsTask