// Learn more about F# at http://fsharp.org

open System
open Microsoft.FSharp.Reflection

type Player = PlayerX | PlayerO
type Row = Top | Middle | Bottom
type Column = Left | Center | Right
type Mark = Mark of Player | Empty
type Position = Position of Row * Column

let createGrid = [
    for r in FSharpType.GetUnionCases typeof<Row> do
    for c in FSharpType.GetUnionCases typeof<Column> do
    let row = FSharpValue.MakeUnion(r, [| |]) :?> Row
    let column = FSharpValue.MakeUnion(c, [| |]) :?> Column
    yield Position (row, column), Empty
] 

let markGrid grid (position:Position) player =
    match Map.find position grid with
     | Mark _ -> grid
     | Empty -> Map.add position player grid

let start = createGrid |> Map.ofSeq
let playX = markGrid start (Position (Top, Left)) (Mark PlayerX)
let playO = markGrid playX (Position (Top, Left)) (Mark PlayerO)

[<EntryPoint>]
let main argv = 
    printfn "Hello World!"
    printfn "%A" start
    printfn "%A" playX
    printfn "%A" playO
    0 // return an integer exit code

