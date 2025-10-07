#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# Base Image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Build Image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj files and restore dependencies
COPY ["example.product.domain/example.product.domain.csproj", "example.product.domain/"]
COPY ["example.product.infrastructure/example.product.infrastructure.csproj", "example.product.infrastructure/"]
COPY ["example.product.service/example.product.service.csproj", "example.product.service/"]
COPY ["example.product.api/example.product.api.csproj", "example.product.api/"]

# Restore project dependencies
RUN dotnet restore "example.product.api/example.product.api.csproj"

# Copy the remaining files and build the application
COPY . .
WORKDIR "/src/example.product.api"
RUN dotnet build "example.product.api.csproj" -c Release -o /app/build

# Publish Image
FROM build AS publish
RUN dotnet publish "example.product.api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final Image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENV ASPNETCORE_URLS=http://+:80
ENV ASPNETCORE_ENVIRONMENT=Production
ENTRYPOINT ["dotnet", "example.product.api.dll"]