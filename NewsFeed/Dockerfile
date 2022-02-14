FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src

COPY . .
RUN dotnet restore NewsFeed.Api/NewsFeed.Api.csproj
WORKDIR /src/NewsFeed.Api
RUN dotnet build NewsFeed.Api.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish NewsFeed.Api.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "NewsFeed.Api.dll"]