using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Webapp;
using Microsoft.Extensions.DependencyInjection;
using Webapp.Services;
using Base.Service.Contracts;
using BlazorWebApp.Services;
using ITaxi.Service;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddSingleton<IAppState, AppState>();
builder.Services.AddTransient<ITaxiAuthTokenHandler>();
builder.Services.AddHttpClient("Auth", config => config.BaseAddress = new Uri("https://localhost:44308/api/v1.0/"));
builder.Services.AddHttpClient("API", config => config.BaseAddress = new Uri("https://localhost:44308/api/v1.0/"))
    .AddHttpMessageHandler<ITaxiAuthTokenHandler>();
//builder.Services.AddOidcAuthentication(options =>
//{
//    // Configure your authentication provider options here.
//    // For more information, see https://aka.ms/blazor-standalone-auth
//    builder.Configuration.Bind("Local", options.ProviderOptions);
//});

builder.Services.AddTransient<VehicleService>();

await builder.Build().RunAsync();
