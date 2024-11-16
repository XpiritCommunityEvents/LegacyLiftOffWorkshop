using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddHttpClient("HmsApi", (sp, client) =>{
    var navigation = sp.GetRequiredService<NavigationManager>();
    client.BaseAddress = new Uri(navigation.BaseUri);
});

builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>()
    .CreateClient("HmsApi"));

await builder.Build().RunAsync();
