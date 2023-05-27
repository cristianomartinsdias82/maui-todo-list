using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using System.Net;
using ToDoApi.ToDo.Models;
using ToDoApi.ToDo.Persistence;
using ToDoApi.ToDo.ViewModels;

namespace ToDoApi.ToDo.Endpoints;

public static class TodoEndpointsConfiguration
{
    private const string ToDoApiRouteBase = "api/todo";

    public static void Configure(WebApplication app)
    {
        //GET
        app.MapGet(ToDoApiRouteBase, async (ToDoDbContext dbContext, CancellationToken ct) =>
        {
            var todos = await dbContext
                            .ToDos
                            .OrderByDescending(td => td.CreatedAt)
                            .ToListAsync(ct);

            return new ListToDosViewModel
            {
                ToDos =
                todos?.Select(td => new ToDoViewModelItem
                {
                    Id = td.Id,
                    CreatedAt = td.CreatedAt,
                    Details = td.Details,
                    Done = td.Done,
                    Title = td.Title
                })?.ToList() ?? Enumerable.Empty<ToDoViewModelItem>()
            };
        });

        //POST
        app.MapPost(ToDoApiRouteBase, async (ToDoDbContext dbContext, ToDoViewModelItem viewModel, CancellationToken ct) =>
        {
            var newToDo = new Models.ToDo
            {
                Details = viewModel.Details,
                Done = viewModel.Done,
                Title = viewModel.Title
            };

            await dbContext
                    .ToDos
                    .AddAsync(newToDo, ct);
            await dbContext.SaveChangesAsync(ct);

            return Results.Created(ToDoApiRouteBase + $"/{newToDo.Id}", newToDo);
        });

        //PUT
        app.MapPut(ToDoApiRouteBase + "/{id:Guid}", async (ToDoDbContext dbContext, Guid id, ToDoViewModelItem viewModel, CancellationToken ct) =>
        {
            var toDo = await dbContext.FindAsync<Models.ToDo>(id);
            if (toDo is null)
                return Results.NotFound();

            toDo.Title = viewModel.Title;
            toDo.Details = viewModel.Details;
            toDo.Done = viewModel.Done;

            await dbContext.SaveChangesAsync(ct);

            return Results.NoContent();
        });

        //DELETE
        app.MapDelete(ToDoApiRouteBase + "/{id:Guid}", async (ToDoDbContext dbContext, Guid id, CancellationToken ct) =>
        {
            var toDo = await dbContext.FindAsync<Models.ToDo>(id);
            if (toDo is null)
                return Results.NotFound();
            
            dbContext.Remove(toDo);
            await dbContext.SaveChangesAsync(ct);

            return Results.NoContent();
        });
    }
}