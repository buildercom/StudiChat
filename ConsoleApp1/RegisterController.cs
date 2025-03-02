using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ConsoleApp1.Models;
using ConsoleApp1.Models.Dto;

namespace ConsoleApp1;

internal class RegisterController
{
    public void Process()
    {
        Console.WriteLine("Запущена регистрация!");

        Console.WriteLine("Придумайте логин:");
        string userLogin = Console.ReadLine();

        Console.WriteLine("Придумайте пароль:");
        string password = Console.ReadLine();

        Console.WriteLine("Как вас зовут?");
        string name = Console.ReadLine();

        UserRegister(userLogin, password, name);

        Console.WriteLine("Спасибо за регистрацию!");
    }

    public User UserRegister( string userLogin, string password, string name)
    {
        var client = new HttpClient();
        var request = new HttpRequestMessage(HttpMethod.Post, "http://localhost:5227/login/register");
        var requestModel = new
        {
            Login = userLogin,
            Password = password,
            Name = name,
        };
        string json = JsonSerializer.Serialize(requestModel);
        var content = new StringContent(json, null, "application/json");
        request.Content = content;
        var response = client.Send(request);

        string responseJson = response.Content.ReadAsStringAsync().Result;
        var result = JsonSerializer.Deserialize<LoginResponce>(responseJson);
        if (result.Success == true)
        {
            return new User
            {
                Id = result.Result.Id,
                Name = result.Result.Name,
                Password = password,
                Login = userLogin,
            };
        }
        else
        {
            Console.WriteLine(result.Description);
            return null;
        }
    }
}
