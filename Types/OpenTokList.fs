namespace ISchemm.OpenTokFs.Types

/// <summary>
/// A partial list of results from the OpenTok REST API.
/// </summary>
type OpenTokList<'a> = {
    /// <summary>
    /// The total number of items in the entire list.
    /// </summary>
    count: int
    /// <summary>
    /// A partial list of items, based on the count and offset parameters in the request.
    /// </summary>
    items: 'a[]
}