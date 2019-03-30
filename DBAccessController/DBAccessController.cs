using System;
using Oracle.ManagedDataAccess.Client;
using System.Data.SqlClient;
using System.Data;

namespace Controllers
{
    public class DBAccessController
    {
        static OracleConnection oraConnection = null;
        static SqlConnection sqlConnnection = null;
        static DataTable dataTable = null;

        static string[] oraConnectionString;
        static string[] sqlConnectionString;

        public DBAccessController()
        {
            if (Sistem.LoadConfigFlag)
            {
                createConnectionStrings();
                oraConnection = dbGetOracleConnection();
                sqlConnnection = dbGetSqlConnection();
            }
            else
                Sistem.WriteLog("Debe completar el archivo de configuracion Config.xml antes de iniciar el sistema.", "DBAccessController()", true, true);
        }

        private void createConnectionStrings()
        {
            string[] oraCon = {
                                string.Format("Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST={0})(PORT={1})))",Sistem.OraHost,Sistem.OraPort),
                                string.Format("Data Source={0};User Id={1};Password={2};",Sistem.OraHost,Sistem.OraUser,Sistem.OraPassword),
                                string.Format("Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST={0})(PORT={1})))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME={2})));User Id={3}; Password = {4};",Sistem.OraHost,Sistem.OraPort,Sistem.OraServicename,Sistem.OraUser,Sistem.OraPassword),
                                string.Format("Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST={0})(PORT={1}))(CONNECT_DATA=(SERVICE_NAME={2})));User Id={3}; Password = {4};",Sistem.OraHost,Sistem.OraPort,Sistem.OraServicename,Sistem.OraUser,Sistem.OraPassword)
            };

            string[] sqlCon = {
                                string.Format("Server={0};Database={1};User Id={2};Password={3};",Sistem.SqlHost,Sistem.SqlDatabase,Sistem.SqlUser,Sistem.SqlPassword),
                                string.Format("Data Source={0},{1};Network Library={2};Initial Catalog={3}; User ID = {4}; Password={5};",Sistem.SqlHost,Sistem.SqlPort,Sistem.SqlNetworkLibrary,Sistem.SqlDatabase,Sistem.SqlUser,Sistem.SqlPassword),
                                string.Format("Server=(localdb)\v11.0;Integrated Security=true;AttachDbFileName={0};", Sistem.SqlLocalDatabaseNamePath)
            };

            sqlConnectionString = sqlCon;
            oraConnectionString = oraCon;
        }

        public static SqlConnection dbGetSqlConnection()
        {
            if (sqlConnnection != null)
                if (sqlConnnection.State == System.Data.ConnectionState.Open)
                    return sqlConnnection;

            if (Sistem.LoadConfigFlag)
            {
                SqlConnection connection = null;
                for (int i = 0; i < sqlConnectionString.Length; i++)
                {
                    connection = new SqlConnection(sqlConnectionString[i]);
                    try
                    {
                        connection.Open();
                        break;
                    }
                    catch (Exception ex)
                    {
                        Sistem.WriteLog(ex, "DBAccessController.dbGetSqlConnection()");
                        connection = null;
                    }
                }
                return connection;
            }
            else
                return null;
        }

        public static OracleConnection dbGetOracleConnection()
        {
            if (oraConnection != null)
                if (oraConnection.State == System.Data.ConnectionState.Open)
                    return oraConnection;

            if (Sistem.LoadConfigFlag)
            {
                OracleConnection connection = null;
                for (int i = 0; i < oraConnectionString.Length; i++)
                {
                    connection = new OracleConnection(oraConnectionString[i]);
                    try
                    {
                        connection.Open();
                        break;
                    }
                    catch (Exception ex)
                    {
                        Sistem.WriteLog(ex, "DBAccessController.dbGetOracleConnection()");
                        connection = null;
                    }
                }
                return connection;
            }
            else
                return null;
        }

        public static OracleConnection dbGetOracleConnection(string connectionString)
        {
            if (oraConnection != null)
                if (oraConnection.State == System.Data.ConnectionState.Open)
                    return oraConnection;

            oraConnection = new OracleConnection(connectionString);
            try
            {
                oraConnection.Open();
            }
            catch (Exception ex)
            {
                oraConnection = null;
            }
            return oraConnection;
        }

        public static bool ExecuteSQLCommand(string cmdText)
        {
            try
            {
                SqlCommand cmd = new SqlCommand(cmdText, dbGetSqlConnection());
                if (cmd.Connection == null)
                    return false;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                dataTable.Dispose();
                dataTable = new DataTable();
                da.Fill(dataTable);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static bool ExecuteOracleCommand(string cmdText)
        {
            try
            {
                OracleCommand cmd = new OracleCommand(cmdText, dbGetOracleConnection());
                if (cmd.Connection == null)
                    return false;
                OracleDataAdapter da = new OracleDataAdapter(cmd);
                dataTable.Dispose();
                dataTable = new DataTable();
                da.Fill(dataTable);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
