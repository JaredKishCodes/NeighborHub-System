# 1. Use the official .NET ASP.NET runtime image for .NET 10
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 80

# 2. Use the official .NET SDK image for .NET 10 to build
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy all repository contents into the container
COPY . .

# Move directly into the folder containing your backend solution or projects
WORKDIR /src/Backend/NeighborHub

# Restore all projects found within this subdirectory
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

# Dynamic search for your API dll inside the published bundle
ENTRYPOINT ["sh", "-c", "dotnet $(ls *.dll | grep -i api.dll | head -n 1)"]