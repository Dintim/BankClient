using KKB.BankKlient.BLL.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KKB.BankKlient.BLL.User.Account;
using System.Threading;
using KKB.BankKlient.BLL.Account;
using n = NLog;

namespace KKB.BankKlient.Web.Model
{

    public class ServiceMenu
    {
        private static User AuthorUser = null;
        private static ServiceUser service = null;
        private static n.Logger logger = n.LogManager.GetCurrentClassLogger();

        static ServiceMenu()
        {
            service = new ServiceUser();
        }

        public static void MainMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Добро пожаловать в KKB!\n");
                Console.WriteLine("1. Регистрация");
                Console.WriteLine("2. Вход");
                Console.WriteLine("3. Выход");

                Console.Write(": ");
                int menu = Int32.Parse(Console.ReadLine());

                if (menu == 1)
                    RegisterMenu();
                else if (menu == 2)
                    LogOnMenu();
                else
                    break;
            }
        }

        /// <summary>
        /// Регистрация пользователя
        /// </summary>
        public static void RegisterMenu()
        {
            Console.Clear();
            Console.WriteLine("Форма регистрации пользователя\n");

            User user = new User();

            Console.Write("FirstName: ");
            user.FirstName = Console.ReadLine();

            Console.Write("LastName: ");
            user.LastName = Console.ReadLine();

            Console.Write("Login: ");
            user.Login = Console.ReadLine();

            Console.Write("Password: ");
            user.Password = Console.ReadLine();

            string message = "";
            if (service.RegisterUser(user, out message))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(message);
                Console.ForegroundColor = ConsoleColor.White;
                //Thread.Sleep(3000);
                //MainMenu();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(message);
                Console.ForegroundColor = ConsoleColor.White;
            }
            Thread.Sleep(3000);
        }

        /// <summary>
        /// Проверка наличия пользователя в системе 
        /// </summary>
        public static void LogOnMenu()
        {
            Console.Clear();
            Console.WriteLine("");

            Console.Write("Login: ");
            string login = Console.ReadLine();
            logger.Info("Login: " + login);

            Console.Write("Password: ");
            string password = Console.ReadLine();
            logger.Info("Password: " + password);
            string message = "";
            User user = service.LogOn(login, password,
                                       out message);

            if (user != null)
            {
                AuthorUser = user;
                AuthorUser.Accounts = ServiceAccount.GetAccountsByUserId(AuthorUser.Id);
                AuthorizeUserMenu();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(message);
                Console.ForegroundColor = ConsoleColor.White;
                Thread.Sleep(3000);
                //LogOnMenu();
            }
            //Thread.Sleep(3000);
        }

        /// <summary>
        /// Вход авторизованного пользователя в пользовательское меню
        /// </summary>
        public static void AuthorizeUserMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Приветствуем Вас, {0} {1}\n",
                    AuthorUser.FirstName, AuthorUser.LastName);
                Console.WriteLine("1. Вывод списка счетов на экран"); //+
                Console.WriteLine("2. Пополнение счета"); //+
                Console.WriteLine("3. Снять деньги со счета"); //+
                Console.WriteLine("4. Создать счет"); //+
                Console.WriteLine("5. Выход"); //+
                Console.Write(": ");
                int menu = Int32.Parse(Console.ReadLine());
                if (menu == 5)
                    break;
                else if (menu == 1)
                    PrintBalanceOnScreen();
                else if (menu == 2)
                    AddMoneyToAccount();
                else if (menu == 3)
                    WithdrawMoneyFromAccount();
                else if (menu == 4)
                    CreateAccountMenu();

                Thread.Sleep(3000);
            }

        }
        /// <summary>
        /// Вывод списка счетов на экран
        /// </summary>
        public static void PrintBalanceOnScreen()
        {
            Console.Clear();
            if (AuthorUser.Accounts.Count != 0)
            {
                Console.WriteLine("Ваши счета, {0} {1}",
                AuthorUser.FirstName, AuthorUser.LastName);
                Console.WriteLine("\n-----------------------------------\n");
                Console.WriteLine("ID\tНомер\t\tДата создания\tВалюта\tСумма");
                for (int i = 0; i < AuthorUser.Accounts.Count; i++)
                {
                    Console.WriteLine("{0}\t{1}\t{2: dd.MM.yyyy}\t{3}\t{4}",
                        AuthorUser.Accounts[i].Id, AuthorUser.Accounts[i].Number, AuthorUser.Accounts[i].CreateDate,
                        AuthorUser.Accounts[i].Currency, AuthorUser.Accounts[i].Balance);
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("У вас нет активных счетов");
                Console.ForegroundColor = ConsoleColor.White;
            }
        }

        /// <summary>
        /// Создать счет
        /// </summary>
        public static void CreateAccountMenu()
        {
            Console.Clear();
            ServiceAccount serviceAcc = new ServiceAccount();
            Account acc = null;
            Console.WriteLine("Выберите валюту счета:");
            Console.WriteLine("1. KZT\n2. USD\n3. RUR");
            Console.Write(": ");
            int ch = Int32.Parse(Console.ReadLine());
            if (ch == 1)
                acc = serviceAcc.CreateAccount(AuthorUser, currency.kzt);
            else if (ch == 2)
                acc = serviceAcc.CreateAccount(AuthorUser, currency.usd);
            else
                acc = serviceAcc.CreateAccount(AuthorUser, currency.rur);
            string message = "";
            if (serviceAcc.CreateAccountDb(acc, out message))
            {
                Console.ForegroundColor = ConsoleColor.Green;

                Console.WriteLine(message);
                Console.ForegroundColor = ConsoleColor.White;

                AuthorUser.Accounts = ServiceAccount.GetAccountsByUserId(AuthorUser.Id);
                //AuthorizeUserMenu();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(message);
                Console.ForegroundColor = ConsoleColor.White;
            }            
        }

        /// <summary>
        /// Пополнение счета
        /// </summary>
        public static void AddMoneyToAccount()
        {
            PrintBalanceOnScreen();
            Console.WriteLine("\n-----------------------------------\n");
            Console.Write("Выберите счет (id): ");
            int accId = Int32.Parse(Console.ReadLine());
            Console.Write("Введите сумму пополнения (в валюте счета): ");
            int sum = Int32.Parse(Console.ReadLine());

            Account acc = null;
            for (int i = 0; i < AuthorUser.Accounts.Count; i++)
            {
                if (AuthorUser.Accounts[i].Id == accId)
                    acc = AuthorUser.Accounts[i];
            }

            string message = "";
            ServiceAccount.UpdateAccountBalaceDB(acc, sum, out message);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.White;
        }

        /// <summary>
        /// Снять деньги со счета
        /// </summary>
        public static void WithdrawMoneyFromAccount()
        {
            PrintBalanceOnScreen();
            Console.WriteLine("\n-----------------------------------\n");
            Console.Write("Выберите счет (id): ");
            int accId = Int32.Parse(Console.ReadLine());
            Console.Write("Введите сумму снятия (в валюте счета): ");
            int sum = Int32.Parse(Console.ReadLine());

            Account acc = null;
            for (int i = 0; i < AuthorUser.Accounts.Count; i++)
            {
                if (AuthorUser.Accounts[i].Id == accId)
                    acc = AuthorUser.Accounts[i];
            }
            if (acc.Balance >= sum)
            {
                sum = sum * (-1);
                string message = "";
                ServiceAccount.UpdateAccountBalaceDB(acc, sum, out message);
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine(message);
                Console.ForegroundColor = ConsoleColor.White;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Суммы на cчете {0} недостаточно для такой операции", acc.Number);
                Console.ForegroundColor = ConsoleColor.White;
            }
        }

    }
}
