#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
# Copy all the referenced projects
COPY src/SharedKernel/SharedKernel.csproj ./SharedKernel/SharedKernel.csproj
COPY src/Domain/Domain.csproj ./Domain/Domain.csproj
COPY src/Application/Application.csproj ./Application/Application.csproj
COPY src/Infrastructure/Infrastructure.csproj ./Infrastructure/Infrastructure.csproj
COPY src/Contracts/Contracts.csproj ./Contracts/Contracts.csproj
COPY src/WebApi/WebApi.csproj  ./WebApi/WebApi.csproj
# Restore packages based on startup project
RUN dotnet restore "./WebApi/WebApi.csproj"
# Copy source code
COPY src/SharedKernel/ ./SharedKernel/
COPY src/Domain/ ./Domain/
COPY src/Application/ ./Application/
COPY src/Infrastructure/ ./Infrastructure/
COPY src/Contracts/ ./Contracts/
COPY src/WebApi/  ./WebApi/
WORKDIR "/src/WebApi"

RUN dotnet build "./WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Define entrypoint
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebApi.dll"]
