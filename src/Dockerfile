
FROM microsoft/aspnetcore:1.0.1
MAINTAINER Shayne Boyer
LABEL Name=downr Version=0.0.1 
ARG source=publish
WORKDIR /app
COPY $source .
ENTRYPOINT ["dotnet","src.dll"]
