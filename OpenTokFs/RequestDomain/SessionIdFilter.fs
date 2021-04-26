namespace OpenTokFs.RequestDomain

type SessionIdFilter =
| AnySessionId
| SingleSessionId of string
with
    member this.QueryString = [
        match this with
        | SingleSessionId id -> ("sessionId", id)
        | AnySessionId -> ()
    ]