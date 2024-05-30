#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:8.0.3-bookworm-slim-amd64 AS base
RUN apt-get update
RUN apt-get install -y restic
USER app
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ResticClient.Console/ResticClient.Console.csproj", "ResticClient.Console/"]
RUN dotnet restore "./ResticClient.Console/ResticClient.Console.csproj"
COPY . .
WORKDIR "/src/ResticClient.Console"
RUN dotnet build "./ResticClient.Console.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ResticClient.Console.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ResticClient.Console.dll"]