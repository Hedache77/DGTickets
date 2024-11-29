using CurrieTechnologies.Razor.SweetAlert2;
using DGTickets.Frontend;
using DGTickets.Frontend.AuthenticationProviders;
using DGTickets.Frontend.Repositories;
using DGTickets.Frontend.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var urlProd = "https://dgticketsbackend.azurewebsites.net";
var urlDev = "https://localhost:7037";
builder.Services.AddSingleton(sp => new HttpClient { BaseAddress = new Uri(urlProd) });

builder.Services.AddScoped<IRepository, Repository>();
builder.Services.AddLocalization();
builder.Services.AddSweetAlert2();
builder.Services.AddMudServices();
builder.Services.AddAuthorizationCore();

builder.Services.AddScoped<AuthenticationProviderJWT>();
builder.Services.AddScoped<AuthenticationStateProvider, AuthenticationProviderJWT>(x => x.GetRequiredService<AuthenticationProviderJWT>());
builder.Services.AddScoped<ILoginService, AuthenticationProviderJWT>(x => x.GetRequiredService<AuthenticationProviderJWT>());

await builder.Build().RunAsync();