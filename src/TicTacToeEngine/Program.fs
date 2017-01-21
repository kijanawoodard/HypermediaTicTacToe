// Learn more about F# at http://fsharp.org

open System
open Microsoft.FSharp.Reflection

type Player = PlayerX | PlayerO
type Row = Top | Middle | Bottom
type Column = Left | Center | Right
type Mark = Mark of Player | Empty
type Position = Position of Row * Column

type Game = { 
    Grid: Map<Position, Mark>
    Next: Player
}

type PlayAccepted = {
    Position: Position 
    Player: Player
}
let createGrid = [
    for r in FSharpType.GetUnionCases typeof<Row> do
    for c in FSharpType.GetUnionCases typeof<Column> do
    let row = FSharpValue.MakeUnion(r, [| |]) :?> Row
    let column = FSharpValue.MakeUnion(c, [| |]) :?> Column
    yield Position (row, column), Empty
]

let acceptPlay game event =
    let mark = Mark event.Player
    { game with Grid = Map.add event.Position mark game.Grid }

let markGrid grid (position:Position) player =
    let mark = Mark player
    match Map.find position grid with
     | Mark _ -> grid
     | Empty -> Map.add position mark grid

let start = createGrid |> Map.ofSeq
let playX = markGrid start (Position (Top, Left)) PlayerX
let playO = markGrid playX (Position (Top, Left)) PlayerO

[<EntryPoint>]
let main argv = 
    printfn "Hello World!"
    printfn "%A" start
    printfn "%A" playX
    printfn "%A" playO
    0 // return an integer exit code

