namespace OpenTokFs.Api

open OpenTokFs
open OpenTokFs.Credentials
open OpenTokFs.RequestTypes
open OpenTokFs.ResponseTypes

module Sip =
    let AsyncDial (credentials: IProjectCredentials) (dial: OpenTokDialRequest) = async {
        let req = OpenTokAuthentication.BuildProjectLevelRequest credentials "dial" Seq.empty
        req.Method <- "POST"

        do! OpenTokAuthentication.AsyncWriteJson req dial

        return! OpenTokAuthentication.AsyncReadJson<OpenTokSipConnection> req
    }

    let DialAsync credentials req =
        AsyncDial credentials req
        |> Async.StartAsTask