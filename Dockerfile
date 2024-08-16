FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /hermesApp
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["Hermes.API/Hermes.API.csproj", "Hermes.API/"]
COPY ["Hermes.Infrastructure/Hermes.Infrastructure.csproj", "Hermes.Infrastructure/"]
COPY ["Hermes.Application/Hermes.Application.csproj", "Hermes.Application/"]

RUN dotnet restore "Hermes.API/Hermes.API.csproj"

COPY . .

WORKDIR "/src/Hermes.API"

RUN dotnet build "Hermes.API.csproj" -c $BUILD_CONFIGURATION -o /hermesApp/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Hermes.API.csproj" -c $BUILD_CONFIGURATION -o /hermesApp/publish

FROM base AS final
WORKDIR /hermesApp
COPY --from=publish /hermesApp/publish .

ENTRYPOINT ["dotnet", "Hermes.API.dll"]
