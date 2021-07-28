# Visual Studio Optimized Dockerfile

# Base image to run the app
FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Builder image to compile and build the project
FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build

# Install necessary packages
RUN dotnet tool install -g Microsoft.Web.LibraryManager.Cli
RUN dotnet tool install -g Excubo.WebCompiler

ENV PATH="${PATH}:/root/.dotnet/tools"

# Change work directory
WORKDIR /src

# Copy project source
COPY . .

# Restore package dependencies
RUN dotnet restore ./Venjix.csproj
RUN libman restore
RUN webcompiler wwwroot/scss/venjix.scss -o wwwroot/css/ -z disable

# Compile and build the project
RUN dotnet build Venjix.csproj -c Release -o /app/build

# Publish project
FROM build AS publish
RUN dotnet publish Venjix.csproj -c Release -o /app/publish

# Build final image and run the app
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Venjix.dll"]