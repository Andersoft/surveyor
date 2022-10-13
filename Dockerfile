FROM mcr.microsoft.com/dotnet/aspnet:6.0

RUN mkdir /app
WORKDIR /app

ARG image_folder
ARG assembly_name

COPY ${image_folder}/. /app
WORKDIR /app

ENV _exe="$assembly_name.dll"

ENTRYPOINT exec dotnet "$_exe"
