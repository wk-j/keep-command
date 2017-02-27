module KeepCommand.Command

open System
open System.IO
open System.Linq

open KeepCommand.TomlParser
open KeepCommand.Formatter

let findKeepingCommand() = 
    let keeps = 
        Directory.EnumerateFiles("./", "keep.toml", SearchOption.AllDirectories) 
        |> Seq.toList
        |> List.map (File.ReadAllText >> parse)
    keeps |> List.collect(fun x -> [for k in x.Keys do yield (k, sprintf "%A" (snd <| x.TryGetValue(k))) ])


let startKeep() =
    let command = findKeepingCommand()

    let rs = readInput "Select command" command

    for k, v in command do
        let value = v.Trim().TrimStart('\"').TrimEnd('\"')
        Console.WriteLine("{0} {1}", k, value) 