# Blazing Trails

Blazing Trails app built while studying [Blazor in Action](https://www.manning.com/books/blazor-in-action).

## Initialize Database

```bash
# dotnet ef migrations add Initial --project src/BlazingTrails.Infrastructure/ --startup-project src/BlazingTrails.Api/
dotnet ef database update --project src/BlazingTrails.Infrastructure/ --startup-project src/BlazingTrails.Api/
```

## Run

```bash
dotnet watch --project src/BlazingTrails.Api/ --launch-profile https
```

## Seed data

```bash
dotnet run --project src/BlazingTrails.Api.DataSeeder/
```
