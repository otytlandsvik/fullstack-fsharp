module App

open Feliz
open FS.FluentUI

[<ReactComponent>]
let ToastTest () =
    let unmounted, setUnmounted = React.useState true
    let toastId = Fui.useId (Some "toast", None)
    let toasterId = Fui.useId (Some "toaster", None)

    let toastController = Fui.useToastController (Some toasterId)

    let notify =
        fun _ ->
            setUnmounted false
            toastController.dispatchToast (
                Fui.toast [
                    toast.appearance.inverted
                    toast.children [
                        Fui.toastTitle [
                            toastTitle.action (Fui.link [link.text "Undo"])
                            toastTitle.text "Email sent"
                        ]
                        Fui.toastBody [
                            toastBody.subtitle "This is a subtitle"
                            toastBody.text "This toast never closes"
                        ]
                        Fui.toastFooter [
                            Fui.link [link.text "Action1"]
                            Fui.link [link.text "Action2"]
                        ]
                    ]
                ],
                [
                    dispatchToastOptions.timeout -1
                    dispatchToastOptions.toastId toastId
                    dispatchToastOptions.intent.error
                ]
            )

    let update =
        fun _ ->
            toastController.updateToast [
                updateToastOptions.timeout 2000
                updateToastOptions.toastId toastId
                updateToastOptions.content (
                    Fui.toast [
                        Fui.toastTitle [
                            toastTitle.text "This toast will close soon"
                        ]
                    ]
                )
                updateToastOptions.intent.success
            ]

    Html.div [
        prop.children [
            Fui.toaster [
                toaster.toasterId toasterId
                toaster.offset [
                    toastOffset.horizontal 300
                    toastOffset.vertical 400
                ]
                toaster.shortcuts {focus = fun d -> d.ctrlKey && d.key = "m"}
            ]
            Fui.button [
                button.onClick (fun _ -> if unmounted then notify () else update ())
                button.text (if unmounted then "Open Toast" else "Update toast")
            ]
        ]
    ]

[<ReactComponent>]
let App () =
    Fui.fluentProvider [
        fluentProvider.theme.webDarkTheme
        fluentProvider.children [ToastTest ()]
    ]

open Browser.Dom

let root = ReactDOM.createRoot (document.getElementById "root")
root.render (React.strictMode [App ()])
