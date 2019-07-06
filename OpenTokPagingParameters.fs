namespace ISchemm.OpenTokFs

open System

type OpenTokPagingParameters() =
    member val Offset = 0 with get, set
    member val Count = Nullable<int>() with get, set