namespace OpenTokFs.Types

/// A partial list of results from the OpenTok REST API.
[<AllowNullLiteral>]
type OpenTokList<'a>() =
    /// The total number of items in the entire list.
    member val Count: int = 0 with get, set
    /// A partial list of items, based on the count and offset parameters in the request.
    member val Items: 'a[] = Array.empty with get, set

    override this.ToString() = sprintf "Partial list (%d/%d items): %A" (Array.length this.Items) this.Count (List.ofArray this.Items)