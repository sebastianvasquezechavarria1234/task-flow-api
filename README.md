# MiAp – Premium Minimal ASP.NET Core 9.0 Web API

A production‑ready, clean‑architecture Web API built with **ASP.NET Core 9.0** that demonstrates modern backend best practices:

| Feature | Description |
|---|---|
| **Folder structure** | Separate *Endpoints*, *Models*, *Data*, *Middleware* and *Validators* for a clean, maintainable codebase. |
| **CORS** | Global `AllowAll` policy (easily replaceable with a stricter whitelist). |
| **Global exception handling** | Implements `IExceptionHandler` and returns RFC 7807 *Problem Details* for any unhandled error. |
| **SQLite + EF Core** | `AppDbContext` with migrations‑ready SQLite database for persisting Kanban tasks. |
| **CRUD task management** | Minimal‑API group `/api/tasks` with fully typed `TaskItem` entity. |
| **FluentValidation** | Strong validation rules (`TaskItemValidator`) injected automatically. |
| **Serilog** | Structured logging to console and rolling log files (`logs/app‑*.txt`) with request‑logging middleware. |
| **OpenAPI (Swagger/Scalar)** | Auto‑generated OpenAPI spec (`/openapi`) for quick API exploration. |
| **Weather forecast demo** | Simple demo endpoint (`/weatherforecast`) kept for reference. |

## Getting Started

```bash
# Clone the repo
git clone https://github.com/sebastianvasquezechavarria1234/app-web
cd app-web

# Ensure .NET 9 SDK is installed
dotnet --list-sdks   # should show a 9.x version

# Restore packages and run migrations
dotnet restore
dotnet ef migrations add InitialCreate
dotnet ef database update

# Run the API
dotnet run
```

The API will be reachable at `https://localhost:7180` (or the port shown in the console). Open `https://localhost:7180/openapi` to view the generated Swagger UI.

## Project Structure

```
/Endpoints          – Minimal‑API route groups (Weather, Task)
 /WeatherEndpoints.cs
 /TaskEndpoints.cs
/Data               – EF Core DbContext (AppDbContext.cs)
/Models             – Domain entities (TaskItem.cs, WeatherForecast.cs)
/Middleware         – GlobalExceptionHandler.cs
/Validators         – FluentValidation validators (TaskItemValidator.cs)
/Program.cs         – Application bootstrap (Serilog, DI, CORS)
/MiAp.csproj        – Package references (OpenAPI, EF Core, FluentValidation, Serilog)
```

## Logging

Serilog is configured in **Program.cs**:

* Console output with a concise timestamp and level.
* Rolling file output (`logs/app‑<date>.txt`) retained for 7 days.
* Automatic request logging (`HTTP GET /api/tasks responded 200 in 12.34 ms`).

## Contributing

Feel free to open issues or submit pull requests. Follow the existing folder conventions and run `dotnet format` before committing.

## License

MIT – see the `LICENSE` file.

---

*This repository showcases a ready‑to‑deploy, extensible backend that can be used as a starter for production‑grade .NET web services.*
