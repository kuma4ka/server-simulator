FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["src/ServerSimulator.Library/ServerSimulator.Library.csproj", "ServerSimulator.Library/"]
COPY ["src/ServerSimulator.Application/ServerSimulator.Application.csproj", "ServerSimulator.Application/"]

RUN dotnet restore "ServerSimulator.Application/ServerSimulator.Application.csproj"

COPY src/ .

WORKDIR "/src/ServerSimulator.Application"
RUN dotnet publish "ServerSimulator.Application.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/runtime:8.0 AS final
WORKDIR /app

RUN adduser -u 5678 --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser

COPY --from=build /app/publish .

COPY src/ServerSimulator.Application/servers_config.json .

ENTRYPOINT ["dotnet", "ServerSimulator.Application.dll"]