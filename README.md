# DockerTestCSharp

#### BlazorApp ####

  # Documentation / Tuto
  
  https://learn.microsoft.com/fr-fr/training/browse/?products=blazor&expanded=dotnet

  https://github.com/dotnet-presentations/blazor-workshop/
 
  https://dotnet.microsoft.com/fr-fr/learn/aspnet/blazor-tutorial/intro

dotnet run --environment Staging

  # lister les sdk dotnet
  dotnet new list
  dotnet --list-sdks

  # entity framework
  dotnet add package Microsoft.EntityFrameworkCore.SqlServer

  # créer un nouveau composant Blazor en ligne de commande
  dotnet new razorcomponent -n Todo -o Components/Pages

  # approuver le certificat 
  dotnet dev-certs https
  dotnet dev-certs https --trust

  # Raccourci fenetre de Debugage
  ctrl + shift + D

  # Publish realease 
  dotnet publish -c Realease

  # Code generator
  dotnet tool install --global dotnet-aspnet-codegenerator
  https://github.com/microsoft/vscode-dev-containers/issues/1625


# Retour Chariot encodage
Using Visual Studio Code: If you’re using Visual Studio Code:

Open installSQLtools.sh.
In the bottom-right corner, you should see either CRLF or LF. Click on it.
Select LF (Unix format) to change the line endings.
Save the file.

git config --global core.autocrlf false

#### Docker ####

# Documentation / Tuto
https://www.youtube.com/watch?v=0v3JXxuyF7Y

# volume mapp sous windows
\\wsl.localhost\docker-desktop\mnt\docker-desktop-disk\data\docker\volumes\
\\wsl.localhost\docker-desktop\mnt\docker-desktop-disk\data\docker\volumes\DockerTestCSharp-cab16d7d7fbfe764c9b4643aa2dc40298734549aac415fecc7cae82c6d4b126d\_data\DockerTestCSharp
# commande Docker
  //dans le dossier du dockerFile
  docker build -t monimagedocker

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

  //Docker compose
  validez votre configuration :
  docker compose -f /workspaces/DockerTestCSharp/.devcontainer/docker-compose.yml config

  //Docker network +  inspecter les réseaux
  docker network ls
  docker inspect {NOM_DU_RESEAU}
  docker inspect {NOM_DU_RESEAU} | more
  
  // build conteneur
  docker compose up -d --build
  docker compose up --build
  docker compose --env-file secret.env build
  docker compose --env-file secret.env build --progress=plain --no-cache
docker-compose up -d --force-recreate
docker compose build --no-cache && docker compose up
  docker-compose build --progress=plain

  // exécuter un bash sur un conteneur distant 
  docker exec -it c8e754d9db75 bash
  docker exec -it --user root 9582dbfbc941 bash


//volume 
 docker volume ls
 docker inspect deploydockerwithnginx_nginx

#### Docker dev environement ####

--> devcontainer.json
The Visual Studio Code Dev Containers extension lets you use a Docker container as a full-featured development environment. It allows you to open any folder or repository inside a container and take advantage of Visual Studio Code's full feature set. A devcontainer.json file in your project tells VS Code how to access (or create) a development container with a well-defined tool and runtime stack. This container can be used to run an application or to provide separate tools, libraries, or runtimes needed for working with a codebase.

#### Nginx ####
//Host ASP.NET Core on Linux with Nginx
https://learn.microsoft.com/en-us/aspnet/core/host-and-deploy/linux-nginx?view=aspnetcore-9.0&tabs=linux-ubuntu

/usr/share/nginx/html
\\wsl.localhost\docker-desktop\mnt\host\wsl\docker-desktop\shared-sockets\guest-services

#### serveur SQL ODBC ####
af06eea7c11d

Docker exec -it af06eea7c11d bash
/opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P P@ssw0rd -c

Docker exec -it <nom_du_conteneur> bash
/opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P P@ssw0rd -C -d PizzaDatabase

docker exec -it <db_container_name> /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P P@ssw0rd -C

#test du serveur sql 
docker run -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=P@ssw0rd' -p 1433:1433 --name test-mssql -d mcr.microsoft.com/mssql/server:2019-latest
docker stop test-mssql
docker stop test-mssql

EXEC sp_configure 'remote access', 1;
RECONFIGURE;
GO

faire la connection a partir de la fenetre et pas de l invite en haut de visual studio code
#serveur SQL ODBC Driver 18

https://learn.microsoft.com/fr-fr/sql/linux/quickstart-install-connect-docker?view=sql-server-ver16&tabs=cli&pivots=cs1-bash


#### gestion des mots de passe ####
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

##### ping ####
apt-get update && apt-get install -y iputils-ping

##### nginx test
docker run -d  -p 8880:80 nginx:alpine
Tester la configuration de Nginx : Assurez-vous que la configuration de Nginx est correcte avec la commande suivante :

bash
Copier le code
nginx -t
Ensuite, redémarrez Nginx pour appliquer les nouvelles configurations :

bash
Copier le code
systemctl restart nginx

##### nginx test load balancing
###fichier host windows
C:\Windows\System32\drivers\etc\hosts
127.0.0.1 blazorPizzza1.com
127.0.0.1 blazorPizzza2.com

##Test
1) modifier le css d'un des deux site déployé pour les différencier 
/app/wwwroot/css/site.css
exemple : 
/*app1*/ 
.top-bar {
    height: 5rem;
    background-color: rgb(192,0,0); 
    ....
}
/*app2*/ 
.top-bar {
    height: 5rem;
    background-color: rgb(0, 19, 192); 
    ....
}

2) utiliser le ctrl + F5 pour vérifier que les requetes sont bien réparties sur les deux instances app1 et app2 

##Résolution erreur 
Error: Unable to connect to the server with any of the available transports. Error: WebSockets failed: Error: WebSocket failed to connect. 
The connection could not be found on the server, either the endpoint may not be a SignalR endpoint, the connection ID is not present on the server, 
or there is a proxy blocking WebSockets. If you have multiple servers check that sticky sessions are enabled. ServerSentEvents failed: 
Error: 'ServerSentEvents' does not support Binary. Error: LongPolling failed: Error: No Connection with that ID: Status code '404'

Pourquoi cette erreur se produit ?
SignalR nécessite des connexions persistantes :
Lorsque Nginx redirige les requêtes de manière aléatoire (round-robin), il peut rediriger une requête SignalR vers un autre backend. Si la connexion SignalR initiale n'est plus disponible sur le backend cible, vous recevez une erreur 404.

Transports multiples échouent :
L'erreur indique que WebSockets, ServerSentEvents, et LongPolling ont tous échoué à établir une connexion stable avec le serveur.


upstream blazor_backend {
    ip_hash; # Associe une IP à un backend spécifique
    server app1:8080;
    server app2:8080;
}

##Résolution erreur autre solution 
Optionnel : Utiliser des Sticky Sessions Basées sur des Cookies
Si votre application a des utilisateurs derrière des NAT ou des proxys partagés (qui pourraient partager la même IP), vous pouvez configurer des sticky sessions basées sur des cookies au lieu des adresses IP :

upstream blazor_backend {
    server app1:8080;
    server app2:8080;
    sticky cookie srv_id expires=1h domain=blazorPizzza.com path=/;
}
Avec ces modifications, votre application Blazor utilisant SignalR devrait fonctionner correctement avec le load balancing derrière Nginx. Si vous rencontrez toujours des problèmes, faites un retour avec des détails supplémentaires !