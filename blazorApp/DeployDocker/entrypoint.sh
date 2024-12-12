#!/bin/bash

# Démarrer SQL Server en arrière-plan
/opt/mssql/bin/sqlservr &

# Attendre que SQL Server soit prêt
echo "Waiting for SQL Server to start..."
sleep 15

# Exécuter le script d'initialisation
echo "Running initialization script toto..."
/opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P "$SA_PASSWORD" -i /scripts/initSql.sql

# Maintenir le conteneur actif
wait