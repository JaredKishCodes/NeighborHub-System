# 1. Use the official .NET ASP.NET runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 80

# 2. Use the official .NET SDK image to build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy everything from the repository into the build container
COPY . .

# Let .NET automatically find the .sln or main .csproj file and restore dependencies
RUN dotnet restore

# Build the application in Release mode
RUN dotnet build -c Release -o /app/build

# 3. Publish the compiled binaries
FROM build AS publish
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

# 4. Final step: run the app
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Dynamic search for your API dll so casing issues don't crash the entrypoint
ENTRYPOINT ["sh", "-c", "dotnet $(ls NeighborHub*.dll | grep -i api.dll | head -n 1)"]