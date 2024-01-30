# Blazing Trails

Blazing Trails app built while studying [Blazor in Action](https://www.manning.com/books/blazor-in-action).

## Initialize SQLite

```bash
dotnet ef migrations add Initial --project src/BlazingTrails.Server/ --output-dir Persistence/Data/Migrations
dotnet ef database update --project src/BlazingTrails.Server/
```

## Run

```bash
dotnet watch --project src/BlazingTrails.Server/ --launch-profile https
```
