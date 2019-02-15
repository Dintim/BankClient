using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KKB.BankKlient.BLL.Account
{
    class ServiceAccount
    {
        Random rnd = new Random();
        public Account CreateAccount(User.User user, currency currency)
        {
            Account account = new Account();
            account.CreateDate = DateTime.Now.AddMonths(rnd.Next(1, 12)*-1);
            account.Number = currency.ToString().ToUpper() + rnd.Next();
            account.Balance = 0;
            account.UserId = user.Id;
            account.Currency = currency;
            return account;
        }
        public bool CreateAccount(Account account, out string message)
        {
            try
            {
                using (var db = new LiteDatabase(@"kkb.db"))
                {
                    var accounts = db.GetCollection<Account>("Accounts");
                    accounts.Insert(account);
                }
                message = "Создание счета прошло успешно";
                return true;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return false;
            }
        }

    }
}
