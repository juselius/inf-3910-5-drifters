module Index

open Elmish
open Fable.Core
open Fable.React.Props
open Feliz
open Feliz.Bulma
open Thoth.Fetch
open Thoth.Json
open Leaflet
open Fable.Core.JsInterop
open Shared

module RL = ReactLeaflet

Leaflet.icon?Default?imagePath <- "//cdnjs.cloudflare.com/ajax/libs/leaflet/1.3.1/images/"

type Model = unit

type Msg =
    | Hello

let decodeUnit : Decoder<unit> = Decode.Auto.generateDecoder ()

// stub fetch example
let getGrid dispatch =
    promise {
        let! x = Fetch.fetchAs (url="/api/getGrid", decoder = decodeUnit)
        ()
    } |> Promise.start

let init () : Model * Cmd<Msg> =
    let model = ()
    let cmd = Cmd.none
    model, cmd

let update (msg: Msg) (model: Model) : Model * Cmd<Msg> =
    match msg with
    | Hello ->  model, Cmd.none

let wmtsSource layer =
    "http://opencache.statkart.no/gatekeeper/gk/gk.open_wmts?" +
        "&layer=" + layer +
        "&style=default" +
        "&tilematrixset=EPSG%3A3857" +
        "&Service=WMTS" +
        "&Request=GetTile" +
        "&Version=1.0.0" +
        "&Format=image%2Fpng" +
        "&TileMatrix=EPSG%3A3857:{z}" +
        "&TileCol={x}" +
        "&TileRow={y}"

let osm = "https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"

let tile =
    RL.tileLayer [
        RL.TileLayerProps.Url (wmtsSource "norgeskart_bakgrunn")
        // RL.TileLayerProps.Url osm
    ] []

let drawCircle (pos: float * float) =
    let p = U3.Case3 pos
    RL.circle [
        RL.CircleProps.Custom ("center", p)
        RL.CircleProps.Radius 10.
    ] []

let drawTriangle (a, b, c) =
    let p =
        U3.Case1 [|
            U3.Case3 a
            U3.Case3 b
            U3.Case3 c
        |]
    RL.polygon [
        RL.PolygonProps.Positions p
        RL.PolygonProps.Weight 1.
    ] []

let drawPolyline (track: (float * float) array) =
    let p = track |> Array.map U3.Case3 |> U3.Case1
    RL.polyline [
        RL.PolylineProps.Positions p
        RL.PolylineProps.Weight 1.0
    ] []

let map (model: Model) dispatch =
    let pos = U3.Case3 (68.1, 13.4)
    RL.map [
        RL.MapProps.Zoom 9.
        RL.MapProps.Style [
            Height 600
            MinWidth 400
        ]
        RL.MapProps.Center pos
    ] [
        tile
        drawCircle (68.05, 13.6)
    ]

let view (model: Model) (dispatch: Msg -> unit) =
    Bulma.hero [
        hero.isFullHeight
        color.isPrimary
        prop.style [
            style.backgroundSize "cover"
            style.backgroundImageUrl "https://unsplash.it/1200/900?random"
            style.backgroundPosition "no-repeat center center fixed"
        ]
        prop.children [
            Bulma.heroBody [ Bulma.container [
                Bulma.heroHead []
                Bulma.columns [
                Bulma.column [
                    column.is8
                    column.isOffset2
                    prop.children [
                        Bulma.title [
                            text.hasTextCentered
                            prop.text "Hello world"
                        ]
                        Bulma.field.div [
                            prop.children [
                                map model dispatch
                            ]
                        ]
                    ]
                ] ]
            ] ]
        ]
    ]
