﻿namespace ISchemm.OpenTokFs.Requests

open System
open System.IO
open ISchemm.OpenTokFs

module Broadcast =
    [<AllowNullLiteral>]
    type ListParameters() =
        member val Offset = 0 with get, set
        member val Count = Nullable<int>() with get, set
        member val SessionId: string = null with get, set

    /// <summary>
    /// Use this method to get details on broadcasts that are in progress and started. Completed broadcasts are not included in the listing.
    /// </summary>
    let AsyncList (credentials: IOpenTokCredentials) (parameters: ListParameters) = async {
        let query = seq {
            if not (isNull parameters) then
                yield parameters.Offset |> sprintf "offset=%d"
                if parameters.Count.HasValue then
                    yield parameters.Count.Value |> sprintf "count=%d"
                if not (String.IsNullOrEmpty parameters.SessionId) then
                    yield parameters.SessionId |> Uri.EscapeDataString |> sprintf "sessionId=%s"
        }

        let req = OpenTokAuthentication.BuildRequest credentials "broadcast" query
        use! resp = req.AsyncGetResponse()
        use s = resp.GetResponseStream()
        use sr = new StreamReader(s)

        return! sr.ReadToEndAsync() |> Async.AwaitTask
    }

    /// <summary>
    /// Use this method to get details on broadcasts that are in progress and started. Completed broadcasts are not included in the listing.
    /// </summary>
    let ListAsync (credentials: IOpenTokCredentials) (parameters: ListParameters) =
        AsyncList credentials parameters
        |> Async.StartAsTask