#!/bin/bash
echo "Starting SQL Server and initializing database..."
/opt/mssql/bin/sqlservr & 
sleep 30  
/opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P "$SA_PASSWORD" -i /docker-entrypoint-initdb.d/init-db.sql
wait 
