FROM mcr.microsoft.com/dotnet/aspnet:6.0

RUN mkdir /app
WORKDIR /app

ARG assembly_name

COPY artifacts/publish/${assembly_name}/. /app
WORKDIR /app

ENV _exe="$assembly_name.dll"

ENTRYPOINT exec dotnet "$_exe"
