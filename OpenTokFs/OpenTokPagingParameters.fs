namespace OpenTokFs

open System

/// A set of parameters to control which section of a list is returned from a request.
type OpenTokPagingParameters = {
    /// The start offset in the list.
    offset: int
    /// The number of items to retrieve, starting at the offset.
    count: int Nullable
}