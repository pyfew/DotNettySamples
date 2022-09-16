// See https://public static void Main()
using System.Net.Sockets;
using System.Net;
using System.Text;
{
    while (true)
    {
        Socket udpClient = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        EndPoint serverPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 6666);
        string message = Console.ReadLine();
        byte[] data = Encoding.UTF8.GetBytes(message);
        udpClient.SendTo(data, serverPoint);
    }
}