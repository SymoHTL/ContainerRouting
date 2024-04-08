namespace Entrypoint
#nowarn "20"
open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting

module Program =
    let exitCode = 0

    [<EntryPoint>]
    let main args =

        let builder = WebApplication.CreateBuilder(args)

        builder.Services.AddControllers() |> ignore
        
        builder.Services.AddEndpointsApiExplorer() |> ignore
        
        builder.Services.AddHttpClient() |> ignore


        let app = builder.Build()

        app.UseHttpsRedirection() |> ignore
        
        app.UseAuthorization() |> ignore
        app.MapControllers() |> ignore

        app.Run()

        exitCode