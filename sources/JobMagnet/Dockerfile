FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine AS base
WORKDIR /app

RUN apk add --no-cache icu-libs

ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
USER $APP_UID
EXPOSE 80
EXPOSE 443

RUN echo "Download ASP.NET ALPINE "

FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["JobMagnet.csproj", "."]
RUN dotnet restore "JobMagnet.csproj"
COPY . .
WORKDIR /src
RUN dotnet build "JobMagnet.csproj" -c $BUILD_CONFIGURATION --no-restore

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "JobMagnet.csproj" -c $BUILD_CONFIGURATION -o /app/publish --no-build

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "JobMagnet.dll"]