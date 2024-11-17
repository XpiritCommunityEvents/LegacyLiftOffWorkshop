using HmsBlazor.Client.Pages;
using HmsBlazor.Components;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using Microsoft.AspNetCore.HttpOverrides;
using SmartComponents.Inference.OpenAI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddMicrosoftIdentityConsentHandler()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddSmartComponents()
       .WithInferenceBackend<OpenAIInferenceBackend>(); ;

builder.Services.AddScoped<Project_HMS.DataAccess>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("EntraID"));

builder.Services.AddControllersWithViews()
                .AddMicrosoftIdentityUI();
                
builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = options.DefaultPolicy;
});

var baseAddress = builder.Configuration.GetValue<string>("BaseUrl");

// enables HttpClientFactory.CreateClient()
builder.Services.AddHttpClient("HmsApi", 
    client => client.BaseAddress = new Uri(baseAddress));

// registers HttpClient so you can inject one in your components
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>()
    .CreateClient("HmsApi"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

// Add ForwardedHeaders middleware
var forwardedHeadersOptions = new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto | ForwardedHeaders.XForwardedHost
};
forwardedHeadersOptions.KnownNetworks.Clear(); // Loopback by default, this clears it
forwardedHeadersOptions.KnownProxies.Clear(); // Loopback by default, this clears it

app.UseForwardedHeaders(forwardedHeadersOptions);


app.UseAuthentication(); // <-- add this
app.UseAuthorization(); // <-- add this

app.MapControllers();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(HmsBlazor.Client._Imports).Assembly);

app.Run();
