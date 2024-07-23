# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:8.0-jammy-arm64v8 AS build
WORKDIR /source
COPY . .

# Restore dependencies
RUN dotnet restore "./src/Avatarize.csproj" --disable-parallel --runtime linux-arm64

# Publish the application
RUN dotnet publish "./src/Avatarize.csproj" -c Release -o /app --no-restore --runtime linux-arm64 --self-contained true

# Create and export certificates
RUN apk add --no-cache openssl && \
    mkdir /tmp/certs && \
    openssl req -x509 -newkey rsa:4096 -keyout /tmp/certs/certificate.key -out /tmp/certs/certificate.crt -days 365 -nodes -subj "/CN=localhost" && \
    openssl pkcs12 -export -out /tmp/certs/certificate.pfx -inkey /tmp/certs/certificate.key -in /tmp/certs/certificate.crt -passout pass:password

# Serve Stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0-jammy-arm64v8 AS run
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
ENV ASPNETCORE_URLS="http://+:50002;https://+:50003"
ENV ASPNETCORE_HTTPS_PORT=50003
ENV ASPNETCORE_HTTP_PORT=50002

# Install necessary packages
RUN apk upgrade --no-cache && apk add --no-cache icu-libs libgdiplus

# Create a non-root user
RUN adduser -D -h /app dotnetuser && chown -R dotnetuser /app

WORKDIR /app
COPY --from=build /app .
COPY --from=build /source/src/Images /app/Images
COPY --from=build /tmp/certs /app/certs
RUN chmod 644 /app/certs/certificate.pfx
USER dotnetuser

EXPOSE 50002 50003

# Set environment variables for the certificate
ENV ASPNETCORE_Kestrel__Certificates__Default__Password="password"
ENV ASPNETCORE_Kestrel__Certificates__Default__Path="/app/certs/certificate.pfx"

ENTRYPOINT ["./Avatarize"]