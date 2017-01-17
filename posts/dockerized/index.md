---
title: downr in a Container
slug: dockerized
author: Shayne Boyer
lastModified: 2017-01-17 12:00:00
pubDate: 2017-01-17 12:00:00
categories: docker, linux
---

## New build functionaility has been added with Docker.

Adding the container technology to downr allows for the build of the application to happen locally or in a CI/CD process such as TFS, Travis CI or CodeShip. The Dockerfile is simple.

```
FROM microsoft/aspnetcore:1.0.1
MAINTAINER Shayne Boyer
LABEL Name=downr Version=0.0.1 
ARG source=publish
WORKDIR /app
COPY $source .
ENTRYPOINT ["dotnet","src.dll"]
```

It uses the base `aspnetcore` image which sits on top of `debian:jessie`, an Ubunutu image. In the near future this will be furhter reduces to the `alpine` base image which is ~5 mb.

To build the Docker image, first publish the application to the `publish` folder.

```
dotnet publish -o publish -c Release
```

Build the Docker image

```
docker build -t <your-docker-hub-id>/downr
```

Push to docker hub. (Create the repository on Docker Hub first)
```
docker push <your-docker-hub-id>/downr
```

### Publishing to Azure App Service (Linux)

Create a new Azure App Service in the portal and configure the application to use the container from the Docker Hub from the **Docker Container** setting. 

Select **Docker** as the repository, and input the name of the image name you just published and tap **Save**.

The App Service will pull the image and start the application from the image.