module KeepCommand.Formatter

open System

type private Konsole = System.Console

type Text =
      | Info of string
      | Prompt of string
      | Key of string
      | Description of string
      | Error of string

let write text =
    let setColor color = Konsole.ForegroundColor <- color
    let writeLine x = Konsole.WriteLine(x.ToString())
    let write x = Konsole.Write(x.ToString())
    match text with
    | Info text ->
        setColor ConsoleColor.Green
        writeLine text
    | Prompt text ->
        setColor ConsoleColor.Red
        write text
        setColor ConsoleColor.Yellow
    | Key text ->
        setColor ConsoleColor.Green
        write text
    | Description text ->
        setColor ConsoleColor.White
        writeLine text
    | Error text ->
        setColor ConsoleColor.Red
        writeLine text

let readInput (info:string) options   = 
      let f = sprintf
      Info(info) |> write
      let mutable index = 1
      for k, v in options do
            let key = f " %-2d %-25s" index k
            let desc = f "%-20s" v
            Key(key) |> write
            Description(desc) |> write
            index <- index + 1
      Prompt("> ") |> write
      Console.ReadLine()