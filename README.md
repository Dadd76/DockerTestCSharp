# DockerTestCSharp

--> devcontainer.json
The Visual Studio Code Dev Containers extension lets you use a Docker container as a full-featured development environment. It allows you to open any folder or repository inside a container and take advantage of Visual Studio Code's full feature set. A devcontainer.json file in your project tells VS Code how to access (or create) a development container with a well-defined tool and runtime stack. This container can be used to run an application or to provide separate tools, libraries, or runtimes needed for working with a codebase.


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


\\wsl.localhost\docker-desktop\mnt\docker-desktop-disk\data\docker\volumes\

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


#serveur SQL ODBC Driver 18
/opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P P@ssw0rd -C



//
dotnet add package Microsoft.EntityFrameworkCore.SqlServer


1. Comment cela fonctionne ?
Lorsque .NET charge la configuration, il combine les différentes sources de configuration dans l’ordre suivant (par défaut) :

appsettings.json : valeurs définies dans le fichier.
appsettings.{Environment}.json : valeurs spécifiques à un environnement (par exemple, appsettings.Production.json).
Variables d’environnement : elles remplacent les clés correspondantes des fichiers JSON.
Arguments en ligne de commande : ils ont la priorité absolue.
Ainsi, la chaîne de connexion définie dans ConnectionStrings__DefaultConnection via les variables d’environnement dans docker-compose.yml écrasera la valeur correspondante dans appsettings.json, si les clés sont identiques.

2. Vérification de la clé utilisée
Dans ton fichier appsettings.json, la clé pour la chaîne de connexion est probablement définie ainsi :

json
Copier le code
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=PizzaDatabase;Trusted_Connection=True;MultipleActiveResultSets=true"
  }
}
Dans ton docker-compose.yml, tu utilises la clé ConnectionStrings__DefaultConnection. Cette syntaxe avec __ (double underscore) est interprétée par .NET pour cibler des sous-sections de configuration. Ainsi, ta configuration sera remplacée correctement.

3. Configuration dans le code
Pour t’assurer que ta configuration utilise bien les variables d’environnement, ton application doit inclure la méthode AddEnvironmentVariables dans le pipeline de configuration, ce qui est souvent le cas par défaut dans les applications .NET modernes.

Dans Program.cs (ou Startup.cs si tu utilises une version plus ancienne), vérifie que cela est présent :

csharp
Copier le code
var builder = WebApplication.CreateBuilder(args);

// Ajoute automatiquement les variables d'environnement à la configuration
builder.Configuration.AddEnvironmentVariables();

var app = builder.Build();
4. Comment vérifier que cela fonctionne ?
Tu peux vérifier que la chaîne de connexion finale utilisée par ton application est celle définie dans les variables d’environnement.

Ajoute un log dans Program.cs pour inspecter la chaîne de connexion chargée :

csharp
Copier le code
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
Console.WriteLine($"Final connection string: {connectionString}");
Lorsque tu démarres le conteneur, la sortie devrait afficher la chaîne définie dans docker-compose.yml.

5. Précautions
Ne laisse pas de mots de passe en clair dans appsettings.json. Si quelqu’un l’examine, il pourrait obtenir des informations sensibles.
Si tu utilises une chaîne de connexion avec un placeholder pour le mot de passe dans appsettings.json, comme ceci :
json
Copier le code
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Database=PizzaDatabase;User=sa;Password=;Encrypt=True;TrustServerCertificate=True;"
  }
}
La variable d’environnement remplacera entièrement cette valeur au moment de l’exécution.
6. Résumé
Oui, ta chaîne définie dans docker-compose.yml remplacera celle d’appsettings.json si tout est configuré correctement.
Ajoute AddEnvironmentVariables dans ton code si ce n’est pas déjà fait.
Vérifie avec un log ou un débogueur que la configuration finale utilisée est correcte.
Cette approche garantit que ta chaîne de connexion est sécurisée et configurable pour différents environnements sans modifier le code ou les fichiers sensibles.



