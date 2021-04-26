namespace OpenTokFs

type PagingCount = ExplicitMaximum of int | DefaultPagingCount

type Paging = {
    offset: int
    count: PagingCount
} with
    member this.QueryString = [
        ("offset", string this.offset)
        match this.count with
        | ExplicitMaximum v -> ("count", string v)
        | DefaultPagingCount -> ()
    ]