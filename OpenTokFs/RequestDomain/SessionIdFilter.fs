namespace OpenTokFs.RequestDomain

type SessionIdFilter =
| AnySessionId
| SingleSessionId of string