using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;
using System.Text;

namespace Evolantis.Data.SqlClient
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

        public int ExecuteNonQuery(string query)
        {
            try
            {
                this.CheckConnection();
                this.m_command.CommandText = query;
                this.m_command.CommandType = CommandType.Text;
                this.m_command.Connection = this.m_conn;
                return m_command.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                ProjectData.SetProjectError((Exception)ex);
                throw new ApplicationException(ex.Message + "----" + query);
            }
        }

        public void ExecuteReader(string query)
        {
            try
            {
                this.CheckConnection();
                this.m_command.CommandText = query;
                this.m_command.CommandType = CommandType.Text;
                this.m_command.Connection = this.m_conn;
                this.m_reader = this.Command.ExecuteReader();
            }
            catch (SqlException ex)
            {
                ProjectData.SetProjectError((Exception)ex);
                throw new ApplicationException(ex.Message + "----" + query);
            }
        }

        public object ExecuteScalar(string query)
        {
            try
            {
                this.CheckConnection();
                this.m_command.CommandText = query;
                this.m_command.CommandType = CommandType.Text;
                this.m_command.Connection = this.m_conn;
                return RuntimeHelpers.GetObjectValue(this.m_command.ExecuteScalar());
            }
            catch (SqlException ex)
            {
                ProjectData.SetProjectError((Exception)ex);
                throw new ApplicationException(ex.Message + "----" + query);
            }
        }

        public object ExecuteScopeIdentity(string query)
        {
            string sqlScope = "Select @@Identity";
            try
            {
                this.CheckConnection();
                this.m_command.CommandText = query;
                this.m_command.CommandType = CommandType.Text;
                this.m_command.Connection = this.m_conn;
                this.m_command.ExecuteNonQuery();
                this.m_command.CommandText = sqlScope;
                return RuntimeHelpers.GetObjectValue(this.m_command.ExecuteScalar());
            }
            catch (SqlException ex)
            {
                ProjectData.SetProjectError((Exception)ex);
                throw new ApplicationException(ex.Message + "----" + query);
            }
        }


        public DataSet ExecuteDataSet(string query)
        {
            DataSet ds = null;
            try
            {
                this.CheckConnection();
                this.m_command.CommandText = query;
                this.m_command.CommandType = CommandType.Text;
                this.m_command.Connection = this.m_conn;

                SqlDataAdapter adapter = new SqlDataAdapter(this.m_command);
                ds = new DataSet();

                adapter.TableMappings.Add("Table", "ds");
                adapter.Fill(ds);
                adapter.Dispose();

                return ds;
            }
            catch (SqlException ex)
            {
                ProjectData.SetProjectError((Exception)ex);
                throw new ApplicationException(ex.Message + "----" + query);
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
                   
                }
                throw new ApplicationException(stringBuilder.ToString());
            }
        }


    }
}

