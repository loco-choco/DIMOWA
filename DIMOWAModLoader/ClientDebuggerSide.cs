using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

namespace DIMOWAModLoader
{
    public enum LogingType : ushort
    {
        SEND,
        LOG,
        SEND_AND_LOG,
        UNKNOWN,
    }
    public class ClientDebuggerSide: MonoBehaviour
    {
        ClientSocket clSocket;
        
        void Start()
        {
            clSocket = new ClientSocket();
        }
        public void SendString(string data, DebugType debugType = DebugType.LOG,LogingType logingType = LogingType.SEND_AND_LOG)
        {
            
            if((logingType == LogingType.SEND_AND_LOG || logingType == LogingType.SEND) && clSocket.IsThereAConnection)
            {
                //Debug.Log("Mando para o dubugger. . . ");
                //1-Mandamos o tipo de mensagem que é (ushort)
                //2 - // o tempo da mensagem (em relação ao inicio do jogo) (float)
                //3- tamanho da msg (int)
                //4- a msg (string)
                

                clSocket.SendStringData(data, debugType);
            }

            if (logingType == LogingType.SEND_AND_LOG || logingType == LogingType.LOG)
            {
                //Debug.Log("Escrevendo no arquivo. . . ");
                Debug.Log(data);
            }
        }

    }
    class ClientSocket 
    {
        Socket clientSck;
        public bool IsThereAConnection { get; private set; }

        public ClientSocket()
        {
            StartConnection();
        }
        Socket CreateSocket()
        {
            return new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);
        }
        void StartConnection()
        {
            clientSck = CreateSocket();
            try
            {
                clientSck.Connect(new IPEndPoint(IPAddress.Parse("127.1.0.0"), 1997));
                IsThereAConnection = true;
            }
            catch
            {
                IsThereAConnection = false;
            }
            
        }
        public void SendStringData(string data, DebugType debugType)
        {
            //Melhorar isso para deixar tudo separado e bunitinho
            //1- cria o buffer, 2 - manda o tamanho do buffer, 3- manda o buffer
            if (debugType != 0)
            {
                byte[] dataBuffer = Encoding.Default.GetBytes(data);
                clientSck.Send(BitConverter.GetBytes((ushort)debugType));
                clientSck.Send(BitConverter.GetBytes(Time.realtimeSinceStartup));
                clientSck.Send(BitConverter.GetBytes(dataBuffer.Length), 0, 4, 0);
                clientSck.Send(dataBuffer);
            }
        }

    }
}
