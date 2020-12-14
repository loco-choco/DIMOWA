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
            Console.WriteLine("--- DIMOWA's Mod Debugger ---");
            Console.WriteLine("Waiting for CAMOWA to start. . .");
            DebuggerServer debugger = new DebuggerServer();
            while (debugger.ServerIsUp)
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
        public bool ServerIsUp {get; private set; }

        public DebuggerServer()
        {
            CreateServer();
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
            ServerIsUp = true;
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
                        

                        byte[] receivedBuffer = new byte[2];
                        
                        conectedSck.Receive(receivedBuffer, 0, receivedBuffer.Length, 0);
                        DebugType debugType = (DebugType)BitConverter.ToUInt16(receivedBuffer, 0);

                        if (debugType == 0)
                            throw new SocketException();

                        receivedBuffer = new byte[4];

                        conectedSck.Receive(receivedBuffer, 0, receivedBuffer.Length, 0);
                        float sendTime = BitConverter.ToSingle(receivedBuffer, 0);
                        
                        conectedSck.Receive(receivedBuffer, 0, receivedBuffer.Length, 0);
                        int msgSize = BitConverter.ToInt32(receivedBuffer, 0);
                        

                        MemoryStream memStream = new MemoryStream();

                        while (msgSize > 0)
                        {
                            byte[] msgBuffer;

                            if (msgSize < conectedSck.ReceiveBufferSize)
                                msgBuffer = new byte[msgSize];
                            else
                                msgBuffer = new byte[conectedSck.ReceiveBufferSize];

                            int rec = conectedSck.Receive(msgBuffer, 0, msgBuffer.Length, 0);

                            msgSize -= rec;

                            memStream.Write(msgBuffer, 0, msgBuffer.Length);
                        }

                        memStream.Close();

                        byte[] msg = memStream.ToArray();

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

                        Console.WriteLine("- {0} - {1}",sendTime,Encoding.Default.GetString(msg));

                        Console.ResetColor();
                    }
                    catch
                    {
                        Console.WriteLine("CAMOWA was disconnected");
                        Console.WriteLine("Closing Server...");
                        conectedSck.Close();
                        ServerIsUp = false;
                        break;
                    }

                }
            }
            ).Start();
        }

    }

}
