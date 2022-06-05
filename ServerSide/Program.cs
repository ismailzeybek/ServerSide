using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Server
{

    class Program
    {

        static void Main(string[] args)
        {
            ExecuteServer();
        }

        public static void ExecuteServer()
        {
          
            IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddr, 11111);

          
            Socket listener = new Socket(ipAddr.AddressFamily,
                         SocketType.Stream, ProtocolType.Tcp);

            try
            {

             
                listener.Bind(localEndPoint);

                listener.Listen(10);

                while (true)
                {

                    Console.WriteLine("Waiting connection ... ");

                    
                    Socket clientSocket = listener.Accept();

                    ReceiveMessage(clientSocket);

                    Console.WriteLine("Enter the Message.");

                    string userMessage = Console.ReadLine();

             

                    byte[] tmpSource = ASCIIEncoding.ASCII.GetBytes(userMessage);

                    SendMessage(clientSocket, userMessage);


                    clientSocket.Shutdown(SocketShutdown.Both);
                    clientSocket.Close();
                }
            }

            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public static void ReceiveMessage(Socket clientSocket)
        {
            byte[] bytes = new Byte[1024];
            string data = null;

            while (true)
            {

                int numByte = clientSocket.Receive(bytes);

                data += Encoding.ASCII.GetString(bytes,
                                           0, numByte);

                if (data.IndexOf("<EOF>") > -1)
                    break;
            }

            Console.WriteLine("Text received -> {0} ", data);
        }

        public static void SendMessage(Socket clientSocket,string msg)
        {
            byte[] message = Encoding.ASCII.GetBytes(msg);

            clientSocket.Send(message);
        }
    }
}