using System;
using Oracle.ManagedDataAccess.Client;
using System.Xml;
using System.IO;
using System.Data.SqlClient;
using System.Data;

namespace Controllers
{
    public class DBAccessController
    {
        static OracleConnection oraConnection = null;
        static SqlConnection sqlConnnection = null;
        static DataTable dataTable = null;

        public enum enuConfig
        {
            Config = 0,
            Oracle = 1,
            SQL = 2,
            host = 3,
            server_port = 4,
            service_name = 5,
            user_name = 6,
            password = 7,
            database_name = 8,
            network_library = 9
        }

        static string[] oraConnectionString;
        static string[] sqlConnectionString;

        static string oraHost, oraPort, oraUser, oraPassword, oraDatabase, oraServicename;
        static string sqlHost, sqlPort, sqlUser, sqlPassword, sqlDatabase, sqlNetworkLibrary;
        static bool loadConfigFlag = false;

        string xmlPath = Directory.GetCurrentDirectory() + @"\XMLS\Config.xml";


        public DBAccessController()
        {
            loadConfigFlag = loadConfig();

            if (loadConfigFlag)
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
                                string.Format("Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST={0})(PORT={1})))",oraHost,oraPort),
                                string.Format("Data Source={0};User Id={1};Password={2};",oraHost,oraUser,oraPassword),
                                string.Format("Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST={0})(PORT={1})))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME={2})));User Id={3}; Password = {4};",oraHost,oraPort,oraServicename,oraUser,oraPassword),
                                string.Format("Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST={0})(PORT={1}))(CONNECT_DATA=(SERVICE_NAME={2})));User Id={3}; Password = {4};",oraHost,oraPort,oraServicename,oraUser,oraPassword)
            };

            string[] sqlCon = {
                                string.Format("Server={0};Database={1};User Id={2};Password={3};",sqlHost,sqlDatabase,sqlUser,sqlPassword),
                                string.Format("Data Source={0},{1};Network Library={2};Initial Catalog={3}; User ID = {4}; Password={5};",sqlHost,sqlPort,sqlNetworkLibrary,sqlDatabase,sqlUser,sqlPassword),
                                string.Format("Server=(localdb)\v11.0;Integrated Security=true;AttachDbFileName={0};", Directory.GetCurrentDirectory() + @"\Database1.mdf")
            };

            sqlConnectionString = sqlCon;
            oraConnectionString = oraCon;
        }

        public static SqlConnection dbGetSqlConnection()
        {
            if (sqlConnnection != null)
                if (sqlConnnection.State == System.Data.ConnectionState.Open)
                    return sqlConnnection;

            if (loadConfigFlag)
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

            if (loadConfigFlag)
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

        private bool CheckConfigProperties()
        {
            bool sqlresult = true;
            bool oraresult = true;

            try
            {
                if ((string.IsNullOrEmpty(sqlHost) || string.IsNullOrEmpty(sqlDatabase) || string.IsNullOrEmpty(sqlUser) || string.IsNullOrEmpty(sqlPassword)))
                    sqlresult = false;
                if ((string.IsNullOrEmpty(oraHost) || string.IsNullOrEmpty(oraServicename) || string.IsNullOrEmpty(oraPassword) || string.IsNullOrEmpty(oraUser)))
                    oraresult = false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return (sqlresult || oraresult) ? true : false;
        }

        private bool loadConfig()
        {
            bool methodResult = false;
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                if (File.Exists(xmlPath))
                {
                    xmlDoc.Load(xmlPath);
                    foreach (XmlNode node in xmlDoc.ChildNodes)
                    {
                        if (node.Name == enuConfig.Config.ToString())
                        {
                            foreach (XmlNode childnode in node.ChildNodes)
                            {
                                if (childnode.Name == enuConfig.Oracle.ToString())
                                {
                                    //ORACLE ENVIROMENT
                                    foreach (XmlNode childnode2 in childnode.ChildNodes)
                                    {
                                        switch (childnode2.Name)
                                        {
                                            case "host":
                                                oraHost = childnode2.InnerText;
                                                break;
                                            case "server_port":
                                                oraPort = childnode2.InnerText;
                                                break;
                                            case "service_name":
                                                oraServicename = childnode2.InnerText;
                                                break;
                                            case "user_name":
                                                oraUser = childnode2.InnerText;
                                                break;
                                            case "password":
                                                if (!string.IsNullOrEmpty(childnode2.InnerText))
                                                    oraPassword = SecurityController.RSADecryption(childnode2.InnerText);
                                                else
                                                    oraPassword = string.Empty;
                                                break;
                                        }
                                    }
                                }
                                else if (childnode.Name == enuConfig.SQL.ToString())
                                {
                                    //SQL ENVIROMENT
                                    foreach (XmlNode childnode2 in childnode.ChildNodes)
                                    {
                                        switch (childnode2.Name)
                                        {
                                            case "network_library":
                                                sqlNetworkLibrary = childnode2.InnerText;
                                                break;
                                            case "host":
                                                sqlHost = childnode2.InnerText;
                                                break;
                                            case "server_port":
                                                sqlPort = childnode2.InnerText;
                                                break;
                                            case "database_name":
                                                sqlDatabase = childnode2.InnerText;
                                                break;
                                            case "user_name":
                                                sqlUser = childnode2.InnerText;
                                                break;
                                            case "password":
                                                sqlPassword = childnode2.InnerText;
                                                break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    methodResult = CheckConfigProperties();
                }
                else
                {
                    // CREATE XML CONFIG FILE
                    StreamWriter sw = new StreamWriter(xmlPath);
                    sw.WriteLine("<?xml version=" + '"' + "1.0" + '"' + "encoding =" + '"' + "utf - 8" + '"' + "?> ");
                    sw.WriteLine("<Config>");
                    sw.WriteLine("");
                    sw.WriteLine("  <Oracle>");
                    sw.WriteLine("    <host></host>");
                    sw.WriteLine("    <server_port></server_port>");
                    sw.WriteLine("    <service_name></service_name>");
                    sw.WriteLine("    <user_name></user_name>");
                    sw.WriteLine("    <password></password>");
                    sw.WriteLine("  </Oracle>");
                    sw.WriteLine("");
                    sw.WriteLine("  <SQL>");
                    sw.WriteLine("    <host>latx24.nam.nsroot.net</host>");
                    sw.WriteLine("    <server_port>1433</server_port>");
                    sw.WriteLine("    <database_name>SIT11G</database_name>");
                    sw.WriteLine("    <user_name>PRISM9</user_name>");
                    sw.WriteLine("    <password>PRISM9</password>");
                    sw.WriteLine("    <network_library></network_library>");
                    sw.WriteLine("  </SQL>");
                    sw.WriteLine("");
                    sw.WriteLine("</Config>");
                    sw.Close();
                }
            }
            catch (Exception ex)
            {
                Sistem.WriteLog(ex, "DBAccessController.loadConfig()");
                throw new Exception(ex.Message);
            }
            return methodResult;
        }

    }
}
