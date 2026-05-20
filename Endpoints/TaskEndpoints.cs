using Microsoft.EntityFrameworkCore;
using FluentValidation;
using MiAp.Data;
using MiAp.Models;

namespace MiAp.Endpoints;

public static class TaskEndpoints
{
    public static void MapTaskEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/tasks")
                       .WithTags("Tasks")
                       .WithOpenApi();

        // GET: /api/tasks
        group.MapGet("/", async (AppDbContext db) =>
        {
            var tasks = await db.Tasks.ToListAsync();
            return Results.Ok(tasks);
        })
        .WithName("GetAllTasks")
        .Produces<List<TaskItem>>(StatusCodes.Status200OK);

        // GET: /api/tasks/{id}
        group.MapGet("/{id:guid}", async (Guid id, AppDbContext db) =>
        {
            var task = await db.Tasks.FindAsync(id);
            return task is not null ? Results.Ok(task) : Results.NotFound();
        })
        .WithName("GetTaskById")
        .Produces<TaskItem>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        // POST: /api/tasks
        group.MapPost("/", async (TaskItem task, AppDbContext db, IValidator<TaskItem> validator) =>
        {
            var validationResult = await validator.ValidateAsync(task);
            if (!validationResult.IsValid)
            {
                return Results.ValidationProblem(validationResult.ToDictionary());
            }

            task.Id = Guid.NewGuid();
            task.CreatedAt = DateTime.UtcNow;
            task.UpdatedAt = null;

            db.Tasks.Add(task);
            await db.SaveChangesAsync();

            return Results.Created($"/api/tasks/{task.Id}", task);
        })
        .WithName("CreateTask")
        .Produces<TaskItem>(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest);

        // PUT: /api/tasks/{id}
        group.MapPut("/{id:guid}", async (Guid id, TaskItem inputTask, AppDbContext db, IValidator<TaskItem> validator) =>
        {
            var validationResult = await validator.ValidateAsync(inputTask);
            if (!validationResult.IsValid)
            {
                return Results.ValidationProblem(validationResult.ToDictionary());
            }

            var task = await db.Tasks.FindAsync(id);

            if (task is null)
            {
                return Results.NotFound();
            }

            task.Title = inputTask.Title;
            task.Description = inputTask.Description;
            task.Status = inputTask.Status;
            task.Priority = inputTask.Priority;
            task.UpdatedAt = DateTime.UtcNow;

            await db.SaveChangesAsync();

            return Results.NoContent();
        })
        .WithName("UpdateTask")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status404NotFound);

        // DELETE: /api/tasks/{id}
        group.MapDelete("/{id:guid}", async (Guid id, AppDbContext db) =>
        {
            var task = await db.Tasks.FindAsync(id);

            if (task is null)
            {
                return Results.NotFound();
            }

            db.Tasks.Remove(task);
            await db.SaveChangesAsync();

            return Results.NoContent();
        })
        .WithName("DeleteTask")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound);
    }
}
