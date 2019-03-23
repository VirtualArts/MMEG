using System;
using System.Linq;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Threading;

namespace AutoLoginPro
{
    public class AutoLoginPro
    {

        static Process p;

        [DllImport("User32.dll")]
        static extern int SetForegroundWindow(IntPtr point);

        public static void login(string username = "", string password = "", string processName = "")
        {
            p = Process.GetProcesses().Where(i => i.ProcessName.Contains(processName) && i.MainWindowTitle.Contains(processName)).FirstOrDefault();
            if (p != null)
            {
                Console.WriteLine("[SYS] Se encontro el proceso");
                SetForegroundWindow(p.MainWindowHandle);
                SendKeys.SendWait(username + "{TAB}" + password + "{ENTER}");
                Console.WriteLine("[SYS] Se envio el login: " + username + "," + password);
                p.Close();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[SYS] No se encontro el proceso " + processName);
                Console.WriteLine("[SYS] Error al enviar el login: " + username + "," + password);
                Console.ForegroundColor = ConsoleColor.Gray;
            }
        }

        public static void detectEBO(string username = "", string password = "", string processName = "")
        {
            if (string.IsNullOrEmpty(processName))
                return;

            Console.WriteLine("[SYS] Detectando EBO ");
            bool eboStarted = false;
            while (!eboStarted)
            {
                if (Process.GetProcesses().ToList().Where(i => i.MainWindowTitle.Contains(processName) && !i.MainWindowTitle.Contains("Microsoft")).Count() > 0)
                {
                    eboStarted = true;
                    login(username, password, processName);
                }
                Thread.Sleep(2500);
            }
        }

    }
}
