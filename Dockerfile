# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine3.20-arm64v8 AS build
WORKDIR /source
RUN apk add --no-cache icu-libs
COPY . .
RUN dotnet restore "./src/Avatarize.csproj" --disable-parallel --runtime linux-musl-arm64
RUN dotnet publish "./src/Avatarize.csproj" -c Release -o /app --no-restore --runtime linux-musl-arm64 --self-contained true

# Serve Stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine3.20-arm64v8 AS run
RUN apk add --no-cache icu-libs
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
ENV ASPNETCORE_URLS="http://+:50002"
ENV ASPNETCORE_HTTP_PORT=50002

RUN apk upgrade --no-cache && apk add --no-cache icu-devtools libgdiplus

WORKDIR /app
COPY --from=build /app .
COPY --from=build /source/src/Images /app/Images
COPY --from=build /tmp/certs /app/certs
RUN chmod 644 /app/certs/certificate.pfx
USER dotnetuser

EXPOSE 50002

ENV ASPNETCORE_Kestrel__Certificates__Default__Path="/app/certs/certificate.pfx"

ENTRYPOINT ["./Avatarize"]