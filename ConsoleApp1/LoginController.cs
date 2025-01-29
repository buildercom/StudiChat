using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1;

internal class LoginController
{
    public void Process()
    {
        int countfiles = 0;
        while (true)
        {
            Console.WriteLine("Войдите в систему");
            Console.Write("Login: ");
            string login = Console.ReadLine();

            Console.Write("Password: ");
            string password = Console.ReadLine();
            bool access = CheckLoginPassword(login, password);
            if (access)
            {
                break;
            }
            else
            {
                Console.WriteLine("error");
                countfiles++;
                if (countfiles == 3)
                {
                    Console.WriteLine("you hacker");
                    Console.ReadLine();
                    return;
                }
            }

        }

        Console.WriteLine("Welcome!");
        Console.ReadLine();
    }

    private bool CheckLoginPassword(string login, string password)
    {
        foreach(var item in DataBase.Users)
        {
            if (item.Password == password && item.Login == login)
            {
                return true;
            }
        }
        return false;
    }
}

