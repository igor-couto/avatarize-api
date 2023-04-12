# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:7.0-alpine AS build
WORKDIR /source
COPY . .
RUN dotnet restore "./src/Avatarize.csproj" --disable-parallel --runtime linux-musl-x64
RUN dotnet publish "./src/Avatarize.csproj" -c Release -o /app --no-restore --runtime linux-musl-x64 --self-contained true

# Serve Stage
FROM mcr.microsoft.com/dotnet/runtime-deps:7.0-alpine AS run
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
ENV ASPNETCORE_URLS="http://+:80;https://+:443"
ENV ASPNETCORE_HTTPS_PORT=443
ENV ASPNETCORE_HTTP_PORT=80
ENV ASPNETCORE_Kestrel__Certificates__Default__Path="/https/certificate.pfx"

ARG PFX_FILE=certificate.pfx
ARG ASPNETCORE_Kestrel__Certificates__Default__Password
ENV ASPNETCORE_Kestrel__Certificates__Default__Password=$ASPNETCORE_Kestrel__Certificates__Default__Password

RUN apk upgrade musl
RUN apk add icu-dev
RUN apk add musl-dev
RUN apk add libgdiplus

RUN adduser --disabled-password \
  --home /app \
  --gecos '' dotnetuser && chown -R dotnetuser /app
USER dotnetuser

WORKDIR /app
COPY --from=build /app .
COPY --from=build /source/src/Images /app/Images
COPY $PFX_FILE /https/
EXPOSE 80
EXPOSE 443
ENTRYPOINT ["./Avatarize"]
