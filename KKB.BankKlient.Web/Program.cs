using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KKB.BankKlient.BLL.User;
using KKB.BankKlient.BLL.User.Account;
using KKB.BankKlient.Web.Model;

namespace KKB.BankKlient.Web
{
    class Program
    {
        static void Main(string[] args)
        {
            //ServiceUser service = new ServiceUser();
            //User user = new User();

            //RandomUser.GenerateUser generate = new RandomUser.GenerateUser();

            //var rUser = generate.GetUser();
            //user.LastName = rUser.name.title;
            //user.FirstName = rUser.name.first;
            //user.Login = "admin";
            //user.Password = "admin";

            //string message = "";
            //if (service.RegisterUser(user, out message))
            //{
            //    Console.ForegroundColor = ConsoleColor.Green;
            //    Console.WriteLine(message);
            //    Console.ForegroundColor = ConsoleColor.White;
            //}
            //else
            //{
            //    Console.ForegroundColor = ConsoleColor.Red;
            //    Console.WriteLine(message);
            //    Console.ForegroundColor = ConsoleColor.White;
            //}

            ServiceMenu.MainMenu();
        }
    }
}
