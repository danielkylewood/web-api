#!/bin/bash

# Start SQL Server
/opt/mssql/bin/sqlservr &

# Start the script to create the DB and user
/usr/src/app/script-database.sh

# Call extra command
eval $1