using MinimalAPICheatsheet;
//(14)
//Creates WebApplicationBuilder object
//to configure how the ASP.NET services run
var builder = WebApplication.CreateBuilder(args);
//(14)
//Add and configure services


// Add services to the container.
builder.Services.AddScoped<NameService>();

//(14)
//Configure Open API (Swagger)
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//(14)
//After adding and configuring services
//Create an instance of WebApplication object
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//(14)
//Map Minimal API Routes/Endpoints


//83 Middlewares
app.UseHttpLogging();

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

//80
//route is /weatherforecast, its handler here is the lamda expression ()=>
//how to use dependency injection to inject services into these handlers ()
//make it as (NameService nameService)
app.MapGet("/weatherforecast", (NameService nameService) =>
{
    //81
    app.Logger.LogInformation("/weatherforecast called");
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

//82 Statuscodes
app.MapGet("/Statuscodes", (bool ok) => ok ? Results.Ok("Everything is ok!") : Results.BadRequest("Bad request!"));
//84 Routing
//app.MapGet("/", () => "Get called");
app.MapGet("/", () => get);
app.MapPost("/", () => "Post called");
app.MapPut("/", () => "Put called");
app.MapDelete("/", () => "Delete called");

//Routing using local functions
string get() => "Get called";

var personHandler = new PersonHandler();
app.MapGet("/Persons", personHandler.HandleGet);

////Route parameters
//app.MapGet("/Persons/{id}", personHandler.HandleGetById);
//Route parameter constraints
app.MapGet("/Persons/{id:int}", personHandler.HandleGetById);

//Parameter binding
//Person json from body to Person object automatically
app.MapPost("Persons", (Person person) => person.FirstName + " " + person.LastName);

//(14)
//Run the application
app.Run();

//(14)
//Any Additional Data Below Here
internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
