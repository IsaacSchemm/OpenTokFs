namespace OpenTokFs.OptionUtilities

open System

module Option =
    let Some x = Some x
    let None = None
    let IfNotNull x = Option.ofObj x
    let IfNotNullOrEmpty x = if String.IsNullOrEmpty x then None else Some x
    let IfNotNullOrWhitespace x = if String.IsNullOrWhiteSpace x then None else Some x