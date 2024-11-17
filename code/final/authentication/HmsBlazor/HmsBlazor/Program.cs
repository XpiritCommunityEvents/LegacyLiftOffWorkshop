using HmsBlazor.Components;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddMicrosoftIdentityConsentHandler()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddScoped<Project_HMS.DataAccess>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var entraConfig = builder.Configuration.GetSection("EntraID");

builder.Services
    .AddAuthentication(options => {
        options.DefaultAuthenticateScheme = OpenIdConnectDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
    })
    .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("EntraID"))
        .EnableTokenAcquisitionToCallDownstreamApi([ entraConfig.GetValue<string>("Scopes")! ])
        .AddInMemoryTokenCaches();


builder.Services.AddAuthentication()
    .AddJwtBearer("Bearer", options =>
    {
        options.Authority = entraConfig.GetValue<string>("Authority");
        options.Audience = entraConfig.GetValue<string>("ClientId");
    });

builder.Services.AddControllersWithViews()
                .AddMicrosoftIdentityUI();
                
builder.Services.AddAuthorization(options =>
{
     options.DefaultPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .AddAuthenticationSchemes(CookieAuthenticationDefaults.AuthenticationScheme, OpenIdConnectDefaults.AuthenticationScheme, JwtBearerDefaults.AuthenticationScheme)
        .Build();
    options.FallbackPolicy = options.DefaultPolicy;
});

var baseAddress = builder.Configuration.GetValue<string>("BaseUrl");

// enables HttpClientFactory.CreateClient()
builder.Services.AddHttpClient("HmsApi", 
    client => client.BaseAddress = new Uri(baseAddress))
     .AddMicrosoftIdentityAppAuthenticationHandler("HmsApi", entraConfig);

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
