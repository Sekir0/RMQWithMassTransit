
FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src

COPY . .
RUN dotnet restore Profiles.Api/Profiles.Api.csproj
WORKDIR /src/Profiles.Api
RUN dotnet build Profiles.Api.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish Profiles.Api.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Profiles.Api.dll"]