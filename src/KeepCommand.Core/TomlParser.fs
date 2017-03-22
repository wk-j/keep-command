module KeepCommand.TomlParser
// based on https://github.com/mackwic/To.ml and https://github.com/seliopou/toml
open System

(*

open FParsec
type Token = KeyGroup of string list | KeyValue of string * obj

let (<||>) p1 p2 = attempt (p1 |>> box) <|> attempt (p2 |>> box)
let spc      = many (anyOf [' '; '\t']) 
let lexeme s = pstring s .>> spc
let lexemel s= pstring s .>> spaces
let comment  = pchar '#' .>>. restOfLine false 
let blanks   = skipMany ((comment <||> spc) .>> newline .>> spc) .>> spc
let brace p  = between (lexemel "[") (lexemel "]") p
let pbool    = (lexeme "true" >>% true) <|> (lexeme "false" >>% false)
let pstr     = between (lexeme "\"") (lexeme "\"") (manySatisfy ((<>)'"'))
let pdate' s = try preturn (DateTime.Parse (s, null, Globalization.DateTimeStyles.RoundtripKind)) with _ -> fail ""
let pdate    = between spc spc (anyString 20) >>= pdate'
let ary elem = brace (sepBy (elem .>> spaces) (lexemel ","))
let pary     = ary pbool <||> ary pdate <||> ary pint32 <||> ary pstr <||> ary pfloat
let value    = pbool <||> pdate <||> pstr <||> pfloat <||> pint32 <||> pary <||> ary pary
let kvKey    = many1Chars (noneOf " \t\n=")
let keyvalue = (kvKey .>> spc) .>>. (lexeme "=" >>. value) |>> KeyValue
let kgKey    = (many1Chars (noneOf " \t\n].")) .>> spc
let keygroup = blanks >>. brace (sepBy kgKey (lexeme ".")) |>> KeyGroup
let document = blanks >>. many (keygroup <|> keyvalue .>> blanks)

let parse text =
  let toml = Collections.Generic.Dictionary<string, obj>()
  let currentKg = ref []
  match run document text with
  | Success(tokens,_,_) ->
    for token in tokens do
      match token with
      | KeyGroup kg -> currentKg := kg
      | KeyValue (key,value) -> 
        let key = String.concat "." [ yield! !currentKg; yield key]
        toml.Add(key, value)
  | __ -> ()
  toml
*)
open System.IO

type Keeper = {
  Key: string
  Value: string
}

let parse (text:string) = 

  let findKeeper (line:string) = 
    let tokens = line.Split('=') |> Seq.map (fun x -> x.Trim()) |> Seq.toList
    match tokens with
    | [k;v] ->
      Some { Key = k; Value = v}
    | _ -> None

  let results = text.Split('\n') |> Seq.map findKeeper |> Seq.choose id
  (results)

let removeKey file key =
  match File.Exists file with 
  | true -> 
    let lines = file |> File.ReadAllLines |> Array.filter (fun x -> not <| x.StartsWith key)
    File.WriteAllLines(file, lines)
  | false -> ()

let addKey file key value =
  match File.Exists file with
  | true ->
    let lines = file |> File.ReadAllLines |> Array.filter (fun x -> not <| x.StartsWith key)
    let newLine = sprintf "%s = \"%s\"" key value
    File.WriteAllLines(file, Array.append lines [|newLine|] );
  | false -> ()