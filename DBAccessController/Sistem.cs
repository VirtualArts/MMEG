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

        private static StreamWriter sw;

        private static string database_name;
        private static string host;
        private static string user_name;
        private static string password;
        private static string network_library;
        private static int server_port;
        private static List<string> files;

        private bool configLoaded = false;

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

        private static string[] LogTags = { "[Error] ", "[Server] ", "[Info] ", "[SQL] ", "[PL/SQL] ", "[SistemLog] ", "[Cryptography] ", "[CryptographycError] " };

        private static string[] actionTags = { "<File.", "<SQLMS", "SQLPL"};

        public Sistem()
        {
            string fileName = Directory.GetCurrentDirectory() + @"\Error " + DateTime.Now.ToShortDateString().Replace("/", "-") + ".log";
            if (!File.Exists(fileName))
                File.Create(fileName);
            try
            {
                sw = new StreamWriter(fileName);
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

        private bool LoadConfigs()
        {
            try
            {
                string configFilePath = Directory.GetCurrentDirectory() + @"\XMLS\Config.xml";
                if (File.Exists(configFilePath))
                {
                    XmlDocument xmlConfig = new XmlDocument();
                    xmlConfig.Load(configFilePath);
                    foreach (XmlNode tag in xmlConfig.ChildNodes)
                    {
                        foreach (XmlNode item in tag.ChildNodes)
                        {
                            if (item.InnerText == "Sistem")
                            {
                                foreach (XmlNode item2 in item.ChildNodes)
                                {

                                }
                            }
                        }
                    }
                    return true;
                }
                else
                    return false;
               
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
            if (actionTagsPosition < actionTags.Length)
                return actionTags[actionTagsPosition];
            else
                return string.Empty;
        }

    }
}
