# Étape 1: Utilisation de l'image .NET SDK pour la construction de l'application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Définir le répertoire de travail dans le conteneur
WORKDIR /app

# Copier le fichier .csproj et restaurer les dépendances
COPY ../blazorApp.csproj ./
RUN dotnet restore

# Copier tout le code source dans le conteneur
COPY ../ ./

# Publier l'application en mode Release dans le répertoire /app/publish
RUN dotnet publish -c Release -o /app/publish

# Étape 2 : Runtime pour exécuter l'application
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copie les fichiers compilés depuis l'étape de build
COPY --from=build /app/publish .

# Expose le port utilisé par Kestrel
EXPOSE 5000

# Définit le point d'entrée pour démarrer l'application
ENTRYPOINT ["dotnet", "blazorApp.dll"]