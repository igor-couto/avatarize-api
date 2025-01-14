# ===========================
# Build Stage
# ===========================
FROM mcr.microsoft.com/dotnet/sdk:9.0-bookworm-slim-arm64v8 AS build

WORKDIR /source
RUN apt-get update && \
    apt-get install -y --no-install-recommends \
        libgdiplus \
        git \
        && rm -rf /var/lib/apt/lists/*
COPY . .
RUN dotnet restore "./src/Avatarize.csproj" --disable-parallel --runtime linux-arm64
RUN dotnet publish "./src/Avatarize.csproj" \
    -c Release \
    -o /app \
    --no-restore \
    --runtime linux-arm64 \
    --self-contained true

# ===========================
# Runtime Stage
# ===========================
FROM mcr.microsoft.com/dotnet/aspnet:9.0-bookworm-slim-arm64v8 AS run

RUN apt-get update && \
    apt-get install -y --no-install-recommends \
        libgdiplus \
        && rm -rf /var/lib/apt/lists/*
ENV ASPNETCORE_URLS="http://+:50002"
ENV ASPNETCORE_HTTP_PORT=50002
WORKDIR /app
COPY --from=build /app .
COPY --from=build /source/src/Images /app/Images
EXPOSE 50002

ENTRYPOINT ["./Avatarize"]