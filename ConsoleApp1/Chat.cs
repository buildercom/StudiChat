using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ConsoleApp1.Models;

namespace ConsoleApp1;

public class Chat
{
    private User loginedUser;
    private UdpClient client;

    public Chat(User user)
    {
        loginedUser = user;
        client = new UdpClient(1235);
    }

    public void Process()
    {
        var tread = new Thread(Engine);
        tread.Start();

        var tread2 = new Thread(EngineHeartbeat);
        tread2.Start();

        while (true)
        {
            string input = Console.ReadLine();
            switch (input)
            {
                case "/exit":
                    return;
                default:
                    var frame = new Frame
                    {
                        AuthorName = loginedUser.Name,
                        AuthorId = loginedUser.Id,
                        ReceiverId = 0,
                        Message = input,
                        IsHeartbeat = false,
                    };
                    string json = JsonSerializer.Serialize(frame);
                    byte[] bin = Encoding.UTF8.GetBytes(json);
                    var ep = IPEndPoint.Parse("192.168.1.191:1234");
                    client.Send(bin, bin.Length, ep);
                    break;
            }
        }
    }

    private void Engine()
    {
        //var ip = IPEndPoint.Parse("127.0.0.1");
        //var ip = new IPEndPoint();
        var ip = new IPEndPoint(IPAddress.Any, 0);

        while (true)
        {
            byte[] bin = client.Receive(ref ip);
            string json = Encoding.UTF8.GetString(bin);
            var jsonFrame = JsonSerializer.Deserialize<Frame>(json);

            string authorName = jsonFrame.AuthorName;
            
            Console.WriteLine("Пользователь "+ authorName+":"+ jsonFrame.Message);
        }
    }

    private void EngineHeartbeat()
    {
        while (true)
        {
            var frame = new Frame
            {
                AuthorName = loginedUser.Name,
                AuthorId = loginedUser.Id,
                ReceiverId = 0,
                IsHeartbeat = true,
                Message = null,
            };
            string json = JsonSerializer.Serialize(frame);
            byte[] jsonBytes = Encoding.UTF8.GetBytes(json);
            var ep = IPEndPoint.Parse("192.168.1.191:1234");
            client.Send(jsonBytes, jsonBytes.Length, ep);
            Thread.Sleep(500);
        }
    }
}
