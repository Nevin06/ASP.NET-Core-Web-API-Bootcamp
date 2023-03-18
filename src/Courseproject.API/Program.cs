using Courseproject.API;
using Courseproject.Business;
using Courseproject.Common.Interfaces;
using Courseproject.Common.Model;
using Courseproject.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using Microsoft.OpenApi.Models;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
//103
builder.Host.UseSerilog((contxt, provider, config) => config
.Enrich.FromLogContext()        //Enriching our logs by using logging scope and we can add variables to this scope
.WriteTo.File("log.txt", rollingInterval: RollingInterval.Day,
outputTemplate:"{Time:HH:mm:ss} [{Level:u3}] {Message:lj} " +
"{Properties:j} {Newline} {Exception}"));      //u3 is level name

// Add services to the container.
//65
DIConfiguration.RegisterServices(builder.Services);
//89 read environment varables
var dbFilename = Environment.GetEnvironmentVariable("DB_FILENAME");

//61
//builder.Services.AddDbContext<ApplicationDbContext>(); //registered
builder.Services.AddDbContext<ApplicationDbContext>(
    options => options.UseSqlite($"Filename={dbFilename}")); //89

//65
builder.Services.AddScoped<IGenericRepository<Address>, GenericRepository<Address>>();
//66
builder.Services.AddScoped<IGenericRepository<Job>, GenericRepository<Job>>();
//68
builder.Services.AddScoped<IGenericRepository<Employee>, GenericRepository<Employee>>();
//70
builder.Services.AddScoped<IGenericRepository<Team>, GenericRepository<Team>>();
//98
builder.Services.AddScoped<IFileService, FileService>();
//101
builder.Services.AddScoped<IUploadService, AzureBlobUploadService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
//96
var authUrl = Environment.GetEnvironmentVariable("AUTH_URL");
var tokenUrl = Environment.GetEnvironmentVariable("TOKEN_URL");
var oauthScope = Environment.GetEnvironmentVariable("SCOPE");

builder.Services.AddSwaggerGen(o =>
{
    o.AddSecurityDefinition("oauth", new OpenApiSecurityScheme()
    {
        Description = "Auth Code Flow + PKCE",
        Name = "oauth",
        Type = SecuritySchemeType.OAuth2,
        Flows = new OpenApiOAuthFlows()
        {
            AuthorizationCode = new OpenApiOAuthFlow()
            {
                AuthorizationUrl = new Uri(authUrl),
                TokenUrl = new Uri(tokenUrl),
                Scopes = new Dictionary<string, string>()
                {
                    { oauthScope, "Access the API" }
                }
            }
        }
    });
    o.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme()
            {
                Reference = new OpenApiReference(){ Type = ReferenceType.SecurityScheme, Id = "oauth"}
            },
            new List<string>() { oauthScope }
        }
    });
});



//93
var clientId = Environment.GetEnvironmentVariable("CLIENT_ID");
var tenantId = Environment.GetEnvironmentVariable("TENANT_ID");
var clientSecret = Environment.GetEnvironmentVariable("CLIENT_SECRET");
var adDomain = Environment.GetEnvironmentVariable("AD_DOMAIN");
var callBackPath = Environment.GetEnvironmentVariable("CALLBACK_PATH");
var identityInstance = Environment.GetEnvironmentVariable("IDENTITY_INSTANCE");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi((bearerOptions) => { },
    (mioptions) =>
    {
        mioptions.ClientId = clientId;
        mioptions.TenantId = tenantId;
        mioptions.ClientSecret = clientSecret;
        mioptions.CallbackPath= callBackPath;
        mioptions.Domain = adDomain;
        mioptions.Instance = identityInstance;
    });

var app = builder.Build(); //61 needs app this statement

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{                                     //96
    app.UseSwagger();
//app.UseSwaggerUI();
app.UseSwaggerUI(o =>
{
    o.OAuthClientId(clientId);
    o.OAuthUsePkce();
    o.OAuthScopeSeparator(" ");
});

//}                                     //96

//61
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.EnsureCreated(); //generates our db
}
//71
app.UseMiddleware<ExceptionMiddleware>();
//103
//request logging -> logging requests and data about these requests
app.UseSerilogRequestLogging();

app.UseHttpsRedirection();
//93
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
