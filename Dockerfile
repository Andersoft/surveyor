FROM mcr.microsoft.com/dotnet/sdk:6.0 AS base

ENV NODE_VERSION 18.2.0
ENV NODE_DOWNLOAD_SHA 73d3f98e96e098587c2154dcaa82a6469a510e89a4881663dc4c86985acf245e
ENV NODE_DOWNLOAD_URL https://nodejs.org/dist/v$NODE_VERSION/node-v$NODE_VERSION-linux-x64.tar.gz

RUN wget "$NODE_DOWNLOAD_URL" -O nodejs.tar.gz \
	&& echo "$NODE_DOWNLOAD_SHA  nodejs.tar.gz" | sha256sum -c - \
	&& tar -xzf "nodejs.tar.gz" -C /usr/local --strip-components=1 \
	&& rm nodejs.tar.gz \
	&& ln -s /usr/local/bin/node /usr/local/bin/nodejs \
	&& curl -sL https://deb.nodesource.com/setup_16.x |  bash - \
	&& apt update \
	&& apt-get install -y nodejs

FROM base AS build
WORKDIR /app
ARG project

COPY ./ .
RUN dotnet restore
RUN dotnet build -c Release --no-restore 

FROM build AS test
RUN dotnet test -c Release --no-build --logger trx

# Known issue if multiple tests projects complete within the same time down to
# millisecond difference then the test file will not be uniquely named and will 
# be overwritten by preceding files with the same name. 

FROM scratch AS test-results
COPY --from=test /app/tests/**/TestResults/*.trx .

FROM build as publish
RUN dotnet publish "./src/$project" -c Release --no-build -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
ENV APP_NAME "${project}.dll"
EXPOSE 80
EXPOSE 443
COPY --from=publish /app/publish .
ENTRYPOINT dotnet "$APP_NAME"