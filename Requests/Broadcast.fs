namespace ISchemm.OpenTokFs.Requests

open System
open System.IO
open System.Runtime.InteropServices
open Newtonsoft.Json
open ISchemm.OpenTokFs
open ISchemm.OpenTokFs.Types
open FSharp.Control

module Broadcast =
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
    /// Get details on broadcasts that are currently in progress, making as many requests to the server as necessary.
    /// </summary>
    let AsyncListAll credentials sessionId = asyncSeq {
        let paging = new OpenTokPagingParameters()
        let mutable finished = false
        while not finished do
            let! list = AsyncList credentials paging sessionId
            if Seq.isEmpty list.items then
                finished <- true
            else
                for item in list.items do
                    yield item
                paging.Offset <- paging.Offset + Seq.length list.items
    }

    /// <summary>
    /// Get details on broadcasts that are currently in progress. Completed broadcasts are not included.
    /// </summary>
    let ListAsync credentials paging ([<Optional;DefaultParameterValue(null)>] sessionId: string) =
        sessionId
        |> Option.ofObj
        |> AsyncList credentials paging
        |> Async.StartAsTask

    /// <summary>
    /// Get details on broadcasts that are currently in progress, making as many requests to the server as necessary.
    /// </summary>
    let ListAllAsync credentials max ([<Optional;DefaultParameterValue(null)>] sessionId: string) =
        sessionId
        |> Option.ofObj
        |> AsyncListAll credentials
        |> AsyncSeq.take max
        |> AsyncSeq.toArrayAsync
        |> Async.StartAsTask