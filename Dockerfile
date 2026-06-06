# 1. Use the official .NET ASP.NET runtime image for the execution environment
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 80

# 2. Use the official .NET SDK image to build the application source code
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy all project files to restore dependencies
COPY ["NeighborHub.Api/NeighborHub.Api.csproj", "NeighborHub.Api/"]
COPY ["NeighborHub.Infrastructure/NeighborHub.Infrastructure.csproj", "NeighborHub.Infrastructure/"]
COPY ["NeighborHub.Domain/NeighborHub.Domain.csproj", "NeighborHub.Domain/"]
RUN dotnet restore "NeighborHub.Api/NeighborHub.Api.csproj"

# Copy the rest of the application files and build it
COPY . .
WORKDIR "/src/NeighborHub.API"
RUN dotnet build "NeighborHub.Api.csproj" -c Release -o /app/build

# 3. Publish the compiled binaries
FROM build AS publish
RUN dotnet publish "NeighborHub.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# 4. Final step: copy the published app into the runtime image and start it
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NeighborHub.Api.dll"]