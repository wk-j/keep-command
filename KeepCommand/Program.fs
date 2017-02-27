// Learn more about F# at http://fsharp.net
// See the 'F# Tutorial' project for more help.

open KeepCommand.Command
open System.IO
open System

[<EntryPoint>]
let main argv = 
    let file = Path.Combine(".keep","commands.toml")
    startKeep file (argv |> Seq.toList)
    0 // return an integer exit code

