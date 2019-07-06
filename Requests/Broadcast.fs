﻿namespace ISchemm.OpenTokFs.Requests

open System
open System.IO
open System.Runtime.InteropServices
open Newtonsoft.Json
open ISchemm.OpenTokFs
open ISchemm.OpenTokFs.Types

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
    /// Get details on broadcasts that are currently in progress. Completed broadcasts are not included.
    /// </summary>
    let ListAsync credentials paging ([<Optional;DefaultParameterValue(null)>] sessionId: string) =
        sessionId
        |> Option.ofObj
        |> AsyncList credentials paging
        |> Async.StartAsTask