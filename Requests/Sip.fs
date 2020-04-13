namespace OpenTokFs.Requests

open System.Net
open OpenTokFs
open OpenTokFs.Json.RequestTypes
open OpenTokFs.Json.ResponseTypes

module Sip =
    let AsyncDial (credentials: IOpenTokCredentials) (dial: OpenTokDialRequest) = async {
        let req =
            credentials.ApiKey
            |> sprintf "https://api.opentok.com/v2/project/%d/dial"
            |> WebRequest.CreateHttp
        req.Headers.Add("X-OPENTOK-AUTH", OpenTokAuthentication.CreateToken credentials)
        req.Accept <- "application/json"
        req.Method <- "POST"

        do! OpenTokAuthentication.AsyncWriteJson req dial

        return! OpenTokAuthentication.AsyncReadJson<OpenTokSipConnection> req
    }

    let DialAsync credentials req =
        AsyncDial credentials req
        |> Async.StartAsTask