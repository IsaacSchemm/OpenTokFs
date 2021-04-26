namespace OpenTokFs.RequestDomain

open System

type ListLimit = StopAtItemCount of int | StopAtCreationDate of DateTimeOffset | NoListLimit

type ListParameters = {
    first_page: PageBoundaries
    limit: ListLimit
}