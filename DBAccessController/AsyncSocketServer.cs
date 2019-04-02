using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Controllers
{

    public class Client
    {
        public int ID { get; set; } = 0;
        public string IP { get; set; } = null;
        public DateTime ConnectionTime { get; set; }
        public byte[] DataSended { get; set; }
        public int Port { get; set; }

        public Client(int id)
        {
            ID = id;
        }
    }

    public class ClientsController
    {
        public List<Client> ClientList { get; set; } = new List<Client>();

        public ClientsController()
        {

        }

        public bool AddClient(Client client)
        {
            try
            {
                ClientList.Add(client);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool RemoveClient(Client client)
        {
            try
            {
                ClientList.Remove(client);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public string GetSpecificClientSendedData(Client client)
        {
            try
            {
                string result = Encoding.UTF8.GetString(ClientList[client.ID].DataSended) + '\n';
                return result;
            }
            catch
            {
                return null;
            }
        }

        public int GetClientListCount()
        {
            return ClientList.Count;
        }

    }

    public class StateObject
    {
        // Client  socket.  
        public Socket workSocket = null;
        // Size of receive buffer.  
        public const int BufferSize = 1024;
        // Receive buffer.  
        public byte[] buffer = new byte[BufferSize];
        // Received data string.  
        public StringBuilder sb = new StringBuilder();

    }

    public class AsyncSocketServer
    {
        private static byte[] buffer = new byte[0];

        public static ClientsController ClientController { get; set; } = new ClientsController();
        public static ManualResetEvent AllDone { get; set; } = new ManualResetEvent(false);
        public static int Port { get; set; } = 2080;


        protected AsyncSocketServer(int port = 2080)
        {
            Port = port;
        }

        public static void StartListening()
        {
            // Establish the local endpoint for the socket.  
            // The DNS name of the computer  
            // running the listener is "host.contoso.com".  
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, Port);

            // Create a TCP/IP socket.  
            Socket listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            // Bind the socket to the local endpoint and listen for incoming connections.  
            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(100);

                Sistem.printF(Sistem.GetLogTag(Sistem.EnumLogTags.SERVER) + "Async Socket Server  ONLINE at " + ipAddress.ToString() + ":" + Port, ConsoleColor.Green);

                while (true)
                {
                    // Set the event to nonsignaled state.  
                    AllDone.Reset();

                    // Start an asynchronous socket to listen for connections.  
                    Sistem.printF(Sistem.GetLogTag(Sistem.EnumLogTags.SERVER) + "Waiting for a connection...", ConsoleColor.Gray);
                    listener.BeginAccept(new AsyncCallback(AcceptCallback), listener);

                    // Wait until a connection is made before continuing.  
                    AllDone.WaitOne();
                }

            }
            catch (Exception e)
            {
                Sistem.printF(e.ToString(), ConsoleColor.Red);
            }

            Sistem.printF("\nPress ENTER to continue...", ConsoleColor.Gray, false, false, true, true, true);
        }

        public static void AcceptCallback(IAsyncResult ar)
        {
            // Signal the main thread to continue.  
            AllDone.Set();

            // Get the socket that handles the client request.  
            Socket listener = (Socket)ar.AsyncState;
            Socket handler = listener.EndAccept(ar);

            // Create the state object.  
            StateObject state = new StateObject();
            state.workSocket = handler;

            Client client = new Client(ClientController.GetClientListCount());
            client.ConnectionTime = DateTime.Now;
            client.IP = Convert.ToString(IPAddress.Parse(((IPEndPoint)state.workSocket.RemoteEndPoint).Address.ToString()));
            client.Port = ((IPEndPoint)state.workSocket.RemoteEndPoint).Port;
            ClientController.AddClient(client);

            IPEndPoint clientEndPoint = (IPEndPoint)state.workSocket.RemoteEndPoint;
            Sistem.printF(string.Format("Client Connected: {0}", clientEndPoint.Address.ToString()), ConsoleColor.Green);

            handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallback), state);
        }

        public static void withrawBytes(ref byte[] destination, byte[] bufferToWithraw, int increaseArray)
        {
            int oldArraySize = destination.Length;
            int newArraySize = destination.Length + increaseArray;
            Array.Resize(ref destination, newArraySize);
            Buffer.BlockCopy(bufferToWithraw, 0, destination, oldArraySize, increaseArray);
        }

        public static void ReadCallback(IAsyncResult ar)
        {
            string content = string.Empty;

            // Retrieve the state object and the handler socket from the asynchronous state object.  
            StateObject state = (StateObject)ar.AsyncState;
            Socket handler = state.workSocket;

            // Read data from the client socket.   
            int bytesRead = handler.EndReceive(ar);

            if (bytesRead > 0)
            {
                // There  might be more data, so store the data received so far.
                state.sb.Append(Encoding.UTF8.GetString(state.buffer, 0, bytesRead));
                withrawBytes(ref buffer, state.buffer, bytesRead);
                // Check for end-of-file tag. If it is not there, read  more data.  
                content = state.sb.ToString();

                if (content.Contains("<File.") || content.Contains("<SQL"))
                {
                    // All the data has been read from the  client. Display it on the console.
                    bool result = InterpretData(buffer);
                    if (result)
                    {
                        Sistem.printF(string.Format("Read {0} bytes from socket. \n", content.Length), ConsoleColor.Green);
                        // Echo the data encrypted back to the client.  
                        Send(handler, buffer);
                    }
                    else
                        Send(handler, new byte[0]);
                }
                else                    // Not all data received. Get more.  
                {
                    handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallback), state);
                }
            }
        }

        private static void Send(Socket handler, byte[] data)
        {
            // Begin sending the data to the remote device.  
            handler.BeginSend(data, 0, data.Length, 0, new AsyncCallback(SendCallback), handler);
        }

        private static void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.  
                Socket handler = (Socket)ar.AsyncState;

                // Complete sending the data to the remote device.  
                int bytesSent = handler.EndSend(ar);
                Sistem.printF(string.Format("Sent {0} bytes to client.", bytesSent), ConsoleColor.Gray);

                handler.Shutdown(SocketShutdown.Both);
                handler.Close();

            }
            catch (Exception e)
            {
                Sistem.printF(e.ToString(), ConsoleColor.Red);
            }
        }

        private static bool InterpretData(byte[] encryptedData)
        {
            bool result = false;
            try
            {
                Task action = Task.Run(() => result = InterpretDataActionController.CallInterpretData(encryptedData).Result);
                action.Wait();
            }
            catch (Exception ex)
            {
                Sistem.WriteLog(ex, "AsyncSocketServer.InterpretData(byte[] encryptedData)");
            }
            return result;
        }

        private static string DecodePassword(string password)
        {
            try
            {
                return Encoding.UTF8.GetString(Convert.FromBase64CharArray(password.ToCharArray(), 0, password.Length));
            }
            catch (Exception ex)
            {
                Sistem.WriteLog(ex, "AsyncSocketServer.DecryptMessage(string data)");
            }
            return null;
        }

        [STAThread]
        public static int Main(string[] args)
        {
            try
            {
                StartListening();
                return 1;
            }
            catch
            {
                return 0;
            }
        }
    }
}
