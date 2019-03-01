using System.Data.OleDb;

namespace Evolantis.Data.OleDb
{
    public static class OleDbCommandExtensions
    {
        public static OleDbCommand AddParameter(this OleDbCommand command, string parameterName, object value)
        {
            command.Parameters.AddWithValue(parameterName, value);
            return command;
        }
    }
}
