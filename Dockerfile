# Usa la SDK de .NET 8 para build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copia todo el código fuente
COPY ./src/ ./src/

# Restaurar dependencias (ruta relativa al csproj)
RUN dotnet restore ./src/Presentation/ITLANetworkingApp/ITLANetworkingApp.csproj

# Publicar
RUN dotnet publish ./src/Presentation/ITLANetworkingApp/ITLANetworkingApp.csproj -c Release -o /app/publish

# Imagen final runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "ITLANetworkingApp.dll"]
