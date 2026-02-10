FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["src/Presentation/Finances.API/Finances.API.csproj", "src/Presentation/Finances.API/"]
COPY ["src/Core/Finances.Application/Finances.Application.csproj", "src/Core/Finances.Application/"]
COPY ["src/Core/Finances.Domain/Finances.Domain.csproj", "src/Core/Finances.Domain/"]
COPY ["src/Infrastructure/Finances.DAL.csproj", "src/Infrastructure/"]

RUN dotnet restore "src/Presentation/Finances.API/Finances.API.csproj"

COPY . .
WORKDIR "/src/src/Presentation/Finances.API"
RUN dotnet build "Finances.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Finances.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Finances.API.dll"]