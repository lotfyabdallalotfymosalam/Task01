# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

ASP.NET Core 9.0 MVC application demonstrating real-time employee management with SignalR. No database — data is broadcast-only via WebSockets (no persistence).

## Build & Run Commands

```bash
dotnet build
dotnet run                    # HTTP: http://localhost:5195, HTTPS: https://localhost:7171
dotnet run --launch-profile https
```

## Client-Side Libraries

Managed via LibMan (`libman.json`). To restore:
```bash
dotnet tool install -g Microsoft.Web.LibraryManager.Cli
libman restore
```

SignalR JS client (`@microsoft/signalr@6.0.6`) is installed to `wwwroot/lib/microsoft/signalr/`.

## Architecture

**Real-time data flow:**
1. User POSTs form at `/Employee/Create`
2. `EmployeeController` receives the model, broadcasts via `IHubContext<EmployeeHub>.Clients.All.SendAsync("ReceiveNewEmployee", name, address, age)`
3. `/Employee/Index` page has a SignalR JS client connected to `/employeeHub` that listens for `"ReceiveNewEmployee"` and appends a table row

**Key convention:** The hub (`EmployeeHub`) is empty — all broadcasting is done from the controller using `IHubContext<T>`, not from the hub class itself.

**SignalR endpoint:** `/employeeHub` (mapped in `Program.cs`)

## Important Notes

- Comments in controller/view files are in Arabic (educational project)
- Database save is commented out in `EmployeeController.Create` — only SignalR broadcast is active
- The JS client must use `new signalR.HubConnectionBuilder()` (capital `H`, under `signalR` namespace)
