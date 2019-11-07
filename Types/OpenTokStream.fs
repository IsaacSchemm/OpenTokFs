namespace OpenTokFs.Types

[<AllowNullLiteral>]
type OpenTokStream() =
    member val Id: string = "" with get, set
    member val VideoType: string = "" with get, set
    member val Name: string = "" with get, set
    member val LayoutClassList: string[] = Array.empty with get, set