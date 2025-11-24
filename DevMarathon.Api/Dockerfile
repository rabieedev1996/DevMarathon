FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["DevMarathon.Api/DevMarathon.Api.csproj", "DevMarathon.Api/"]
COPY ["DevMarathon.Infrastructure/DevMarathon.Infrastructure.csproj", "DevMarathon.Infrastructure/"]
COPY ["DevMarathon.Application/DevMarathon.Application.csproj", "DevMarathon.Application/"]
COPY ["DevMarathon.Domain/DevMarathon.Domain.csproj", "DevMarathon.Domain/"]
COPY ["DevMarathon.Utility/DevMarathon.Utility.csproj", "DevMarathon.Utility/"]
COPY ["DevMarathon.Identity/DevMarathon.Identity.csproj", "DevMarathon.Identity/"]
RUN dotnet restore "DevMarathon.Api/DevMarathon.Api.csproj"
COPY . .
WORKDIR "/src/DevMarathon.Api"
RUN dotnet build "./DevMarathon.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./DevMarathon.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DevMarathon.Api.dll"]
