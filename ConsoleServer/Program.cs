using System.Net.Sockets;
using System.Net;
using System.Text;
using ConsoleServer.Models;
using System.Text.Json;

namespace ConsoleServer;

internal class Program
{
    private static Dictionary<IPEndPoint, User> Connections = new Dictionary<IPEndPoint, User>();
    private static UdpClient clientSend;

    static void Main(string[] args)
    {
        DataBase.Init();
        Console.WriteLine("Server is Runing!");
        var clientReceiver = new UdpClient(1234);
        var address = IPAddress.Parse("127.0.0.1");
        var ip = new IPEndPoint(address, 1234);
        clientSend = new UdpClient();

        var tread = new Thread(Engine);
        tread.Start();


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

                    // сообщить всем участникам, что подключился новый пользователь
                    var frameSend = new Frame
                    {
                        AuthorId = 0,
                        AuthorName = "Система",
                        IsHeartbeat = false,
                        Message = "Пользователь " + user.Name + " присоединился к чату!",
                        ReceiverId = 0
                    };
                    string frameSendJson = JsonSerializer.Serialize(frameSend);
                    byte[] a = Encoding.UTF8.GetBytes(frameSendJson);

                    Console.WriteLine("Пользователь " + user.Name + " присоединился к чату!");

                    foreach (var item in Connections.Keys)
                    {
                        if (item.Equals(ip))
                        {
                            continue;
                        }
                        clientSend.Send(a, a.Length, item);
                    }
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

    public static void Engine()
    {
        while (true)
        {
            var list = new List<IPEndPoint>();
            foreach (var item in Connections)
            {
                var delta = DateTime.Now - item.Value.LastHeartbeat;
                if (delta.TotalSeconds > 2)
                {
                    Console.WriteLine("Пользователь " + item.Value.Name + " отключился!");
                    SendOut("Пользователь " + item.Value.Name + " отключился!");

                    list.Add(item.Key);
                    //Connections.Remove(item.Key);
                }
            }
            foreach (var item in list)
            {
                Connections.Remove(item);
            }
            Thread.Sleep(300);
        }
    }

    public static void SendOut(string message)
    {
        var frameSend = new Frame
        {
            AuthorId = 0,
            AuthorName = "Система",
            IsHeartbeat = false,
            Message = message,
            ReceiverId = 0
        };
        string frameSendJson = JsonSerializer.Serialize(frameSend);
        byte[] a = Encoding.UTF8.GetBytes(frameSendJson);

        foreach (var item in Connections.Keys)
        {
            clientSend.Send(a, a.Length, item);
        }
    }
}
