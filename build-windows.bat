dotnet restore
cd src
call npm install
call bower install
call grunt precompile
dotnet build
call grunt postpublish
dotnet bundle
cd ..