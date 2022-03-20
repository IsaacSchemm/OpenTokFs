namespace OpenTokFs.RequestTypes

module OpenTokLayoutClassChangeRequest =
    type ClassChange() =
        member val id = "" with get, set
        member val layoutClassList: seq<string> = Seq.empty with get, set

type OpenTokLayoutClassChangeRequest() =
    member val items: seq<OpenTokLayoutClassChangeRequest.ClassChange> = Seq.empty with get, set
