using Swashbuckle.AspNetCore.Annotations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(opts => opts.EnableAnnotations());

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

// GET UTC
app.MapGet("time/utc", () => Results.Ok(DateTime.UtcNow)).WithMetadata(new SwaggerOperationAttribute(summary: "Summary", description: "Descritption Test")); ;

await app.RunAsync();
