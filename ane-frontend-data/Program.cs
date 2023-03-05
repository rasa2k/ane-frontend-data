using Swashbuckle.AspNetCore.Annotations;
using Microsoft.EntityFrameworkCore;
using aneFrontendData;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<UserDbContext>(options => options.UseInMemoryDatabase("Users"));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAuthentication().AddJwtBearer();
builder.Services.AddAuthorization();
builder.Services.AddSwaggerGen(opts => opts.EnableAnnotations());


var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();


app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

// GET UTC
app.MapGet("time/utc", () => Results.Ok(DateTime.UtcNow)).WithMetadata(new SwaggerOperationAttribute(summary: "Summary", description: "Descritption Test"));

app.MapGet("/show/", async () =>
{
    var reader = new StreamReader("./data/topoJson.json");
    string postData = await reader.ReadToEndAsync();

    //var data = JsonSerializer.Serialize(postData);
    return await Task.FromResult<string>(postData);
});

app.MapGet("/users", async (UserDbContext context) =>
await context.Users.ToListAsync())
.WithName("GetAllUsers");

app.MapGet("/users/{id}", async (int id, UserDbContext context) =>
await context.Users.FindAsync(id)
    is User user
        ? Results.Ok(user)
        : Results.NotFound())
.WithName("GetUserById")
.Produces<User>(StatusCodes.Status200OK)
.Produces(StatusCodes.Status404NotFound);

app.MapPost("/users", async (User user, UserDbContext context) =>
{
    context.Users.Add(user);
    await context.SaveChangesAsync();

    return Results.Created($"/users/{user.Id}", user);
})
.WithName("PostUser")
.ProducesValidationProblem()
.Produces<User>(StatusCodes.Status201Created);

app.MapPut("/users/{id}", async (int id, User userUpdate, UserDbContext context) =>
{
    var user = await context.Users.FindAsync(id);

    if (user is null) return Results.NotFound();

    user.Name = userUpdate.Name;
    user.Email = userUpdate.Email;
    await context.SaveChangesAsync();
    return Results.NoContent();
})
.WithName("UpdateUser")
.ProducesValidationProblem()
.Produces(StatusCodes.Status204NoContent)
.Produces(StatusCodes.Status404NotFound);

app.MapDelete("/users/{id}", async (int id, UserDbContext context) =>
{
    if (await context.Users.FindAsync(id) is User user)
    {
        context.Users.Remove(user);
        await context.SaveChangesAsync();
        return Results.Ok(user);
    }

    return Results.NotFound();
})
.WithName("DeleteUser")
.Produces(StatusCodes.Status204NoContent)
.Produces(StatusCodes.Status404NotFound);

await app.RunAsync();
