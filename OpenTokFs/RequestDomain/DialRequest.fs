namespace OpenTokFs.RequestDomain

open System.Collections.Generic

type SipCaller = SipCallerString of string | NoSipCaller

type SipHeaders = CustomSipHeaders of IEnumerable<KeyValuePair<string, string>> | NoSipHeaders

type SipAuth = UsernamePasswordSipAuth of string * string | NoSipAuth

type SipParameters = {
    uri: string
    from: SipCaller
    headers: SipHeaders
    auth: SipAuth
    secure: bool
} with
    member this.JsonObject = Map.ofList [
        ("uri", this.uri :> obj)
        match this.from with
        | SipCallerString str -> ("from", str :> obj)
        | NoSipCaller -> ()
        match this.headers with
        | CustomSipHeaders dict ->
            ("headers", dict :> obj)
        | NoSipHeaders -> ()
        match this.auth with
        | UsernamePasswordSipAuth (username, password) ->
            ("auth", Map.ofList [
                ("username", username)
                ("password", password)
            ] :> obj)
        | NoSipAuth -> ()
        ("secure", this.secure :> obj)
    ]

type DialRequest = {
    sessionId: string
    token: string
    sip: SipParameters
} with
    member this.JsonObject = Map.ofList [
        ("sessionId", this.sessionId :> obj)
        ("token", this.token :> obj)
        ("sip", this.sip.JsonObject :> obj)
    ]