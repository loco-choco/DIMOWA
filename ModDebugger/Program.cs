using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;

namespace ModDebugger
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Mod Debugger";
            Console.WriteLine("--- DIMOWA's Mod Debugger ---");
            Console.WriteLine("Waiting for CAMOWA to start. . .");
            DebuggerServer debugger = new DebuggerServer();

            //Quando descobrir uma maneira de ler e escrever em threads diferentes, ai podemos fazer isso ai
            while (debugger.GetServerIsUp())
            {

            }

            Console.WriteLine("The Debugger will close, press ENTER to close it");
            Console.Read();
            
        }
    }
    class DebuggerServer
    {
        Socket serverSck;
        Socket conectedSck;

        private bool serverIsUp;
        private readonly object lock_serverIsUp = new object();

        public DebuggerServer()
        {
            CreateServer();
        }
        private void SetServerIsUp(bool value)
        {
            lock (lock_serverIsUp)
            {
                serverIsUp = value;
            }
        }
        public bool GetServerIsUp()
        {
            lock (lock_serverIsUp)
            {
                return serverIsUp;
            }
        }

        private Socket CreateSocket()
        {
            return new Socket(AddressFamily.InterNetwork,SocketType.Stream,0);
        }
        private void CreateServer()
        {
            serverSck = CreateSocket();
            serverSck.Bind(new IPEndPoint(IPAddress.Parse("127.1.0.0"),1997));
            serverSck.Listen(0);
            SetServerIsUp(true);
            //Tranformar tudo abaixo
            new Thread(() =>
            {
                conectedSck = serverSck.Accept();
            Console.WriteLine("CAMOWA was started!");
            serverSck.Close();
            while (true)
            {
                try
                {
                    //No Cliente ....:
                    //1-Mandamos o tipo de mensagem que é (ushort) - 2 bytes
                    //2 - // o tempo da mensagem (em relação ao inicio do jogo) (float) - 4 bytes
                    //3- tamanho da msg (int) - //
                    //4- a msg (string) - ...

                    byte[] buffer = new byte[4]; // Descobrir o tamanho do pacote
                    conectedSck.Receive(buffer, 0, buffer.Length, 0);

                    int bufferSize = BitConverter.ToInt32(buffer, 0);

                    if (bufferSize <= 0)
                        throw new SocketException();

                    buffer = new byte[bufferSize];

                    conectedSck.Receive(buffer, 0, buffer.Length, 0);

                    PacketReader packetReader = new PacketReader(buffer); // Ler todo o pacote de uma vez só

                    DebugType debugType = (DebugType)packetReader.ReadByte();

                    float sendTime = packetReader.ReadSingle();

                    string logMessage = packetReader.ReadString();

                    switch (debugType)
                    {
                        case DebugType.LOG:
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.Write("[LOG]");
                            break;

                        case DebugType.WARNING:
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            Console.Write("[WARNING]");
                            break;

                        case DebugType.ERROR:
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write("[ERROR]");
                            break;

                        case DebugType.UNKNOWN:
                        default:
                            Console.ForegroundColor = ConsoleColor.DarkMagenta;
                            Console.Write("[UNKNOWN]");
                            break;
                    }

                    Console.WriteLine("- {0} - {1}", sendTime, logMessage);

                    packetReader.Close();
                    Console.ResetColor();
                }
                catch
                {

                    Console.WriteLine("CAMOWA was disconnected");
                    Console.WriteLine("Closing Server...");

                    conectedSck.Close();
                    SetServerIsUp(false);
                    Console.WriteLine("Server Closed");
                    break;
                }
            }

            }
            ).Start();

        }

    }

}
