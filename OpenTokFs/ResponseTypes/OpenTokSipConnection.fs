namespace OpenTokFs.ResponseTypes

open System

type OpenTokSipConnection = {
    id: Guid
    connectionId: Guid
    streamId: Guid
}