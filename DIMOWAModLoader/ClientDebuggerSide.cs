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
    public class ClientDebuggerSide : MonoBehaviour
    {
        ClientSocket clSocket;

        object _sendLogLock = new object();

        object _sendLogMultiThreadLock = new object();

        void Start()
        {
            clSocket = new ClientSocket();
        }

        public void SendLogMultiThread(string data, DebugType debugType = DebugType.LOG)
        {
            if (clSocket.GetIsThereAConnection())
            {
                lock (_sendLogMultiThreadLock)
                {
                    clSocket.SendLog(data, debugType);
                }
            }
        }
        public void SendLog(string data, DebugType debugType = DebugType.LOG, LogingType logingType = LogingType.SEND_AND_LOG)
        {

            if ((logingType == LogingType.SEND_AND_LOG || logingType == LogingType.SEND) && clSocket.GetIsThereAConnection())
            {
                //Debug.Log("Mando para o dubugger. . . ");
                //1-Mandamos o tipo de mensagem que é (ushort)
                //2 - // o tempo da mensagem (em relação ao inicio do jogo) (float)
                //3- tamanho da msg (int)
                //4- a msg (string)

                lock (_sendLogLock)
                {
                    clSocket.SendLog(data, debugType);
                }
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
        readonly object _isThereAConnectionLock = new object();
        private bool IsThereAConnection;
        private void SetIsThereAConnection(bool value) // Eu etentei ta, isso é o melhor que deu para fazer 
        {
            lock (_isThereAConnectionLock)
            {
                IsThereAConnection = value;
            }
        }
        public bool GetIsThereAConnection()
        {
            lock (_isThereAConnectionLock)
            {
                return IsThereAConnection;
            }
        }

        public ClientSocket()
        {
            SetIsThereAConnection(false);

            StartConnection();
        }
        Socket CreateSocket()
        {
            return new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }
        void StartConnection()
        {
            clientSck = CreateSocket();
            clientSck.BeginConnect(new IPEndPoint(IPAddress.Parse("127.1.0.0"), 1997), callback, null);
        }

        private void callback(IAsyncResult ar)
        {
            try
            {
                clientSck.EndConnect(ar);

                SetIsThereAConnection(true);
            }
            catch
            {
                //Iiii, deu ruim, e não tem como fazer debug disso direito
            }

        }

        public void SendLog(string log, DebugType debugType)
        {
            //Melhorar isso para deixar tudo separado e bunitinho
            //1- cria o buffer, 2 - manda o tamanho do buffer, 3- manda o buffer
            if (debugType != 0)
            {

                PacketWriter packetWriter = new PacketWriter();
                packetWriter.Write((byte)debugType);
                packetWriter.Write(Time.realtimeSinceStartup);
                packetWriter.Write(log);

                byte[] data = packetWriter.GetBytes();

                clientSck.Send(BitConverter.GetBytes(data.Length));
                clientSck.Send(data);

                packetWriter.Close();
            }
        }

    }
}
