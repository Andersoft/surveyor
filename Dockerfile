FROM mcr.microsoft.com/dotnet/aspnet:6.0

RUN mkdir /app
WORKDIR /app

ARG assembly_name

COPY artifacts/images/${assembly_name}/. /app
WORKDIR /app

ENV _exe="$assembly_name.dll"

ENTRYPOINT exec dotnet "$_exe"

RUN dotnet publish "./src/$project" -c Release --no-build -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:6.0
ARG project
WORKDIR /app
ENV APP_NAME "${project}.dll"
EXPOSE 80
EXPOSE 443
COPY --from=publish /app/publish ./
ENTRYPOINT dotnet "$APP_NAME"
