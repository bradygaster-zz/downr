# downr
dirt-simple markdown blog tool built using asp.net core

## requirements
1. .net core 1.0 or higher
1. node.js
1. grunt cli

## setup
1. clone the repository
1. navigate to the root in your terminal and execute these commands

        npm install        
        dotnet restore
        bower install
        grunt
        dotnet build

## debugging
here are two ways to get downr running on your local workstation. 

1. if have [visual studio code](http://code.visualstudio.com) installed, type the commands below to open the project using Visual Studio Code and just start debugging via f5 (or whatever your shortcut is)

        cd ..
        code .

1. to run the site locally just type the folllowing command in your terminal:

        dotnet run

    use your favorite browser to load up [http://localhost:5000/posts/hello-downr](http://localhost:5000/posts/hello-downr)

## using downr (to be expanded)
take a look at the site's folder structure with the sample blog posts and start writing

### stay tuned
downr is a brand new project that has only a few basic features. your contributions and ideas are welcome. 