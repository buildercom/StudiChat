using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ConsoleApp1.Models;
using ConsoleApp1.Models.Dto;

namespace ConsoleApp1;

internal class LoginController
{
    public User loginUser;

    public bool Process()
    {
        int countfiles = 0;
        while (true)
        {
            Console.WriteLine("Войдите в систему");
            Console.Write("Login: ");
            string login = Console.ReadLine();

            Console.Write("Password: ");
            string password = Console.ReadLine();
            var match = Authorization(login, password);
            if (match != null)
            {
                loginUser = match;
                break;
            }
            else
            {
                countfiles++;
                if (countfiles == 3)
                {
                    Console.WriteLine("you hacker");
                    Console.ReadLine();
                    return false;
                }
            }

        }


        Console.WriteLine("Welcome " + loginUser.Name + "!");
        return true;
    }

    private User Authorization(string login, string password)
    {
        var client = new HttpClient();
        var request = new HttpRequestMessage(HttpMethod.Post, "http://localhost:5227/login");
        var requestModel = new
        {
            Login = login,
            Password = password,
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
                Login = login,
            };
        }
        else
        {
            Console.WriteLine(result.Description);
            return null;
        }
    }
}

