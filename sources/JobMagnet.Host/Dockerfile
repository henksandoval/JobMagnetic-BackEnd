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

COPY sources/JobMagnet.Host/*.csproj ./sources/JobMagnet.Host/
COPY sources/JobMagnet.Application/*.csproj ./sources/JobMagnet.Application/
COPY sources/JobMagnet.Domain/*.csproj ./sources/JobMagnet.Domain/
COPY sources/JobMagnet.Infrastructure/*.csproj ./sources/JobMagnet.Infrastructure/
COPY sources/JobMagnet.Shared/*.csproj ./sources/JobMagnet.Shared/

RUN dotnet restore "sources/JobMagnet.Host/JobMagnet.Host.csproj"

COPY sources/. ./sources/

RUN dotnet publish "sources/JobMagnet.Host/JobMagnet.Host.csproj" -c $BUILD_CONFIGURATION -o /app/publish --no-restore

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "JobMagnet.Host.dll"]