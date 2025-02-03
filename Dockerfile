# Étape 1: Build de l’application
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app
COPY . . 
RUN dotnet publish -c Release -o out

# Étape 2: Runtime de l’application
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .
CMD ["dotnet", "Cuby.API.dll"]
