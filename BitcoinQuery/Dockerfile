FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /app

copy *.sln .
COPY . .

RUN dotnet restore
RUN dotnet build -c Release --no--restore -v normal

WORKDIR /app/BitcoinQuery.WebGateway
RUN dotnet build -c Release -o /app/build -v normal

WORKDIR /app

COPY BitcoinQuery.WebGateway/bin/Release/net8.0/ ./

ENTRYPOINT ["dotnet", "BitcoinQuery.WebGateway"]