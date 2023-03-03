using Microsoft.EntityFrameworkCore;
using HRMSem3ListApi;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<HRMSem3Db>(opt => opt.UseInMemoryDatabase("HRMSem3ListApi"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
var app = builder.Build();

app.MapGet("/hrmsem3s", async (HRMSem3Db db) =>
    await db.HRMSem3s.ToListAsync());

app.MapGet("/hrmsem3s/iscomplete", async (HRMSem3Db db) =>
    await db.HRMSem3s.Where(t => t.IsComplete).ToListAsync());

app.MapGet("/hrmsem3s/{id}", async (int id, HRMSem3Db db) =>
    await db.HRMSem3s.FindAsync(id)
        is HRMSem3 hrmsem3
            ? Results.Ok(hrmsem3)
            : Results.NotFound());

app.MapPost("/hrmsem3s", async (HRMSem3 hrmsem3, HRMSem3Db db) =>
{
    db.HRMSem3s.Add(hrmsem3);
    await db.SaveChangesAsync();

    return Results.Created($"/hrmsem3s/{hrmsem3.Id}", hrmsem3);
});

app.MapPut("hrmsem3s/{id}", async (int id, HRMSem3 inputHRMSem3, HRMSem3Db db) =>
{
    var hrmsem3 = await db.HRMSem3s.FindAsync(id);

    if (hrmsem3 is null) return Results.NotFound();

    hrmsem3.Name = inputHRMSem3.Name;
    hrmsem3.IsComplete = inputHRMSem3.IsComplete;

    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.MapDelete("/hrmsem3s/{id}", async (int id, HRMSem3Db db) =>
{
    if (await db.HRMSem3s.FindAsync(id) is HRMSem3 hrmsem3)
    {
        db.HRMSem3s.Remove(hrmsem3);
        await db.SaveChangesAsync();
        return Results.Ok();
    }

    return Results.NoContent();
});

app.MapGet("/hrmsem3s/search/{Name}", async (string name, HRMSem3Db db) =>
{
    var hrmsem3 = await db.HRMSem3s
        .Where(e => e.Name!.Contains(name))
        .ToListAsync();

    if (hrmsem3 is null || hrmsem3.Count == 0) return Results.NotFound();

    return Results.Ok(hrmsem3);
});
app.Run();