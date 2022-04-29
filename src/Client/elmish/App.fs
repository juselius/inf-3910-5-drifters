module App

open Elmish
open Elmish.React
open Fable.Core.JsInterop

importAll "./public/style.scss"
importAll "../../node_modules/leaflet/dist/leaflet.css"

#if DEBUG
open Elmish.Debug
open Elmish.HMR
#endif

Program.mkProgram Index.init Index.update Index.view
#if DEBUG
|> Program.withConsoleTrace
#endif
|> Program.withReactSynchronous "feliz-app"
#if DEBUG
|> Program.withDebugger
#endif
|> Program.run
