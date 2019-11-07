namespace OpenTokFs.Types

/// <summary>
/// A partial list of results from the OpenTok REST API.
/// </summary>
[<AllowNullLiteral>]
type OpenTokList<'a>() =
    /// <summary>
    /// The total number of items in the entire list.
    /// </summary>
    member val Count: int = 0 with get, set
    /// <summary>
    /// A partial list of items, based on the count and offset parameters in the request.
    /// </summary>
    member val Items: 'a[] = Array.empty with get, set