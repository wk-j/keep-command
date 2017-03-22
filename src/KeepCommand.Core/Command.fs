module KeepCommand.Command

open System
open System.IO

open KeepCommand.TomlParser
open KeepCommand.Formatter
open KeepCommand.Executor

type Name = string
type Command = string

type Verb =
    | Add of Name * Command
    | Remove of Name

let findKeepingCommand() = 
    Directory.EnumerateFiles("./", ".keep.toml", SearchOption.AllDirectories) 
    |> Seq.toList
    |> List.map (File.ReadAllText >> parse)

let showCommands() = 
    let command = findKeepingCommand()
    let rs = readInput "Select command" command
    let ok, index = Int32.TryParse rs
    let command = 
        match ok with
        | true ->
            let value = (snd command.[index - 1]).TrimStart('\"').TrimEnd('\"')
            let tokens = value.Split(' ') |> Seq.toList
            match tokens with
            | fileName :: rest -> 
                Some (fileName, String.concat " " rest)
            | [] -> 
                None
        | false ->
            None

    match command with
    | Some (fileName, args) ->
        printfn "%s %s" fileName args
        executeCommand fileName args
    | None -> ()

let startKeep file argv =
    match argv with
    | "init" :: rest ->
        let info = FileInfo(file) 
        let dir = info.Directory
        if dir.Exists = false then
            dir.Create()
        if info.Exists = false then
            File.Create(info.FullName) |> ignore
        Info(sprintf "init %s" file) |> write
    | "add" :: rest -> 
        match rest with
        | name :: rest ->
            let title = sprintf "Enter command for '%s'" name
            let command = readInput title []
            addKey file name command 
        | x -> ()
    | "remove" :: rest -> 
        removeKey file (String.concat " " rest)
    | verb :: rest -> 
        let write x = x |> Description |> write 
        write "Usage" 
        write "1) keep" 
        write "2) keep init"
        write "3) keep add <key>" 
        write "4) keep remove <key>" 
    | [] -> showCommands()
