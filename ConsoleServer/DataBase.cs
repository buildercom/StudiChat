using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ConsoleServer.Models;

namespace ConsoleServer;

public static class DataBase
{
    public static List<User> Users = new List<User>();

    public static void Init()
    {
        string dir = Environment.CurrentDirectory;
        string path = Path.Combine(dir, "db.json");
        bool isexist = File.Exists(path);
        if (isexist == true)
        {
            string data = File.ReadAllText(path);
            var users = JsonSerializer.Deserialize<List<User>>(data);
            Users = users;
        }
        else
        {
            string dataJson = JsonSerializer.Serialize(Users);
            File.WriteAllText(path, dataJson);
        }
    }

    public static void Save()
    {
        string dir = Environment.CurrentDirectory;
        string path = Path.Combine(dir, "db.json");
        bool isexist = File.Exists(path);
        if (isexist == true)
        {
            File.Delete(path);
            string dataJson = JsonSerializer.Serialize(Users);
            File.WriteAllText(path, dataJson);
        }
        else
        {
            string dataJson = JsonSerializer.Serialize(Users);
            File.WriteAllText(path, dataJson);
        }
    }

}
