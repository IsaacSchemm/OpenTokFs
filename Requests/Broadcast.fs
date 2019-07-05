namespace ISchemm.OpenTokFs.Requests

open System
open System.IO
open Newtonsoft.Json
open ISchemm.OpenTokFs
open ISchemm.OpenTokFs.Types

module Broadcast =
    type ListParameters() =
        member val Offset = 0 with get, set
        member val Count = Nullable<int>() with get, set
        member val SessionId: string = null with get, set

    /// <summary>
    /// Use this method to get details on broadcasts that are in progress and started. Completed broadcasts are not included in the listing.
    /// </summary>
    let AsyncList (credentials: IOpenTokCredentials) (parameters: ListParameters) = async {
        let query = seq {
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
        let! json = sr.ReadToEndAsync() |> Async.AwaitTask

        return JsonConvert.DeserializeObject<OpenTokList<OpenTokBroadcast>> json
    }

    /// <summary>
    /// Use this method to get details on broadcasts that are in progress and started. Completed broadcasts are not included in the listing.
    /// </summary>
    let ListAsync credentials parameters =
        AsyncList credentials parameters
        |> Async.StartAsTask