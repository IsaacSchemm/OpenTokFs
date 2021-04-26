namespace OpenTokFs.RequestDomain

open System

type PageLimit = StopAtItemCount of int | StopAtCreationDate of DateTimeOffset | NoPageLimit

type PagingParameters = {
    first_page: PageBoundaries
    limit: PageLimit
}

//module internal Paging =
//    let GetCreationDate (o: obj) =
//        match o with
//        | :? OpenTokFs.ResponseTypes.OpenTokArchive as a -> a.GetCreationTime()
//        | :? OpenTokFs.ResponseTypes.OpenTokBroadcast as b -> b.GetCreationTime()
//        | _ -> raise ("StopAtCreationDate is not supported on lists of this type." |> NotSupportedException)

//    let AsyncToList limit initial_sequence =
//        match limit with
//        | StopAtItemCount max ->
//            initial_sequence
//            |> AsyncSeq.take max
//            |> AsyncSeq.toListAsync
//        | StopAtCreationDate datetime ->
//            initial_sequence
//            |> AsyncSeq.takeWhile (fun a -> GetCreationDate a > datetime)
//            |> AsyncSeq.toListAsync
//        | NoPageLimit ->
//            initial_sequence
//            |> AsyncSeq.toListAsync