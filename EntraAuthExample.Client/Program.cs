using EntraAuthExample.Client;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp =>
{
    var authorizationMessageHandler = sp.GetRequiredService<AuthorizationMessageHandler>();
    authorizationMessageHandler.InnerHandler = new HttpClientHandler();
    authorizationMessageHandler = authorizationMessageHandler.ConfigureHandler(
        authorizedUrls: ["https://localhost:7187/"],
        scopes: ["api://6fdb9b8e-8b0b-46d0-b6c1-2bd7ddcbbd1a/all.read"]);
    return new HttpClient(authorizationMessageHandler)
    {
        BaseAddress = new Uri("https://localhost:7187/")
    };
});

builder.Services.AddMsalAuthentication(options =>
{
    builder.Configuration.Bind("AzureAd", options.ProviderOptions.Authentication);
    options.ProviderOptions.DefaultAccessTokenScopes.Add("api://6fdb9b8e-8b0b-46d0-b6c1-2bd7ddcbbd1a/all.read");
    options.ProviderOptions.LoginMode = "redirect";
});

await builder.Build().RunAsync();
