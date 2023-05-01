#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base

RUN apt-get update && apt-get install -y openssl
COPY ["PlacowkaOswiatowaQuiz.pfx", "/https/PlacowkaOswiatowaQuiz.pfx"]

WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["PlacowkaOswiatowaQuiz/PlacowkaOswiatowaQuiz.csproj", "PlacowkaOswiatowaQuiz/"]
COPY ["PlacowkaOswiatowaQuiz.Shared/PlacowkaOswiatowaQuiz.Shared.csproj", "PlacowkaOswiatowaQuiz.Shared/"]
RUN dotnet restore "PlacowkaOswiatowaQuiz/PlacowkaOswiatowaQuiz.csproj" --disable-parallel
COPY . .
WORKDIR "/src/PlacowkaOswiatowaQuiz"
RUN dotnet build "PlacowkaOswiatowaQuiz.csproj" -c Release -o /app/build --no-restore

FROM build AS publish
RUN dotnet publish "PlacowkaOswiatowaQuiz.csproj" -c Release -o /app/publish /p:UseAppHost=false --no-restore

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PlacowkaOswiatowaQuiz.dll"]