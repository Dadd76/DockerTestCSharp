# DockerTestCSharp

# BlazorApp

https://learn.microsoft.com/fr-fr/training/browse/?products=blazor&expanded=dotnet

https://github.com/dotnet-presentations/blazor-workshop/
dotnet new list
dotnet --list-sdks

https://dotnet.microsoft.com/fr-fr/learn/aspnet/blazor-tutorial/intro

dotnet new razorcomponent -n Todo -o Components/Pages

//approuver le certificat 
dotnet dev-certs https
dotnet dev-certs https --trust

ctrl + shift + D

dotnet tool install --global dotnet-aspnet-codegenerator
https://github.com/microsoft/vscode-dev-containers/issues/1625


Using Visual Studio Code: If you’re using Visual Studio Code:

Open installSQLtools.sh.
In the bottom-right corner, you should see either CRLF or LF. Click on it.
Select LF (Unix format) to change the line endings.
Save the file.


# Docker
https://www.youtube.com/watch?v=0v3JXxuyF7Y

//dans le dossier du dockerFile
docker build -t moncimagedocker

//lister les images 
docker image ls

//lancer l image 
docker run monimagedocker

//sauvegarder 
docker save monconteneurdocker - monconteneurdocker.tar
--> docker Hub

//lancer l image en exposant un port (redirection 99<-)80
docker run -p 99:80 --name  monconteneurdocker monimagedocker

//retrouver l id de son contener en cours d exécution 
docker ps 

//lancer un bash sur le conteneur 
docker exec -it c8e754d9db75 bash

docker inspect c8e754d9db75 --> voir la config 
docker log c8e754d9db75 --> voir les logs
docker stop c8e754d9db75 ---> arréter le conteneur
docker start c8e754d9db75 ---> démarrer le conteneur 

//Host ASP.NET Core on Linux with Nginx
https://learn.microsoft.com/en-us/aspnet/core/host-and-deploy/linux-nginx?view=aspnetcore-9.0&tabs=linux-ubuntu


//Docker compose



