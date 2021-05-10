module Server

open System
open System.IO
open Microsoft.AspNetCore.Http
open Microsoft.Extensions.Logging
open Saturn
open Giraffe
open Serilog

open Shared
open Settings

let webApp =
    choose [
        GET >=> choose [
            route "/api/getPaticles" >=> warbler (fun _ -> json "")
            route "/api/getGrid" >=> warbler (fun _ -> json "")
            routef "/api/getFrame/%d" (fun _ -> json "")
        ]
        POST >=> choose [
            route "/api/initSimulation" >=> json ""
            route "/api/runSimulation" >=> json ""
            route "/api/stepSimulation" >=> json ""
        ]
    ]

let configureSerilog () =
    LoggerConfiguration()
        .WriteTo.Console()
        .CreateLogger()

let serilog (logger : ILoggingBuilder) =
    logger
        .SetMinimumLevel(LogLevel.Warning)
        .AddSerilog() |> ignore

let app =
    Log.Logger <- configureSerilog ()

    application {
        url "http://0.0.0.0:8085"
        use_router webApp
        memory_cache
        use_static "public"
        use_json_serializer(Thoth.Json.Giraffe.ThothSerializer())
        use_gzip
        logging serilog
    }

run app
