# Server Simulator 🕹️

![.NET Build & Test](https://github.com/kuma4ka/server-simulator/actions/workflows/dotnet.yml/badge.svg?branch=main)](https://github.com/kuma4ka/server-simulator/actions/workflows/dotnet.yml)

A highly concurrent simulation engine designed to emulate high-load game server environments. This project demonstrates the restoration and refactoring of legacy systems into a modern, **Clean Architecture** solution.

## 🚀 Features

- **Async/Await Concurrency**: Efficiently handles thousands of simulated player connections using `SemaphoreSlim` and `Task` based parallelism.
- **Clean Architecture**: Separation of concerns with distinct layers for Domain logic, Application, and UI.
- **Robust Validation**: Prevents invalid configurations using fluent validation rules.
- **Containerization**: Fully dockerized application using multi-stage builds for optimized image size.
- **Automated Testing**: Critical business logic covered by **xUnit** tests with **FluentAssertions**.
- **CI/CD Pipeline**: Automated build and test workflow via **GitHub Actions**.

## 🛠 Tech Stack

- **Core**: .NET 8 (C# 12)
- **Architecture**: Dependency Injection (DI), SOLID
- **Validation**: FluentValidation
- **Testing**: xUnit, NSubstitute, FluentAssertions
- **DevOps**: Docker, GitHub Actions
- **Data**: Newtonsoft.Json

## 🏗 Structure

The solution follows Clean Architecture principles:

- **src/ServerSimulator.Library**: Core domain logic, entities, interfaces, and business rules.
- **src/ServerSimulator.Application**: Console UI, Dependency Injection setup, and application entry point.
- **src/ServerSimulator.Tests**: Unit tests ensuring reliability of the core logic.

## ▶️ How to Run

### Option 1: Local .NET CLI

1. Configure your servers in `src/ServerSimulator.Application/servers_config.json`.
2. Run the application:
   ```bash
   dotnet run --project src/ServerSimulator.Application
3. Follow the interactive prompts in the console.

### Option 2: Docker 🐳

Run the application in an isolated container:

1. Build the image:
   ```bash
   docker build -t server-simulator .
2. Run the container (interactive mode):
   ```bash
   docker run -it --rm server-simulator
3. Follow the interactive prompts in the console.

### 🧪 Running Tests

To verify the system integrity:
```bash
dotnet test
```
---
### 👨‍💻 Author
Refactored and maintained by [Dmytro Prokopenko](www.linkedin.com/in/dmytro-prokopenko-dev).