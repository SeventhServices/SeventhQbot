#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:5.0-buster AS build
WORKDIR /src
COPY ["src/Seventh.Bot/Seventh.Bot.csproj", "src/Seventh.Bot/"]
COPY ["src/Seventh.Resource.Asset.SqlLoader/Seventh.Resource.Asset.SqlLoader.csproj", "src/Seventh.Resource.Asset.SqlLoader/"]
COPY ["src/Seventh.Bot.Client/Seventh.Bot.Client.csproj", "src/Seventh.Bot.Client/"]
COPY ["src/Seventh.Bot.Resource/Seventh.Bot.Resource.csproj", "src/Seventh.Bot.Resource/"]
COPY ["src/Sagilio.Bot.Parser/Sagilio.Bot.Parser.csproj", "src/Sagilio.Bot.Parser/"]
RUN dotnet restore "src/Seventh.Bot/Seventh.Bot.csproj"
COPY . .
WORKDIR "/src/src/Seventh.Bot"
RUN dotnet build "Seventh.Bot.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Seventh.Bot.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Seventh.Bot.dll"]