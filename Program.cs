using Microsoft.OpenApi.Models;
using InfoneticaTask.Models;
using InfoneticaTask.Services;


var builder = WebApplication.CreateBuilder(args);

// Register services
builder.Services.AddSingleton<WorkflowService>();
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

// Enable Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Workflow API V1");
    });
}

app.UseHttpsRedirection();

// Base route
app.MapGet("/", () => "Welcome to the Workflow API!");

// Create Workflow Definition
app.MapPost("/workflows", (WorkflowDefinition definition, WorkflowService service) =>
{
    if (definition == null)
        return Results.BadRequest("Invalid or missing workflow definition JSON.");

    var result = service.CreateWorkflow(definition);
    return result.IsSuccess ? Results.Ok(new { message = result.Message, workflow = definition }) : Results.BadRequest(result.Message);
});

// Get All Workflows
app.MapGet("/workflows", (WorkflowService service) =>
{
    var workflows = service.GetAllWorkflows();
    return Results.Ok(workflows);
});

// Get All Workflow Instances
app.MapGet("/instances", (WorkflowService service) =>
{
    var instances = service.GetAllInstances();
    return Results.Ok(instances);
});


// Get Workflow Definition by ID
app.MapGet("/workflows/{id}", (string id, WorkflowService service) =>
{
    var wf = service.GetWorkflow(id);
    return wf is not null ? Results.Ok(wf) : Results.NotFound("Workflow not found");
});

// Start Workflow Instance
app.MapPost("/instances/{workflowId}", (string workflowId, WorkflowService service) =>
{
    var instance = service.StartWorkflowInstance(workflowId);
    return instance is not null ? Results.Ok(instance) : Results.BadRequest("Invalid workflow");
});

// Execute Action on Instance
app.MapPost("/instances/{instanceId}/actions/{actionId}", (string instanceId, string actionId, WorkflowService service) =>
{
    var result = service.ExecuteAction(instanceId, actionId);
    return result.IsSuccess ? Results.Ok(result.Message) : Results.BadRequest(result.Message);
});

// Get Instance State & History
app.MapGet("/instances/{instanceId}", (string instanceId, WorkflowService service) =>
{
    var instance = service.GetInstance(instanceId);
    return instance is not null ? Results.Ok(instance) : Results.NotFound("Instance not found");
});

app.Run();
