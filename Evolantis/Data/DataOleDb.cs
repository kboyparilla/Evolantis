using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Data;
using System.Data.OleDb;
using System.Runtime.CompilerServices;

namespace Evolantis.Data.OleDb
{
    public class Database
    {   
        private OleDbCommand m_command;
        private OleDbConnection m_conn;
        private OleDbDataReader m_reader;

        public IDataReader Reader
        {
            get
            {
                return (IDataReader)this.m_reader;
            }
        }

        public OleDbCommand Command
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
            this.m_conn = new OleDbConnection();
            this.m_command = new OleDbCommand();
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
            catch (OleDbException ex)
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
            catch (OleDbException ex)
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
            catch (OleDbException ex)
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
            catch (OleDbException ex)
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

                OleDbDataAdapter adapter = new OleDbDataAdapter(this.m_command);
                ds = new DataSet();

                adapter.TableMappings.Add("Table", "ds");
                adapter.Fill(ds);
                adapter.Dispose();

                return ds;
            }
            catch (OleDbException ex)
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
        
    }
}
