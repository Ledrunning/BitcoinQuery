FROM mcr.microsoft.com/dotnet/framework/sdk:4.8-windowsservercore-ltsc2019 AS build

WORKDIR /app

COPY *.sln .
COPY . .

RUN nuget restore
RUN msbuild /p:Configuration=Release

WORKDIR /app/BitcoinQuery.WebGateway
RUN dotnet build -c Release -o /app/build -v normal

WORKDIR /app

COPY BitcoinQuery.WebGateway/bin/Release/net8.0/ ./

ENTRYPOINT ["dotnet", "BitcoinQuery.WebGateway.dll"]
