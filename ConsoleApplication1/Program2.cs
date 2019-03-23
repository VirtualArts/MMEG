using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.DataAccess.Client;
using System.Data;
using System.IO;
using System.Diagnostics;
using static System.Console;

namespace ConsoleApplication1
{
    class Program2
    {

        static string datasource, user, pw, cmd, connection;
        static object resultSet;

        private static void setUpVariables()
        {
            printTxt("Ingrese Data Source:");
            datasource = "latx24.nam.nsroot.net"; //ReadLine();
            printTxt("Ingrese User:");
            user = ReadLine();
            printTxt("Ingrese Password:");
            pw = ReadLine();
        }

        private static void printTxt(string txt)
        {
            ForegroundColor = ConsoleColor.Green;
            WriteLine(txt);
            ForegroundColor = ConsoleColor.Gray;
        }


        static void Main(string[] args)
        {
            //setUpVariables();
            DBAccessController obj = new DBAccessController();
            if (DBAccessController.dbGetOracleConnection() != null)
            {
                DataTable ddt = createOraCmd();
                if (ddt != null)
                    createFileFromDt(ddt);
            }
            else
                Main(null);
        }

        static bool createFileFromDt(DataTable dt)
        {
            bool result = false;
            if (dt != null)
            {
                try
                {
                    string ruta = Directory.GetCurrentDirectory() + "ResultSet" + DateTime.Now.ToLongDateString() + ".txt";
                    StreamWriter sw = new StreamWriter(ruta);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        for (int j = 0; j < dt.Columns.Count; j++)
                        {
                            if (j == (dt.Columns.Count - 1))
                                sw.WriteLine(dt.Rows[i][j].ToString());
                            else
                                sw.Write(dt.Rows[i][j].ToString());
                        }
                    }
                    sw.Close();
                    printTxt("Desea abrir el resultado? y/n: ");
                    if (ReadLine().Contains("y"))
                    {
                        Process.Start("notepad.exe", ruta);
                    }
                    result = true;
                }
                catch
                {

                }
            }
            return result;
        }
        static DataTable createOraCmd()
        {
            DataTable dt = null;
            try
            {
                WriteLine("Command Line = ");
                cmd = ReadLine();
                OracleCommand oraCmd = new OracleCommand(cmd, DBAccessController.dbGetOracleConnection(connection));
                OracleDataAdapter da = new OracleDataAdapter(oraCmd);
                da.Fill(dt);
                return dt;
            }
            catch
            {
                dt = null;
            }
            return dt;
        }


        static void Main11(string[] args)
        {
            int[,] input = new int[,] { { 1, 0 }, { 1, 1 }, { 0, 1 }, { 0, 0 } };
            int[] outputs = { 0, 1, 0, 0 };

            Random r = new Random();

            double[] weights = { r.NextDouble(), r.NextDouble(), r.NextDouble() };

            double learningRate = 1;
            double totalError = 1;

            while (totalError > 0.2)
            {
                totalError = 0;
                for (int i = 0; i < outputs.Length; i++)
                {
                    int output = calculateOutput(input[i, 0], input[i, 1], weights);

                    int error = outputs[i] - output;

                    weights[0] += learningRate * error * input[i, 0];
                    weights[1] += learningRate * error * input[i, 1];
                    weights[2] += learningRate * error * 1;

                    totalError += Math.Abs(error);
                }
            }

            WriteLine("Results:");
            for (int i = 0; i < outputs.Length; i++)
                WriteLine(calculateOutput(input[i, 0], input[i, 1], weights));

            ReadLine();
        }

        private static int calculateOutput(double input1, double input2, double[] weights)
        {
            double sum = input1 * weights[0] + input2 * weights[1] + 1 * weights[2];
            return (sum >= 0) ? 1 : 0;
        }


        static void Main4(string[] args)
        {
            foreach (var name in GetNames())
            {
                WriteLine(name);
            }
            ReadKey();
        }

        static IEnumerable<string> GetNames()
        {
            string[] names =
            {
            "Archimedes", "Pythagoras", "Euclid", "Socrates", "Plato"
            };
            foreach (var name in names)
            {
                yield return name;
            }
        }

        static void Main3(string[] args)
        {
            string s = null;
            WriteLine($"The first letter of {s} is {s?[0] ?? '?'}");
            ReadKey();
        }

        static void Main5(string[] args)
        {
            try
            {

                DBAccessController ora = new DBAccessController();

                string[] connectionString = { "Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=latx24.nam.nsroot.net)(PORT=1521)))",
                                              "Data Source=latx24.nam.nsroot.net;User Id=PRISM9;Password=PRISM9;",
                                              "Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=latx24.nam.nsroot.net)(PORT=1521)))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=SIT11G)));User Id=PRISM9; Password = PRISM9; "
                                            };
                OracleConnection conection = new OracleConnection(connectionString[2]);
                conection.Open();
                //OracleCommand cmd = new OracleCommand("SELECT A.*, B.* FROM all_tab_columns A LEFT JOIN(SELECT B.* FROM all_tab_columns B) B ON B.column_name = A.column_name;", conection);
                OracleCommand cmd = new OracleCommand("SELECT A.* FROM all_tab_columns A", conection);
                OracleDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {

                }
                OracleDataAdapter da = new OracleDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                WriteLine("finished");
                ReadKey();
            }
            catch (Exception ex)
            {
                ForegroundColor = ConsoleColor.Red;
                WriteLine(ex.Message);
            }
            ReadKey();
        }

    }
}
