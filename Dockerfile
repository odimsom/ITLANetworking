# Usa la SDK de .NET 8 para build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copia csproj y restaura dependencias
COPY ./src/Presentation/ITLANetworkingApp/*.csproj ./Presentation/ITLANetworkingApp/
COPY ./src/ITLANetworking.Infrastructure.Identity/*.csproj ./ITLANetworking.Infrastructure.Identity/
COPY ./src/ITLANetworking.Infrastructure.Persistence/*.csproj ./ITLANetworking.Infrastructure.Persistence/
COPY ./src/ITLANetworking.Core.Application/*.csproj ./ITLANetworking.Core.Application/
COPY ./src/ITLANetworking.Core.Domain/*.csproj ./ITLANetworking.Core.Domain/
COPY ./src/ITLANetworking.Infrastructure.Shared/*.csproj ./ITLANetworking.Infrastructure.Shared/
RUN dotnet restore ./Presentation/ITLANetworkingApp/ITLANetworkingApp.csproj

# Copia todo el código y publica
COPY ./ ./ 
RUN dotnet publish ./src/Presentation/ITLANetworkingApp/ITLANetworkingApp.csproj -c Release -o /app/publish

# Imagen final runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "ITLANetworkingApp.dll"]
