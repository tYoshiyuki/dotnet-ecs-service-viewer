# アプリケーションサーバを準備するステージ
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080

# フロントエンドをビルドするステージ
FROM node:22.17.1 AS build-web
WORKDIR /src
COPY ["DotNetEcsServiceViewer.Front/", "."]
RUN npm ci
RUN npm run build

# バックエンドをビルドするステージ
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-api
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["DotNetEcsServiceViewer.Api/", "."]
RUN dotnet restore "./DotNetEcsServiceViewer.Api.csproj"
RUN dotnet build "./DotNetEcsServiceViewer.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build
RUN dotnet publish "./DotNetEcsServiceViewer.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# アプリケーションサーバにビルド結果をデプロイするステージ
FROM base AS final
WORKDIR /app
COPY --from=build-api /app/publish .
COPY --from=build-web /src/dist/dotnet-ecs-service-viewer-front/browser/ ./wwwroot
ENTRYPOINT ["dotnet", "DotNetEcsServiceViewer.Api.dll"]