using Microsoft.OpenApi.Models;
using InfoneticaTask.Services;

var builder = WebApplication.CreateBuilder(args);

// Register services
builder.Services.AddSingleton<WorkflowService>();

// Enable Controllers
builder.Services.AddControllers();

// Enable Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Workflow Management API",
        Version = "v1",
        Description = "Configurable Workflow Engine API with state machine functionality"
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Workflow API V1");
    });
}

app.UseHttpsRedirection();

// Map Controllers

//base route for API
app.MapGet("/", () => "Welcome to the Workflow API!");

app.MapControllers();

app.Run();

