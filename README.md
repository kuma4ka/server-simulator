# Server Simulator 🎮

A robust .NET 8 simulation engine designed to emulate high-load game server environments. This project demonstrates advanced asynchronous programming models, Dependency Injection (DI), and clean architecture principles.

## 🚀 Features

- **Async/Await Concurrency**: Efficiently handles thousands of simulated player connections using `SemaphoreSlim` and `Task` based parallelism.
- **Solid Architecture**: Strictly follows SOLID principles with heavy usage of Dependency Injection.
- **Clean Code**: Refactored from legacy codebases to modern .NET 8 standards (Primary Constructors, Records).
- **Configuration Management**: JSON-based dynamic server configuration.

## 🛠 Tech Stack

- **Core**: .NET 8 (C# 12)
- **Validation**: FluentValidation
- **Logging**: Microsoft.Extensions.Logging
- **Data**: Newtonsoft.Json

## 🏗 Structure

- **ServerSimulatorLib**: Core logic containing server entities, workload managers, and state management.
- **ServerSimulator.ConsoleApp**: Entry point responsible for DI container composition and simulation lifecycle.

## ▶️ How to Run

1. Configure your servers in `servers_config.json`.
2. Run the console application:
   ```bash
   dotnet run --project src/ServerSimulator.ConsoleApp
3. Enter the path to the config file when prompted (e.g., servers_config.json).