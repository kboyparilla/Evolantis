using System.Data.SqlClient;

namespace Evolantis.Data.SqlClient
{
    public static class SqlCommandExtensions
    {
        public static SqlCommand AddParameter(this SqlCommand command, string parameterName, object value)
        {
            command.Parameters.AddWithValue(parameterName, value);
            return command;
        }
    }
}
