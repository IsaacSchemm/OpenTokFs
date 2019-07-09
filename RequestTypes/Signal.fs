namespace OpenTokFs.RequestTypes

/// <summary>
/// A signal to be sent to one or all connected participants.
/// </summary>
type Signal = {
    Type: string
    Data: string
}