using DemoProject;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Scoped -> Creates a new instance for each HTTP Request(Recommended for ASP.NET core most of the time)
//every time server receives request, it will create new instance of this service
// Transient -> Creates a new instance every time the service is requested.
// Singleton (lifetime) -> Creates only one instance for as long as the application is running (only when your application is shutting down, a new instance will be created when application restarts)
builder.Services.AddScoped<NameService>();
builder.Services.AddScoped<SomeOtherService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//54
app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
