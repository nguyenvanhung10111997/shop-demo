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
COPY ["example.order.domain/example.order.domain.csproj", "example.order.domain/"]
COPY ["example.order.infrastructure/example.order.infrastructure.csproj", "example.order.infrastructure/"]
COPY ["example.order.service/example.order.service.csproj", "example.order.service/"]
COPY ["example.order.api/example.order.api.csproj", "example.order.api/"]

# Restore project dependencies
RUN dotnet restore "example.order.api/example.order.api.csproj"

# Copy the remaining files and build the application
COPY . .
WORKDIR "/src/example.order.api"
RUN dotnet build "example.order.api.csproj" -c Release -o /app/build

# Publish Image
FROM build AS publish
RUN dotnet publish "example.order.api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final Image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENV ASPNETCORE_URLS=http://+:80
ENV ASPNETCORE_ENVIRONMENT=orderion
ENTRYPOINT ["dotnet", "example.order.api.dll"]