FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["TradeHelper/TradeHelper.csproj", "TradeHelper/"]
RUN dotnet restore "TradeHelper/TradeHelper.csproj"
COPY . .
WORKDIR "/src/TradeHelper"
RUN dotnet build "TradeHelper.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TradeHelper.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TradeHelper.dll"]
