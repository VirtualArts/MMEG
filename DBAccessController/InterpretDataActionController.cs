using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace Controllers
{
    public class InterpretDataActionController
    {
        private const int actionBytesCount = 20;

        public static async Task<bool> CallInterpretData(byte[] data)
        {
            bool result = await InterpretData(data);
            return result;
        }

        public static async Task<bool> InterpretData(byte[] encryptedData)
        {
            bool result = false;
            try
            {
                Task taskAction = null;
                byte[] realDataEncrypted = new byte[encryptedData.Length - actionBytesCount];
                Buffer.BlockCopy(encryptedData, 0, realDataEncrypted, 0, realDataEncrypted.Length);
                byte[] actionBytes = new byte[20];
                Buffer.BlockCopy(encryptedData, encryptedData.Length - actionBytesCount, actionBytes, 0, actionBytes.Length);
                string action = Encoding.UTF8.GetString(actionBytes);
                string fileExtension;
                string dataEncripted = Encoding.UTF8.GetString(realDataEncrypted);
                byte[] realDataDecrypted = SecurityController.DESDecrypt(realDataEncrypted, "susta250", false, false);

                if (action.Contains(Sistem.GetActionTag((int)Sistem.EnumActionTags.FILE)))
                {
                    int lastIdxOfDot = action.LastIndexOf(".");
                    fileExtension = action.Substring(lastIdxOfDot, action.Length - lastIdxOfDot).Replace(">", "").Replace("\0", "");
                    taskAction = Task.Run(() => result = FileDataArrival(realDataDecrypted, fileExtension).Result);
                }

                if (action.Contains(Sistem.GetActionTag((int)Sistem.EnumActionTags.SQLMS)))
                    taskAction = Task.Run(() => result = SqlCommandExecute(realDataDecrypted, action).Result);
                else
                    if (action.Contains(Sistem.GetActionTag((int)Sistem.EnumActionTags.SQLPL)))
                    taskAction = Task.Run(() => result = OracleCommandExecute(realDataDecrypted, action).Result);
                else
                    result = false;

                if (taskAction != null)
                {
                    taskAction.Wait();
                    taskAction.Dispose();
                }
            }
            catch (Exception ex)
            {
                Sistem.WriteLog(ex, "InterpretData(byte[] encryptedData)", true);
            }
            return result;
        }

        public static async Task<bool> FileDataArrival(byte[] fileBuffer, string fileExtension)
        {
            bool result = false;
            try
            {
                string filename = Directory.GetCurrentDirectory() + "\\" + DateTime.Now.ToShortDateString().Replace("/", "-") + "-" + DateTime.Now.Hour + "hs" + DateTime.Now.Minute + fileExtension;
                File.WriteAllBytes(filename, fileBuffer);
                Sistem.WriteLog(Sistem.GetLogTag(Sistem.EnumLogTags.SERVER) + "File Created: " + filename, "InterpretDataActionController.InterpretData(string encryptedData)", true);
                result = true;
            }
            catch (Exception ex)
            {
                Sistem.WriteLog(ex, "FileDataArrival(byte[] fileBuffer, string fileExtension)", true);
            }
            return result;
        }
        /// <summary>
        /// EXEC NON QUERY COMMANDS WITH NO RETURN OF DATA
        /// </summary>
        /// <param name="dataDecrypted"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static async Task<bool> SqlCommandExecute(byte[] dataDecrypted, string action)
        {
            bool result = false;
            try
            {
                //INSERT, UPDATE AND DELETE
                string sqlCommandString = Encoding.UTF8.GetString(dataDecrypted);
                SqlCommand cmd = new SqlCommand(sqlCommandString, DBAccessController.dbGetSqlConnection());
                int afectedRows = cmd.ExecuteNonQuery();
                if (afectedRows > 0)
                    result = true;
            }
            catch (Exception ex)
            {
                Sistem.WriteLog(ex, "SqlCommandExecute(byte[] dataDecrypted, string action)", true);
            }
            return result;
        }

        /// <summary>
        /// EXEC QUERY COMMANDS THAT RETURNS DATA
        /// </summary>
        /// <param name="dataDecrypted"></param>
        /// <param name="action"></param>
        /// <param name="returnDt"></param>
        /// <returns></returns>
        public static async Task<DataTable> SqlCommandExecute(byte[] dataDecrypted, string action, bool returnDt = true)
        {
            DataTable result = null;
            try
            {
                string sqlCommandString = Encoding.UTF8.GetString(dataDecrypted);
                SqlCommand cmd = new SqlCommand(Encoding.UTF8.GetString(dataDecrypted), DBAccessController.dbGetSqlConnection());
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(result);
            }
            catch (Exception ex)
            {
                Sistem.WriteLog(ex, "SqlCommandExecute(byte[] dataDecrypted, string action, bool returnDt = true)", true);
            }
            return result;
        }

        public static async Task<bool> OracleCommandExecute(byte[] dataDecrypted, string action)
        {
            bool result = false;
            try
            {
                string oracleCommandString = Encoding.UTF8.GetString(dataDecrypted);
                OracleCommand cmd = new OracleCommand(oracleCommandString, DBAccessController.dbGetOracleConnection());
                int afectedRows = cmd.ExecuteNonQuery();
                if (afectedRows > 0)
                    result = true;
            }
            catch (Exception ex)
            {
                Sistem.WriteLog(ex, "OracleCommandExecute(byte[] dataDecrypted, string action)", true);
            }
            return result;
        }

        public static bool Main(byte[] data)
        {
            Task<bool> action = Task.Run(() => CallInterpretData(data));
            if (action.IsCompleted && action.Status == TaskStatus.RanToCompletion && action.Result)
                return true;
            else
                return false;
        }

    }
}
