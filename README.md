# Evolantis-Library

Connection String

public class Connection : IDatabaseSettings
{
    public string ConnectionString
    {
        get { return ConfigurationManager.ConnectionStrings["connection"].ToString(); }
    }
}

To Call The Connection String Class

Database db = new Database(new Connection());
