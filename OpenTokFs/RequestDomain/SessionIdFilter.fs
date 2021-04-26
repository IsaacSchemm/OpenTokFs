namespace OpenTokFs.RequestDomain

type SessionIdFilter =
| SingleSessionId of string
| AnySessionId
with
    member this.QueryString = [
        match this with
        | SingleSessionId id -> ("sessionId", id)
        | AnySessionId -> ()
    ]