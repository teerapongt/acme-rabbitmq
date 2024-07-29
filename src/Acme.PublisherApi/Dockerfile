FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Acme.PublisherApi/Acme.PublisherApi.csproj", "src/Acme.PublisherApi/"]
RUN dotnet restore "src/Acme.PublisherApi/Acme.PublisherApi.csproj"
COPY . .
WORKDIR "/src/src/Acme.PublisherApi"
RUN dotnet build "Acme.PublisherApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Acme.PublisherApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Acme.PublisherApi.dll"]
