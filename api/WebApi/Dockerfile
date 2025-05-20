# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Directory.Packages.props", "."]
COPY ["Directory.Build.props", "."]
COPY ["nuget.config", "."]
COPY ["api/WebApi/WebApi.csproj", "api/WebApi/"]
COPY ["modules/Authentications/Authentications.Infrastructure/Authentications.Infrastructure.csproj", "modules/Authentications/Authentications.Infrastructure/"]
COPY ["commons/Infrastructure/Infrastructure.csproj", "commons/Infrastructure/"]
COPY ["commons/Application/Application.csproj", "commons/Application/"]
COPY ["commons/Domain/Domain.csproj", "commons/Domain/"]
COPY ["modules/Authentications/Authentications.Application/Authentications.Application.csproj", "modules/Authentications/Authentications.Application/"]
COPY ["modules/Authentications/Authentications.Domain/Authentications.Domain.csproj", "modules/Authentications/Authentications.Domain/"]
COPY ["modules/Notifications/Notifications.Infrastructure/Notifications.Infrastructure.csproj", "modules/Notifications/Notifications.Infrastructure/"]
COPY ["modules/Notifications/Notifications.Application/Notifications.Application.csproj", "modules/Notifications/Notifications.Application/"]
COPY ["modules/Notifications/Notifications.Domain/Notifications.Domain.csproj", "modules/Notifications/Notifications.Domain/"]
COPY ["modules/Reports/Reports.Infrastructure/Reports.Infrastructure.csproj", "modules/Reports/Reports.Infrastructure/"]
COPY ["modules/Reports/Reports.Application/Reports.Application.csproj", "modules/Reports/Reports.Application/"]
COPY ["modules/Reports/Reports.Domain/Reports.Domain.csproj", "modules/Reports/Reports.Domain/"]
RUN dotnet restore "./api/WebApi/WebApi.csproj"
COPY . .
WORKDIR "/src/api/WebApi"
RUN dotnet build "./WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebApi.dll"]