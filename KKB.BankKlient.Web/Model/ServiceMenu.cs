using KKB.BankKlient.BLL.User;
using KKB.BankKlient.BLL.User.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KKB.BankKlient.Web.Model
{
    public class ServiceMenu
    {
        private static ServiceUser service=null;
        static ServiceMenu()
        {
            service = new ServiceUser();
        }
        public static void MainMenu()
        {
            Console.WriteLine("Добро пожаловать в ККБ\n");
            Console.WriteLine("1. Регистрация");
            Console.WriteLine("2. Вход");

            Console.WriteLine(": ");
            int menu = Int32.Parse(Console.ReadLine());

            if (menu==1)
            {
                RegisterMenu();
            }
            else if (menu==2)
            {
                LogOnMenu();
            }

        }
        public static void AuthorizeUserMenu(User user)
        {
            Console.Clear();
            Console.WriteLine("Приветствуем вас, {0} {1}", user.FirstName, user.LastName);
            Console.WriteLine("1. Вывод баланса на экран");
            Console.WriteLine("2. Пополнение счета");
            Console.WriteLine("3. Снять деньги о счета");
            Console.WriteLine("4. Выход");

            Console.WriteLine(": ");
            int menu = Int32.Parse(Console.ReadLine());
        }

        public static void RegisterMenu()
        {
            Console.Clear();
            Console.WriteLine("Регистрация пользователя:");

            User user = new User();
            Console.Write("First name = ");
            user.FirstName = Console.ReadLine();

            Console.Write("Last name = ");
            user.LastName = Console.ReadLine();

            Console.Write("Login = ");
            user.Login = Console.ReadLine();

            Console.Write("Password = ");
            user.Password = Console.ReadLine();

            
            string message = "";
            if (service.RegisterUser(user, out message))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(message);
                Console.ForegroundColor = ConsoleColor.White;
                Thread.Sleep(3000);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(message);
                Console.ForegroundColor = ConsoleColor.White;
            }
        }

        public static void LogOnMenu()
        {
            Console.Clear();
            Console.WriteLine("");
            Console.Write("Login: ");
            string login = Console.ReadLine();

            Console.Write("Password: ");
            string password = Console.ReadLine();

            
            string message = "";
            User user = service.LogOn(login, password, out message);

            if (user!=null)
            {
                AuthorizeUserMenu(user);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(message);
                Console.ForegroundColor = ConsoleColor.White;

                Thread.Sleep(3000);
                LogOnMenu();
            }
        }
    }
}
