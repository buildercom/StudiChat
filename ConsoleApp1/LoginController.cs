using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleApp1.Models;

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
            var match = CheckLoginPassword(login, password);
            if (match != null)
            {
                loginUser = match;
                break;
            }
            else
            {
                Console.WriteLine("Error");
                countfiles++;
                if (countfiles == 3)
                {
                    Console.WriteLine("you hacker");
                    Console.ReadLine();
                    return false;
                }
            }

        }


        Console.WriteLine("Welcome "+ loginUser.Name+"!");

        return true;
    }

    private User CheckLoginPassword(string login, string password)
    {
        foreach(var item in DataBase.Users)
        {
            if (item.Password == password && item.Login == login)
            {
                return item;
            }
        }
        return null;
    }
}

