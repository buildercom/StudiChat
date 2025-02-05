using System.Net.Sockets;
using System.Net;
using System.Text;
using ConsoleServer.Models;
using System.Text.Json;

namespace ConsoleServer;

internal class Program
{
    private static Dictionary<IPEndPoint, User> Connections = new Dictionary<IPEndPoint, User>();
    private static int counter;

    static void Main(string[] args)
    {
        Console.WriteLine("Server is Runing!");
        var clientReceiver = new UdpClient(1234);
        var address = IPAddress.Parse("127.0.0.1");
        var ip = new IPEndPoint(address, 1234);
        var clientSend = new UdpClient();


        while (true)
        {
            try
            {
                byte[] bin = clientReceiver.Receive(ref ip);
                string json = Encoding.UTF8.GetString(bin);
                var frame = JsonSerializer.Deserialize<Frame>(json);

                if (Connections.TryGetValue(ip, out var existUser))
                {
                    existUser.LastHeartbeat = DateTime.Now;
                }
                else
                {
                    var user = new User
                    {
                        Id = frame.AuthorId,
                        Name = frame.AuthorName,
                        LastHeartbeat = DateTime.Now,
                    };
                    Connections.Add(ip, user);
                    counter++;

                    // сообщить всем участникам, что подключился новый пользователь

                }

                if (frame.IsHeartbeat == true)
                {
                    continue;
                }

                foreach (var item in Connections.Keys)
                {
                    if (item.Equals(ip))
                    {
                        continue;
                    }

                    clientSend.Send(bin, bin.Length, item);
                }

                string message = Encoding.UTF8.GetString(bin);
                Console.WriteLine(message);
            }
            catch (Exception)
            {
                Console.WriteLine("Возникла непредвиденная ошибка, но сервер продолжает работу");
            }
        }
    }
}
