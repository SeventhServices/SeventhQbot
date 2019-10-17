FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["SeventhServices.QQRobot/SeventhServices.QQRobot.csproj", "SeventhServices.QQRobot/"]
RUN dotnet restore "SeventhServices.QQRobot/SeventhServices.QQRobot.csproj"
COPY . .
WORKDIR "/src/SeventhServices.QQRobot"
RUN dotnet build "SeventhServices.QQRobot.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SeventhServices.QQRobot.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SeventhServices.QQRobot.dll"]