module Settings

open System.IO
open Thoth.Json.Net
open Serilog

// Add more settings as needed
type Settings = {
    grid: string
    particles: string
    uv: string
}

let appsettings =
    let settings = System.IO.File.ReadAllText "appsettings.json"
    match Decode.Auto.fromString<Settings> settings with
    | Ok s -> s
    | Error e -> failwith e

