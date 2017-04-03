dotnet restore
cd src
npm install
bower install
grunt precompile
dotnet build
grunt postpublish
dotnet bundle