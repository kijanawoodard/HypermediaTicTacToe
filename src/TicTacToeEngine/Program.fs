// Learn more about F# at http://fsharp.org

open System
open Microsoft.FSharp.Reflection

type Player = PlayerX | PlayerO
type Row = Top | Middle | Bottom
type Column = Left | Center | Right
type Position = Row * Column

let createCell position (player: Player option) =
    (position, player)

let createGrid = [
    for r in FSharpType.GetUnionCases typeof<Row> do
    for c in FSharpType.GetUnionCases typeof<Column> do
    let row = FSharpValue.MakeUnion(r, [| |]) :?> Row
    let column = FSharpValue.MakeUnion(c, [| |]) :?> Column
    yield createCell (row, column) None
] 

let grid = createGrid |> Map.ofSeq
let x = grid.Add((Top, Left), Some PlayerO)
let markGrid grid position player =
    match Map.find position grid with
     | Some marked -> grid
     | None -> Map.add position player grid

let playX = markGrid grid (Top, Left) (Some PlayerX)
let playO = markGrid playX (Top, Left) (Some PlayerO)

[<EntryPoint>]
let main argv = 
    printfn "Hello World!"
    printfn "%A" grid
    printfn "%A" playX
    printfn "%A" playO
    0 // return an integer exit code

