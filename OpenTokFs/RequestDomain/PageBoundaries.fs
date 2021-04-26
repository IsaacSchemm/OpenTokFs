namespace OpenTokFs.RequestDomain

type PageBoundaries = {
    offset: int
    count: int
} with
    member this.QueryString = [
        ("offset", string this.offset)
        ("count", string this.count)
    ]