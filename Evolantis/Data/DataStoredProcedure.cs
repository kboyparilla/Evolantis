using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;

namespace Evolantis.Data.StoredProcedure
{
    public class Database
    {
        private SqlCommand m_command;
        private SqlConnection m_conn;
        private SqlDataReader m_reader;

        public IDataReader Reader
        {
            get
            {
                return (IDataReader)this.m_reader;
            }
        }

        public SqlCommand Command
        {
            get
            {
                return this.m_command;
            }
        }

        public Database(IDatabaseSettings loc)
        {
            if (Microsoft.VisualBasic.CompilerServices.Operators.CompareString(loc.ConnectionString, (string)null, false) == 0 | loc.ConnectionString.Length == 0)
                throw new ApplicationException("Connection String was empty");
            this.m_conn = new SqlConnection();
            this.m_command = new SqlCommand();
            this.m_conn.ConnectionString = loc.ConnectionString;
        }

        public void Refresh()
        {
            if (this.m_reader != null)
                this.m_reader.Close();
            this.m_command.Dispose();
            this.m_command = new SqlCommand();
        }

        public void RunFile(string filepath)
        {
            this.CheckConnection();
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.Connection = this.m_conn;
            string[] strArray = new Regex("^GO", RegexOptions.IgnoreCase | RegexOptions.Multiline).Split(new StreamReader(filepath).ReadToEnd());
            int index = 0;
            while (index < strArray.Length)
            {
                string Left = strArray[index];
                if (Microsoft.VisualBasic.CompilerServices.Operators.CompareString(Left, string.Empty, false) != 0)
                {
                    sqlCommand.CommandText = Left;
                    sqlCommand.ExecuteNonQuery();
                }
                checked { ++index; }
            }
        }

        public void ExecuteReaderProcedure(string proc)
        {
            this.CheckConnection();
            this.m_command.CommandText = proc;
            this.m_command.Connection = this.m_conn;
            this.m_command.CommandType = CommandType.StoredProcedure;
            try
            {
                this.m_reader = this.m_command.ExecuteReader();
            }
            catch (SqlException ex)
            {
                ProjectData.SetProjectError((Exception)ex);
                StringBuilder stringBuilder = new StringBuilder(ex.Message);
                stringBuilder.Append(proc + "---");
                try
                {
                    foreach (SqlParameter parameter in this.m_command.Parameters)
                        stringBuilder.Append(Microsoft.VisualBasic.CompilerServices.Operators.ConcatenateObject(Microsoft.VisualBasic.CompilerServices.Operators.ConcatenateObject((object)(parameter.ParameterName + ":"), parameter.Value), (object)"---"));
                }
                finally
                {
                    //IEnumerator enumerator;
                    //if (enumerator is IDisposable)
                    //    (enumerator as IDisposable).Dispose();
                }
                throw new ApplicationException(stringBuilder.ToString());
            }
        }

        public int ExecuteNonQueryProcedure(string proc)
        {
            this.CheckConnection();
            this.m_command.CommandText = proc;
            this.m_command.Connection = this.m_conn;
            this.m_command.CommandType = CommandType.StoredProcedure;
            try
            {
                return this.m_command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                ProjectData.SetProjectError(ex);
                StringBuilder stringBuilder = new StringBuilder(ex.Message);
                stringBuilder.Append(proc + "---");
                try
                {
                    foreach (SqlParameter parameter in this.m_command.Parameters)
                        stringBuilder.Append(Microsoft.VisualBasic.CompilerServices.Operators.ConcatenateObject(Microsoft.VisualBasic.CompilerServices.Operators.ConcatenateObject((object)(parameter.ParameterName + ":"), parameter.Value), (object)"---"));
                }
                finally
                {
                    //IEnumerator enumerator;
                    //if (enumerator is IDisposable)
                    //    (enumerator as IDisposable).Dispose();
                }
                throw new ApplicationException(stringBuilder.ToString());
            }
        }

        public object ExecuteScalarProcedure(string proc)
        {
            this.CheckConnection();
            this.m_command.CommandText = proc;
            this.m_command.Connection = this.m_conn;
            this.m_command.CommandType = CommandType.StoredProcedure;
            try
            {
                return RuntimeHelpers.GetObjectValue(this.m_command.ExecuteScalar());
            }
            catch (SqlException ex)
            {
                ProjectData.SetProjectError((Exception)ex);
                StringBuilder stringBuilder = new StringBuilder(ex.Message);
                stringBuilder.Append(proc + "---");
                try
                {
                    foreach (SqlParameter parameter in this.m_command.Parameters)
                        stringBuilder.Append(Microsoft.VisualBasic.CompilerServices.Operators.ConcatenateObject(Microsoft.VisualBasic.CompilerServices.Operators.ConcatenateObject((object)(parameter.ParameterName + ":"), parameter.Value), (object)"---"));
                }
                finally
                {
                    //IEnumerator enumerator;
                    //if (enumerator is IDisposable)
                    //    (enumerator as IDisposable).Dispose();
                }
                throw new ApplicationException(stringBuilder.ToString());
            }
        }

        protected void CheckConnection()
        {
            if (this.m_conn.State != ConnectionState.Closed)
                return;
            this.m_conn.Open();
        }

        public void Close()
        {
            if (this.m_reader != null)
                this.m_reader.Close();
            if (this.m_command != null)
                this.m_command.Dispose();
            if (this.m_conn != null)
                this.m_conn.Close();
            GC.SuppressFinalize((object)this);
        }

        public DataTable GetDataTable()
        {
            return this.GetDataSet().Tables[0];
        }

        public DataRow GetDataRow()
        {
            return this.GetDataTable().Rows[0];
        }

        public DataSet GetDataSet()
        {
            if (this.m_reader == null)
                throw new ApplicationException("The Data Reader Is Not Filled");
            DataSet dataSet = new DataSet();
            do
            {
                DataTable schemaTable = this.m_reader.GetSchemaTable();
                DataTable table = new DataTable();
                if (schemaTable != null)
                {
                    int num1 = 0;
                    int num2 = checked(schemaTable.Rows.Count - 1);
                    int index = num1;
                    while (index <= num2)
                    {
                        DataRow row = schemaTable.Rows[index];
                        DataColumn column = new DataColumn(Conversions.ToString(row["ColumnName"]), (Type)row["DataType"]);
                        table.Columns.Add(column);
                        checked { ++index; }
                    }
                    dataSet.Tables.Add(table);
                    while (this.m_reader.Read())
                    {
                        DataRow row = table.NewRow();
                        int num3 = 0;
                        int num4 = checked(this.m_reader.FieldCount - 1);
                        int i = num3;
                        while (i <= num4)
                        {
                            row[i] = RuntimeHelpers.GetObjectValue(this.m_reader.GetValue(i));
                            checked { ++i; }
                        }
                        table.Rows.Add(row);
                    }
                }
                else
                {
                    DataColumn column = new DataColumn("Rows Affected");
                    table.Columns.Add(column);
                    dataSet.Tables.Add(table);
                    DataRow row = table.NewRow();
                    row[0] = (object)this.m_reader.RecordsAffected;
                    table.Rows.Add(row);
                }
            }
            while (this.m_reader.NextResult());
            return dataSet;
        }

    }
}
