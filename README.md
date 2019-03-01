Connection String
<pre>
public class Connection : IDatabaseSettings
{
    public string ConnectionString
    {
        get { return ConfigurationManager.ConnectionStrings["connection"].ToString(); }
    }
}
</pre>

To Call The Connection String Class
<pre>
Database db = new Database(new Connection());
</pre>
