namespace WebApiTemplate.Domain.Configuration
{
    public class DatabaseOptions
    {
        public string DatabaseConnectionString { get; set; }

        public DatabaseOptions()
        {

        }

        public DatabaseOptions(string databaseConnectionString)
        {
            DatabaseConnectionString = databaseConnectionString;
        }
    }
}
