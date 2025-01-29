using System;

namespace ConsoleApp1;

internal class Program
{
    static void Main(string[] args)
    {
        while (true)
        {
            Console.WriteLine("Введите команду:");
            string command = Console.ReadLine();
            switch (command)
            {
                case "login":
                    var loginController = new LoginController();
                    loginController.Process();
                    break;
                case "register":
                    var regController = new RegisterController();
                    regController.Process();

                    break;
                default:
                    Console.WriteLine("Неизвестная команда");
                    break;
            }

        }
        Console.ReadLine();
    }

}
