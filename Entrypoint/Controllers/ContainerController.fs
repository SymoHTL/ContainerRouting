namespace Entrypoint.Controllers

open System.Net.Http
open System.Net.Http.Json
open Contracts.Entities
open Microsoft.AspNetCore.Mvc

[<ApiController>]
[<Route("api/containers")>]
type ContainerController (client : HttpClient) =
    inherit ControllerBase()


    [<HttpPut>]
    member _.Put(container : Container) =
        client.PutAsync("http://localhost:5084/api/containers", JsonContent.Create(container)) |> ignore
        0
        
        

