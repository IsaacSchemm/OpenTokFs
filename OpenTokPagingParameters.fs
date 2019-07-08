namespace ISchemm.OpenTokFs

open System

/// <summary>
/// A set of parameters to control which section of a list is returned from a request.
/// </summary>
type OpenTokPagingParameters = {
    /// <summary>
    /// The start offset in the list.
    /// </summary>
    offset: int
    /// <summary>
    /// The number of items to retrieve, starting at the offset.
    /// </summary>
    count: Nullable<int>
}