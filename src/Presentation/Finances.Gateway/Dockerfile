FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["src/Presentation/Finances.Gateway/Finances.Gateway.csproj", "src/Presentation/Finances.Gateway/"]

RUN dotnet restore "src/Presentation/Finances.Gateway/Finances.Gateway.csproj"

COPY . .
WORKDIR "/src/src/Presentation/Finances.Gateway"
RUN dotnet build "Finances.Gateway.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Finances.Gateway.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Finances.Gateway.dll"]