namespace OpenTokFs.Types

[<AllowNullLiteral>]
type OpenTokSession() =
    member val Session_id: string = "" with get, set
    member val Project_id: string = "" with get, set
    member val Create_dt: string = "Sat Jan 01 12:00:00 PST 2000" with get, set