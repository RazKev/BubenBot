FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
COPY . /app
WORKDIR /app
RUN dotnet restore
RUN dotnet build --configuration Release --no-restore

FROM mcr.microsoft.com/dotnet/runtime:5.0
COPY --from=build /app/BubenBot.Bot/bin/Release/netcoreapp5.0 /app
WORKDIR /app
ENTRYPOINT [ "dotnet", "BubenBot.Bot.dll" ]