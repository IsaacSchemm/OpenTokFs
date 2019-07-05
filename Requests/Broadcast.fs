namespace ISchemm.OpenTokFs.Requests

open ISchemm.OpenTokFs
open System.Net
open System
open System.IO
open JWT.Algorithms
open JWT.Serializers
open JWT

type Tokenstuff = {
    iss: string
    ist: string
    iat: int64
    exp: int64
    jti: string
}

module Broadcast =
    let fromHex (s:string) = 
      s
      |> Seq.windowed 2
      |> Seq.mapi (fun i j -> (i,j))
      |> Seq.filter (fun (i,j) -> i % 2=0)
      |> Seq.map (fun (_,j) -> Byte.Parse(new System.String(j),System.Globalization.NumberStyles.AllowHexSpecifier))
      |> Array.ofSeq

    /// <summary>
    /// Use this method to get details on broadcasts that are in progress and started. Completed broadcasts are not included in the listing.
    /// </summary>
    let AsyncList (credentials: IOpenTokCredentials) = async {
        let uri = sprintf "https://api.opentok.com/v2/project/%d/broadcast" credentials.ApiKey
        let req = uri |> WebRequest.CreateHttp

        let payload = {
            iss = credentials.ApiKey.ToString()
            ist = "project"
            iat = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
            exp = DateTimeOffset.UtcNow.AddMinutes(3.0).ToUnixTimeSeconds()
            jti = Guid.NewGuid().ToString()
        }

        let algorithm = new HMACSHA256Algorithm()
        let serialzier = new JsonNetSerializer()
        let urlEncoder = new JwtBase64UrlEncoder()
        let encoder = new JwtEncoder(algorithm, serialzier, urlEncoder)
        req.Headers.Add("X-OPENTOK-AUTH", encoder.Encode(payload, credentials.ApiSecret))

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