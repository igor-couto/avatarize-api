# Serve Stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine3.20-arm64v8 AS run

# Install runtime dependencies
RUN apk add --no-cache icu-libs

# Install build dependencies and libgdiplus
RUN apk add --no-cache --virtual .build-deps \
        build-base \
        autoconf \
        automake \
        libtool \
        pkgconfig \
        cairo-dev \
        pango-dev \
        giflib-dev \
        libjpeg-turbo-dev \
        libpng-dev \
        freetype-dev \
        libtiff-dev \
        gobject-introspection-dev \
        glib-dev \
        mono-libs \
        mono-dev \
        git \
        bash && \
    git clone https://github.com/mono/libgdiplus.git && \
    cd libgdiplus && \
    git checkout tags/6.0.0 && \
    ./autogen.sh && \
    make && \
    make install && \
    ldconfig && \
    # Clean up
    cd .. && \
    rm -rf libgdiplus && \
    apk del .build-deps

ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
ENV ASPNETCORE_URLS="http://+:50002"
ENV ASPNETCORE_HTTP_PORT=50002

WORKDIR /app
COPY --from=build /app .
COPY --from=build /source/src/Images /app/Images

EXPOSE 50002

ENTRYPOINT ["./Avatarize"]