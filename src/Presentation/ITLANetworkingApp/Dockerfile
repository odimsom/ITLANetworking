# Usar la imagen oficial de .NET 8 SDK para build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copiar csproj y restaurar dependencias
COPY ./src/Presentation/ITLANetworkingApp/ITLANetworkingApp.csproj ./Presentation/ITLANetworkingApp/
COPY ./src/ITLANetworking.Core.Domain/ITLANetworking.Core.Domain.csproj ./ITLANetworking.Core.Domain/
COPY ./src/ITLANetworking.Core.Application/ITLANetworking.Core.Application.csproj ./ITLANetworking.Core.Application/
COPY ./src/ITLANetworking.Infrastructure.Identity/ITLANetworking.Infrastructure.Identity.csproj ./ITLANetworking.Infrastructure.Identity/
COPY ./src/ITLANetworking.Infrastructure.Persistence/ITLANetworking.Infrastructure.Persistence.csproj ./ITLANetworking.Infrastructure.Persistence/
COPY ./src/ITLANetworking.Infrastructure.Shared/ITLANetworking.Infrastructure.Shared.csproj ./ITLANetworking.Infrastructure.Shared/

RUN dotnet restore ./Presentation/ITLANetworkingApp/ITLANetworkingApp.csproj

# Copiar todo el código y publicar
COPY . .
RUN dotnet publish ./src/Presentation/ITLANetworkingApp/ITLANetworkingApp.csproj -c Release -o /app/publish

# Imagen final runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 5000
ENTRYPOINT ["dotnet", "ITLANetworkingApp.dll"]
