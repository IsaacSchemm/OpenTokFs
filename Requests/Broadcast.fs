namespace ISchemm.OpenTokFs.Requests

open ISchemm.OpenTokFs
open System.Net
open System
open System.IO
open JWT.Algorithms
open JWT.Serializers
open JWT

module Broadcast =
    type Body = {
        iss: string
        ist: string
        iat: int64
        exp: int64
        jti: string
    }

    /// <summary>
    /// Use this method to get details on broadcasts that are in progress and started. Completed broadcasts are not included in the listing.
    /// </summary>
    let AsyncList (credentials: IOpenTokCredentials) = async {
        let uri = sprintf "https://api.opentok.com/v2/project/%d/broadcast" credentials.ApiKey
        let req = WebRequest.CreateHttp uri

        req.Headers.Add("X-OPENTOK-AUTH", OpenTokAuthentication.CreateToken credentials)

        use! resp = req.AsyncGetResponse()
        use s = resp.GetResponseStream()
        use sr = new StreamReader(s)

        return! sr.ReadToEndAsync() |> Async.AwaitTask
    }

    /// <summary>
    /// Use this method to get details on broadcasts that are in progress and started. Completed broadcasts are not included in the listing.
    /// </summary>
    let ListAsync (credentials: IOpenTokCredentials) =
        AsyncList credentials
        |> Async.StartAsTask