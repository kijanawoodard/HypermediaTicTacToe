// Learn more about F# at http://fsharp.org

open System
open Microsoft.FSharp.Reflection

type Player = PlayerX | PlayerO
type Row = Top | Middle | Bottom
type Column = Left | Center | Right
type Position = Row * Column
type Cell = { 
    Where: Position; 
    Mark: Player option 
}

type Grid = Cell list   

let createGrid = [
    for r in FSharpType.GetUnionCases typeof<Row> do
    for c in FSharpType.GetUnionCases typeof<Column> do
    let row = FSharpValue.MakeUnion(r, [| |]) :?> Row
    let column = FSharpValue.MakeUnion(c, [| |]) :?> Column
    yield { 
        Where = (row, column); 
        Mark = None
    }
]

let grid = createGrid
//    ( (Top, Left), None ),
  

[<EntryPoint>]
let main argv = 
    printfn "Hello World!"
    printfn "%A" grid
    0 // return an integer exit code

