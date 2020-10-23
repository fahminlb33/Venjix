# Venjix ![.NET Core](https://github.com/fahminlb33/Venjix/workflows/.NET%20Core/badge.svg)

Venjix is an open source web app to collect, visualize, and forecast data from 
IOT devices, acting as a server hub for centralized IOT data collection.
You can think this app is like ThingSpeak but hosed in your local server.


## Features

1. Built using ASP.NET Core MVC (.NET 3.1).
2. SQLite database.
3. ML.NET integration to run data forecasting.
4. Basic table and chart visualization and map visualization.
5. Export/import collected data.
6. Two user level (admin/user).
7. Telegram Bot integration based on event trigger.
8. Webhooks based on event trigger.

## Deploying

Deploying is easy, you can deploy this app to run in your local computer,
inside a docker container, or into a Raspberry Pi!

### Docker

1. Clone this repo
2. `docker build -t venjix:latest`
3. `docker run venjix:latest`

### Locally

Make sure you have .NET Core 3.1 SDK installed to run this app.

1. Clone this repo.
2. `dotnet restore`
3. `dotnet run`

### Raspberry Pi

Will be updated!

## Contributing

Contribution is very welcome! Create an issue or pull request to help imrpove this
project.

What tools I'm using:

1. Visual Studio 2019.
2. Web Compiler (extension).
3. Libman (included in VS2019).
4. Docker.
