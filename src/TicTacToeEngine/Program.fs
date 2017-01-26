// Learn more about F# at http://fsharp.org

open System
open Microsoft.FSharp.Reflection

type Player = PlayerX | PlayerO
type Row = Top | Middle | Bottom
type Column = Left | Center | Right
type Mark = Mark of Player | Empty
type Position = Position of Row * Column

type Game = { 
    grid: Map<Position, Mark>
    next: Player
}

type PlayAccepted = {
    position: Position 
    player: Player
}

type Error = AlreadyMarked

type Response = 
    Accepted of PlayAccepted | Problem of Error
let createGrid = [
    for r in FSharpType.GetUnionCases typeof<Row> do
    for c in FSharpType.GetUnionCases typeof<Column> do
    let row = FSharpValue.MakeUnion(r, [| |]) :?> Row
    let column = FSharpValue.MakeUnion(c, [| |]) :?> Column
    yield Position (row, column), Empty
]

let togglePlayer player = 
    match player with
        | PlayerX -> PlayerO
        | PlayerO -> PlayerX
let acceptPlay game event =
    let mark = Mark event.player
    { game with grid = Map.add event.position mark game.grid; next = togglePlayer event.player }

let markGrid game position player =
    match Map.find position game.grid with
     | Mark _ -> Problem AlreadyMarked
     | Empty -> Accepted { position = position; player = player }

let makePlay game position player =
    let response = markGrid game position player
    match response with
     | Accepted accepted -> acceptPlay game accepted
     | Problem _ -> game

let grid = createGrid |> Map.ofSeq
let start = { grid = grid; next = PlayerX }
let playX = makePlay start (Position (Top, Left)) PlayerX
let playO = makePlay playX (Position (Top, Left)) PlayerO

[<EntryPoint>]
let main argv = 
    printfn "Hello World!"
    printfn "%A" start
    printfn "%A" playX
    printfn "%A" playO
    0 // return an integer exit code

