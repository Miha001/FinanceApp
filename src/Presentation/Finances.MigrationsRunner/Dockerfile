FROM mcr.microsoft.com/dotnet/sdk:10.0-alpine AS build
WORKDIR /src

COPY ["src/Presentation/Finances.MigrationsRunner/Finances.MigrationsRunner.csproj", "src/Presentation/Finances.MigrationsRunner/"]
COPY ["src/Core/Finances.Application/Finances.Application.csproj", "src/Core/Finances.Application/"]
COPY ["src/Core/Finances.Domain/Finances.Domain.csproj", "src/Core/Finances.Domain/"]
COPY ["src/Infrastructure/Finances.DAL.csproj", "src/Infrastructure/"]

RUN dotnet restore "src/Presentation/Finances.MigrationsRunner/Finances.MigrationsRunner.csproj"

COPY . .

RUN dotnet publish "src/Presentation/Finances.MigrationsRunner/Finances.MigrationsRunner.csproj" \
    -c Release \
    -o /app/publish \
    /p:UseAppHost=false \
    --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0-alpine AS final
WORKDIR /app

ENV ASPNETCORE_URLS=http://+:8080
USER app

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Finances.MigrationsRunner.dll"]