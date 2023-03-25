#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS base

RUN apt-get update && apt-get install -y openssl

#ADD "$env:USERPROFILE/.aspnet/https/Quiz.Api.pfx" /usr/local/share/ca-certificates/Quiz.Api.pfx
#COPY ca-cert.crt /usr/local/share/ca-certificates/ca-cert.crt 
#RUN chmod 644 /usr/local/share/ca-certificates/Quiz.Api.pfx && update-ca-certificates

#RUN dotnet tool install --tool-path ~/ dotnet-certificate-tool
#RUN /certificate-tool add --file /https/Quiz.Api.pfx --password Str0ngP@ssw0rd
COPY ["Quiz.Api.pfx", "/https/Quiz.Api.pfx"]
RUN dotnet dev-certs https -ep /https/Quiz.Api.pfx -p Str0ngP@ssw0rd
RUN dotnet dev-certs https --trust

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