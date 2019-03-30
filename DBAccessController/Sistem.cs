using System;
using System.IO;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Xml;
using System.Linq;
using System.Collections.Generic;

namespace Controllers
{
    public class Sistem
    {
        private static List<string> files;
        private static StreamWriter sw;
        private readonly string xmlPath = Directory.GetCurrentDirectory() + @"\XMLS\Config.xml";

        public static Sistem Instance { get; } = new Sistem();
        public static string OraHost { get; set; }
        public static string OraPort { get; set; }
        public static string OraUser { get; set; }
        public static string OraPassword { get; set; }
        public static string OraDatabase { get; set; }
        public static string OraServicename { get; set; }
        public static string SqlHost { get; set; }
        public static string SqlPort { get; set; }
        public static string SqlUser { get; set; }
        public static string SqlPassword { get; set; }
        public static string SqlDatabase { get; set; }
        public static string SqlNetworkLibrary { get; set; }
        public static string SqlLocalDatabaseNamePath { get; set; }
        public static string EncryptPassword { get; set; } = "c3VzdGEyNTA=";
        public static bool LoadConfigFlag { get; set; } = false;
        public static string[] ActionTags { get; set; } = { "<File.", "<SQLMS", "SQLPL" };
        public static string[] LogTags { get; set; } = { "[Error] ", "[Server] ", "[Info] ", "[SQL] ", "[PL/SQL] ", "[SistemLog] ", "[Cryptography] ", "[CryptographycError] " };

        public enum EnumEstados
        {
            NA = 0,
            ALTA = 1,
            MODIFICACION = 2,
            BAJA = 3
        }
        public enum EnumLogTags
        {
            ERROR = 0,
            SERVER = 1,
            INFO = 2,
            SQL = 3,
            SQLERROR = 4,
            PLSQL = 5,
            PLSQLERROR = 6,
            SISTEMLOG = 5,
            CRYPTOGRAPHY = 6,
            CRYPTOGRAPHYCERROR = 7
        }
        public enum EnumActionTags
        {
            FILE = 0,
            SQLMS = 1,
            SQLPL = 2
        }
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

        public Sistem()
        {
            string fileName = Directory.GetCurrentDirectory() + @"\Error " + DateTime.Now.ToShortDateString().Replace("/", "-") + ".log";
            if (!File.Exists(fileName))
                File.Create(fileName);
            try
            {
                sw = new StreamWriter(fileName);
                LoadConfigFlag = LoadConfigs();
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private bool CheckConfigProperties()
        {
            bool sqlresult = true;
            bool oraresult = true;

            try
            {
                if ((string.IsNullOrEmpty(SqlHost) || string.IsNullOrEmpty(SqlDatabase) || string.IsNullOrEmpty(SqlUser) || string.IsNullOrEmpty(SqlPassword)))
                    sqlresult = false;
                if ((string.IsNullOrEmpty(OraHost) || string.IsNullOrEmpty(OraServicename) || string.IsNullOrEmpty(OraPassword) || string.IsNullOrEmpty(OraUser)))
                    oraresult = false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return (sqlresult || oraresult);
        }


        private bool LoadConfigs()
        {
            try
            {
                string configFilePath = xmlPath;
                if (File.Exists(configFilePath))
                {
                    XmlDocument xmlConfig = new XmlDocument();
                    xmlConfig.Load(configFilePath);
                    foreach (XmlNode tag in xmlConfig.ChildNodes)
                    {
                        if (tag.Name == enuConfig.Config.ToString())
                        {
                            foreach (XmlNode item in tag.ChildNodes)
                            {
                                if (item.InnerText == "Sistem")
                                {
                                    foreach (XmlNode item2 in item.ChildNodes)
                                    {
                                        int tagCount;
                                        if (item2.InnerText == "LogTags")
                                        {
                                            tagCount = 0;
                                            LogTags = new string[item2.ChildNodes.Count];
                                            foreach (XmlNode item3 in item2.ChildNodes)
                                            {
                                                LogTags[tagCount] = item3.InnerText;
                                                tagCount++;
                                            }
                                        }
                                        if (item2.InnerText == "ActionTags")
                                        {
                                            tagCount = 0;
                                            ActionTags = new string[item2.ChildNodes.Count];
                                            foreach (XmlNode item3 in item2.ChildNodes)
                                            {
                                                ActionTags[tagCount] = item3.InnerText;
                                                tagCount++;
                                            }
                                        }
                                    }
                                }
                                else if (item.Name == enuConfig.Oracle.ToString())
                                {
                                    //ORACLE ENVIROMENT
                                    foreach (XmlNode childnode2 in item.ChildNodes)
                                    {
                                        switch (childnode2.Name)
                                        {
                                            case "host":
                                                OraHost = childnode2.InnerText;
                                                break;
                                            case "server_port":
                                                OraPort = childnode2.InnerText;
                                                break;
                                            case "service_name":
                                                OraServicename = childnode2.InnerText;
                                                break;
                                            case "user_name":
                                                OraUser = childnode2.InnerText;
                                                break;
                                            case "password":
                                                if (!string.IsNullOrEmpty(childnode2.InnerText))
                                                    OraPassword = SecurityController.RSADecryption(childnode2.InnerText);
                                                else
                                                    OraPassword = string.Empty;
                                                break;
                                        }
                                    }
                                }
                                else if (item.Name == enuConfig.SQL.ToString())
                                {
                                    //SQL ENVIROMENT
                                    foreach (XmlNode childnode2 in item.ChildNodes)
                                    {
                                        switch (childnode2.Name)
                                        {
                                            case "network_library":
                                                SqlNetworkLibrary = childnode2.InnerText;
                                                break;
                                            case "host":
                                                SqlHost = childnode2.InnerText;
                                                break;
                                            case "server_port":
                                                SqlPort = childnode2.InnerText;
                                                break;
                                            case "database_name":
                                                SqlDatabase = childnode2.InnerText;
                                                break;
                                            case "user_name":
                                                SqlUser = childnode2.InnerText;
                                                break;
                                            case "password":
                                                SqlPassword = childnode2.InnerText;
                                                break;
                                            case "database_path":
                                                if (File.Exists(childnode2.InnerText))
                                                    SqlLocalDatabaseNamePath = childnode2.InnerText;
                                                else
                                                    WriteLog("The specified path for the local sql database does not exists.", "LoadConfigs()", true, false, true, ConsoleColor.Red);
                                                break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    // CREATE XML CONFIG FILE
                    StreamWriter sw = new StreamWriter(xmlPath);
                    sw.WriteLine("<?xml version=" + '"' + "1.0" + '"' + "encoding =" + '"' + "utf - 8" + '"' + "?> ");
                    sw.WriteLine("<Config>");
                    sw.WriteLine("");
                    sw.WriteLine("  <Sistem>                                    ");
                    sw.WriteLine("    <LogTags>                                 ");
                    sw.WriteLine("      <logTag1>[Error] </logTag1>             ");
                    sw.WriteLine("      <logTag2>[Server] </logTag2>            ");
                    sw.WriteLine("      <logTag3>[Info] </logTag3>              ");
                    sw.WriteLine("      <logTag4>[SQL] </logTag4>               ");
                    sw.WriteLine("      <logTag5>[SistemLog] </logTag5>         ");
                    sw.WriteLine("      <logTag6>[Cryptography] </logTag6>      ");
                    sw.WriteLine("      <logTag7>[CryptographycError] </logTag7>");
                    sw.WriteLine("    </LogTags>                                ");
                    sw.WriteLine("    <ActionTags>                              ");
                    sw.WriteLine("      <ActionTag1></ActionTag1>               ");
                    sw.WriteLine("      <ActionTag2></ActionTag2>               ");
                    sw.WriteLine("      <ActionTag3></ActionTag3>               ");
                    sw.WriteLine("    </ActionTags>                             ");
                    sw.WriteLine("  </Sistem>                                   ");
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
                return CheckConfigProperties();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static string[] getFilesRecursive(string initialPath)
        {
            if (Directory.Exists(initialPath))
            {
                files = new List<string>();
                foreach (var item in Directory.GetFiles(initialPath))
                {
                    files.Add(item);
                }
                foreach (var item in Directory.GetDirectories(initialPath))
                {
                    getFilesRecursive(item);
                }
            }
            else
                return null;

            return files.ToArray();
        }

        public static SqlConnection GetSqlConnection()
        {
            SqlConnection con = null;
            try
            {
                con = DBAccessController.dbGetSqlConnection();
            }
            catch (Exception ex)
            {
                WriteLog(ex, "GetSqlConnection()", true, true);
            }
            return con;
        }

        public static bool WriteLog(string msg, string methodConstructor, bool showDisplayMsg = false, bool throwException = false, bool PrintF = false, ConsoleColor color = ConsoleColor.Gray)
        {
            bool result = false;
            try
            {
                string data = GetLogTag(EnumLogTags.SISTEMLOG) + DateTime.Now + "  Message: " + msg + " " + methodConstructor;
                if (sw != null)
                    sw.WriteLine(data);
                result = true;

                if (PrintF)
                    printF(data, color);

                if (showDisplayMsg)
                {
                    if (throwException)
                        throw new Exception(data);
                    else
                        showDefaultDisplayMessage();
                }
                else if (throwException)
                    throw new Exception(data);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return result;
        }

        public static bool WriteLog(Exception msg, string methodConstructor, bool bPrintF = false, bool showDisplayMsg = false, bool throwException = false)
        {
            bool result = false;
            try
            {
                string data = "[" + DateTime.Now + "]Message: " + methodConstructor + " " + msg.Message + "\t";
                data += "Inner Exception: " + msg.InnerException + "\t";
                data += "StackTrace: " + msg.StackTrace + "\n";
                if (sw != null)
                    sw.WriteLine(data);

                result = true;

                if (showDisplayMsg)
                {
                    if (throwException)
                        throw new Exception(data);
                    else
                        showDefaultDisplayMessage();
                }
                else if (throwException)
                    throw new Exception(data);

                if (bPrintF)
                    printF(data, ConsoleColor.Red, false, true);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return result;
        }

        public static bool closeLog()
        {
            try
            {
                sw.Close();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void showDefaultDisplayMessage()
        {
            MessageBox.Show("Verifique el archivo .log para ver el error que se generó", "Sistema");
        }

        public static void printF(string message, ConsoleColor color = ConsoleColor.Gray, bool readline = false, bool readkey = false, bool goBackGrayColorAfterMessage = true, bool writeLine = true, bool read = false)
        {
            Console.ForegroundColor = color;

            if (writeLine)
                Console.WriteLine(message);
            if (readline)
                Console.ReadLine();
            else if (readkey)
                Console.ReadKey();
            else if (read)
                Console.Read();

            if (goBackGrayColorAfterMessage)
                Console.ForegroundColor = ConsoleColor.Gray;
        }

        public static string GetLogTag(EnumLogTags logTag)
        {
            string result = string.Empty;
            for (int i = 0; i < LogTags.Length; i++)
            {
                if (i == (int)logTag)
                {
                    result = LogTags[(int)logTag];
                    break;
                }
            }
            return result;
        }

        public static string GetActionTag(int actionTagsPosition)
        {
            if (actionTagsPosition < ActionTags.Length)
                return ActionTags[actionTagsPosition];
            else
                return string.Empty;
        }

    }
}
