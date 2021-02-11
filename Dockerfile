#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
RUN curl -sL https://deb.nodesource.com/setup_10.x |  bash -
RUN apt-get install -y nodejs
RUN npm install -g sass

WORKDIR /src
COPY ["Venjix.csproj", ""]
RUN dotnet restore "./Venjix.csproj"

COPY . .
WORKDIR "/src/."
RUN sass wwwroot/scss/sb-admin-2.scss wwwroot/css/sb-admin-2.css
RUN dotnet build "Venjix.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Venjix.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Venjix.dll"]