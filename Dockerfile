FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
ARG project_name
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
ARG project_folder

COPY ./src .
ENV NODE_VERSION 16.9.1
ENV NODE_DOWNLOAD_SHA 1d48c69e4141792f314d29f081501dc22218cfc22f9992c098f7e3f5e0531139
ENV NODE_DOWNLOAD_URL https://nodejs.org/dist/v$NODE_VERSION/node-v$NODE_VERSION-linux-x64.tar.gz

RUN wget "$NODE_DOWNLOAD_URL" -O nodejs.tar.gz \
	&& echo "$NODE_DOWNLOAD_SHA  nodejs.tar.gz" | sha256sum -c - \
	&& tar -xzf "nodejs.tar.gz" -C /usr/local --strip-components=1 \
	&& rm nodejs.tar.gz \
	&& ln -s /usr/local/bin/node /usr/local/bin/nodejs \
	&& curl -sL https://deb.nodesource.com/setup_16.x |  bash - \
	&& apt update \
	&& apt-get install -y nodejs

RUN dotnet restore "$project_folder"

WORKDIR "/src/$project_folder"
RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
ENV APP_NAME "${project_name}.dll"
EXPOSE 80
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT dotnet "$APP_NAME"