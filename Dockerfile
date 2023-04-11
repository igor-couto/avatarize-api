# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /source
COPY . .
RUN dotnet restore "./src/Avatarize.csproj" --disable-parallel
RUN dotnet publish "./src/Avatarize.csproj" -c Release -o /app --no-restore --self-contained true

# Serve Stage
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS run
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
ENV ASPNETCORE_URLS="http://+:80;https://+:443"
ENV ASPNETCORE_HTTPS_PORT=443
ENV ASPNETCORE_HTTP_PORT=80
ENV ASPNETCORE_Kestrel__Certificates__Default__Path="/https/certificate.pfx"

ARG PFX_FILE=certificate.pfx
ARG ASPNETCORE_Kestrel__Certificates__Default__Password
ENV ASPNETCORE_Kestrel__Certificates__Default__Password=$ASPNETCORE_Kestrel__Certificates__Default__Password

RUN apt-get install -y apt-utils
RUN apt-get install -y libgdiplus
RUN apt-get install -y libc6-dev
RUN ln -s /usr/lib/libgdiplus.so/usr/lib/gdiplus.dll
WORKDIR /app
COPY --from=build /app .
COPY --from=build /source/src/Images /app/Images
COPY $PFX_FILE /https/
EXPOSE 80
EXPOSE 443
ENTRYPOINT ["dotnet", "Avatarize.dll"]