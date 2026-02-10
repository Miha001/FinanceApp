FROM mcr.microsoft.com/dotnet/sdk:10.0-alpine AS build
WORKDIR /src

COPY ["src/Presentation/Finances.Worker/Finances.Worker.csproj", "src/Presentation/Finances.Worker/"]
COPY ["src/Core/Finances.Application/Finances.Application.csproj", "src/Core/Finances.Application/"]
COPY ["src/Core/Finances.Domain/Finances.Domain.csproj", "src/Core/Finances.Domain/"]
COPY ["src/Infrastructure/Finances.DAL.csproj", "src/Infrastructure/"]

RUN dotnet restore "src/Presentation/Finances.Worker/Finances.Worker.csproj"

COPY . .

WORKDIR "/src/src/Presentation/Finances.Worker"
RUN dotnet publish "Finances.Worker.csproj" -c Release -o /app/publish /p:UseAppHost=false --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0-alpine AS final
WORKDIR /app

ENV ASPNETCORE_URLS=http://+:8080
USER app

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Finances.Worker.dll"]