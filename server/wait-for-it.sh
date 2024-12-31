#!/bin/bash
# wait-for-it.sh

host=$1
shift
cmd="$@"

until /opt/mssql-tools/bin/sqlcmd -S $host -U sa -P "$SA_PASSWORD" -Q "SELECT 1" > /dev/null 2>&1; do
  echo "Waiting for SQL Server to be ready..."
  sleep 5
done

echo "SQL Server is ready"
exec $cmd
