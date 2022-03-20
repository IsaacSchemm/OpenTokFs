namespace OpenTokFs.ResponseTypes

type OpenTokList<'T> = {
    count: int
    items: 'T list
}