using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleApp1.Models;

namespace ConsoleApp1;

internal class RegisterController
{
    public void Process()
    {
        Console.WriteLine("Запущена регистрация!");

        Console.WriteLine("Придумайте логин:");
        string userLogin = Console.ReadLine();

        bool isFree = CheckLoginFree(userLogin);
        if (isFree == false)
        {
            Console.WriteLine("Такой логин уже существует!");
            return;
        }
        
        Console.WriteLine("Придумайте пароль:");
        string password = Console.ReadLine();

        Console.WriteLine("Как вас зовут?");
        string name = Console.ReadLine();

        var user = new User();
        user.Name = name;
        user.Password = password;
        user.Login = userLogin;

        DataBase.Users.Add(user);
        Console.WriteLine("Спасибо за регистрацию!");

    }

    private bool CheckLoginFree(string login)
    {
        foreach (var user in DataBase.Users)
        {
            if (user.Login == login)
            {
                return false;
            }
           
        }
        return true;

    }
}
