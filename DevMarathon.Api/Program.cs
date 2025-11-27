using DevMarathon.Api;
using DevMarathon.Api.Filters;
using DevMarathon.Application;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using DevMarathon.Api.SignalR;
using DevMarathon.Infrastructure;
using Microsoft.AspNetCore.Http.Connections;
using Scalar.AspNetCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

//In the Debug environment, for security reasons, set these configs in the Secret Storage.
//In the Release environment, for security reasons, set these configs in the Environment Variables.
//builder.Configuration.AddJsonFile("SampleConfigs.json", optional: true, reloadOnChange: true);

bool isDevelopment = builder.Environment.IsDevelopment();

var configs = Configuration.ConfigureConfigs(builder.Configuration, isDevelopment);
builder.Services.AddSingleton(configs);
builder.Configuration.AddUserSecrets<Program>();
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

builder.Services.AddControllers(o =>
{
    o.Filters.Add(typeof(HttpResponseExceptionFilter));
    o.Filters.Add(typeof(FillUserContextFilter));
    o.Filters.Add(typeof(LoggingFilter));
});
builder.Services.Configure<ApiBehaviorOptions>(options => { options.SuppressModelStateInvalidFilter = true; });
builder.Services.AddMvc(setupAction => { }).AddJsonOptions(jsonOptions =>
{
    jsonOptions.JsonSerializerOptions.PropertyNamingPolicy = null;
});

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration, configs);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(x =>
    {
        x.RequireHttpsMetadata = false;
        x.TokenValidationParameters = new TokenValidationParameters
        {
            RequireExpirationTime = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("token key")),
            ValidateAudience = false,
            // Ensure the token was issued by a trusted authorization server (default true):
            ValidateIssuer = false,
            ValidIssuer = "",
            ValidAudience = "",
        };
    })
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme,
        options => builder.Configuration.Bind("CookieSettings", options));

builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, _, _) =>
    {
        // تعریف Security Scheme برای Bearer
        document.Components ??= new OpenApiComponents();
        document.Components.SecuritySchemes ??= new Dictionary<string, OpenApiSecurityScheme>();

        document.Components.SecuritySchemes["BearerAuth"] = new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Name = "Authorization",
            Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\""
        };

        // برای اینکه این امنیت به‌صورت global (برای همه‌ی متدها) اعمال بشود:
        document.SecurityRequirements ??= new List<OpenApiSecurityRequirement>();
        var requirement = new OpenApiSecurityRequirement
        {
            [document.Components.SecuritySchemes["BearerAuth"]] = new List<string>()
        };
        document.SecurityRequirements.Add(requirement);

        // اگر خواستی برای هر operation هم explicit اضافه کنی:
        foreach (var path in document.Paths.Values)
        {
            foreach (var op in path.Operations.Values)
            {
                op.Security ??= new List<OpenApiSecurityRequirement>();
                op.Security.Add(requirement);
            }
        }

        return Task.CompletedTask;
    });
});

builder.Services.AddSignalR(o =>
{
    o.EnableDetailedErrors = true;
    o.MaximumReceiveMessageSize = 102400000;
}).AddJsonProtocol(options => {
    options.PayloadSerializerOptions.PropertyNamingPolicy = null;
});

var app = builder.Build();
app.MapControllers();

// Configure the HTTP request pipeline.

app.MapScalarApiReference();
app.MapOpenApi();


app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<SignalRHub>("/ChatHub", option =>
    {
        option.Transports = HttpTransportType.WebSockets |
                            HttpTransportType.LongPolling
            ;
    });
});

app.Run();