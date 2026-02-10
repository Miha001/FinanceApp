FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["src/Presentation/Finances.Worker/Finances.Worker.csproj", "src/Presentation/Finances.Worker/"]
COPY ["src/Core/Finances.Application/Finances.Application.csproj", "src/Core/Finances.Application/"]
COPY ["src/Core/Finances.Domain/Finances.Domain.csproj", "src/Core/Finances.Domain/"]
COPY ["src/Infrastructure/Finances.DAL.csproj", "src/Infrastructure/"]

RUN dotnet restore "src/Presentation/Finances.Worker/Finances.Worker.csproj"

COPY . .
WORKDIR "/src/src/Presentation/Finances.Worker"
RUN dotnet build "Finances.Worker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Finances.Worker.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Finances.Worker.dll"]