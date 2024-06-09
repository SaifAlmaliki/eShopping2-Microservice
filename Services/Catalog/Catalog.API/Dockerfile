# Base stage: Use the official ASP.NET Core runtime image from Microsoft
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

# Switch to the 'app' user
USER app

# Set the working directory to /app
WORKDIR /app

# Expose ports 8080 and 8081
EXPOSE 8080
EXPOSE 8081

# Build stage: Use the official .NET SDK image for building the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Argument to specify the build configuration, default is Release
ARG BUILD_CONFIGURATION=Release

# Set the working directory to /src
WORKDIR /src

# Copy the project files for Catalog.API and Shared projects to the working directory
COPY ["Services/Catalog/Catalog.API/Catalog.API.csproj", "Services/Catalog/Catalog.API/"]
COPY ["Shared/Shared.csproj", "Shared/"]

# Restore the dependencies for the Catalog.API project
RUN dotnet restore "./Services/Catalog/Catalog.API/Catalog.API.csproj"

# Copy all the files from the current context to the working directory
COPY . .

# Set the working directory to the Catalog.API project folder
WORKDIR "/src/Services/Catalog/Catalog.API"

# Build the Catalog.API project with the specified configuration (default: Release) and output to /app/build
RUN dotnet build "./Catalog.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish stage: Use the build image as the base for the publish stage
FROM build AS publish

# Argument to specify the build configuration, default is Release
ARG BUILD_CONFIGURATION=Release

# Publish the Catalog.API project with the specified configuration (default: Release) to /app/publish
# The /p:UseAppHost=false option is used to disable the creation of an executable host
RUN dotnet publish "./Catalog.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final stage: Use the base runtime image and set up the final application image
FROM base AS final

# Set the working directory to /app
WORKDIR /app

# Copy the published files from the publish stage to the current working directory
COPY --from=publish /app/publish .

# Define the entrypoint for the container to run the Catalog.API application
ENTRYPOINT ["dotnet", "Catalog.API.dll"]
