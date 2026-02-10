FROM mcr.microsoft.com/dotnet/sdk:10.0-alpine AS build
WORKDIR /src

COPY ["src/Presentation/Finances.Gateway/Finances.Gateway.csproj", "src/Presentation/Finances.Gateway/"]

RUN dotnet restore "src/Presentation/Finances.Gateway/Finances.Gateway.csproj"

COPY . .

RUN dotnet publish "src/Presentation/Finances.Gateway/Finances.Gateway.csproj" \
    -c Release \
    -o /app/publish \
    /p:UseAppHost=false \
    --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0-alpine AS final
WORKDIR /app

ENV ASPNETCORE_URLS=http://+:8080
USER app

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Finances.Gateway.dll"]