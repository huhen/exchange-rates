FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /

COPY . .
RUN dotnet restore "src/IdentityService/IdentityService.Api/IdentityService.Api.csproj"
RUN dotnet build "src/IdentityService/IdentityService.Api/IdentityService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "src/IdentityService/IdentityService.Api/IdentityService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish

FROM base AS final
WORKDIR /app

RUN groupadd -r appgroup && useradd -r -g appgroup appuser
COPY --from=publish --chown=appuser:appgroup /app/publish .

USER appuser

ENTRYPOINT ["dotnet", "IdentityService.Api.dll"]
