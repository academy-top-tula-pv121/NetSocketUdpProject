using System.Net;
using System.Net.Sockets;
using System.Text;

namespace NetSocketUdpProject
{
    internal class Program
    {
        static int portLocal;
        static int portRemote;
        static Socket socketLocal;

        static void Listener()
        {
            IPEndPoint localEP = new IPEndPoint(IPAddress.Loopback, portLocal);
            socketLocal.Bind(localEP);

            while(true)
            {
                StringBuilder messageFrom = new StringBuilder();
                byte[] buffer = new byte[1024];
                int count = 0;

                EndPoint remoteEP = new IPEndPoint(IPAddress.Loopback, portRemote);

                do
                {
                    count = socketLocal.ReceiveFrom(buffer, buffer.Length, SocketFlags.None, ref remoteEP);
                    messageFrom.Append(Encoding.Default.GetString(buffer, 0, count));

                } while (socketLocal.Available > 0);

                IPEndPoint remoteIPEP = remoteEP as IPEndPoint;

                Console.WriteLine($"{DateTime.Now.ToShortDateString()}: from {remoteIPEP.Address.ToString()} | message: {messageFrom.ToString()}");
            }
        }
        static void Main(string[] args)
        {
            Console.Write("Imput local port: ");
            portLocal = Int32.Parse(Console.ReadLine());

            Console.Write("Imput remote port: ");
            portRemote = Int32.Parse(Console.ReadLine());

            socketLocal = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            Task taskListener = new Task(Listener);
            taskListener.Start();

            while(true)
            {
                Console.Write(">>> ");
                string messageTo = Console.ReadLine();

                byte[] buffer = Encoding.Default.GetBytes(messageTo);
                EndPoint pemoteEP = new IPEndPoint(IPAddress.Loopback, portRemote);

                socketLocal.SendTo(buffer, pemoteEP);
            }
        }
    }
}