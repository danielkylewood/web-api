FROM microsoft/mssql-server-linux:2017-latest
RUN apt-get update && apt-get install -y dos2unix
# Create app directory
RUN mkdir -p /usr/src/app
WORKDIR /usr/src/app
# Bundle app source
COPY . /usr/src/app
ADD entry-point.sh /usr/src/app/
ADD script-database.sh /usr/src/app/
# Grant permissions for the import-data script to be executable
RUN dos2unix /usr/src/app/entry-point.sh
RUN dos2unix /usr/src/app/script-database.sh
RUN chmod +x /usr/src/app/entry-point.sh
RUN chmod +x /usr/src/app/script-database.sh
EXPOSE 1433
ENTRYPOINT ["/usr/src/app/entry-point.sh"]
# Tail the setup logs to trap the process
CMD ["tail -f /dev/null"]
#HEALTHCHECK --interval=15s CMD /opt/mssql-tools/bin/sqlcmd -U sa -P $SA_PASSWORD -Q "select 1" && grep -q "MSSQL CONFIG COMPLETE" ./config.log