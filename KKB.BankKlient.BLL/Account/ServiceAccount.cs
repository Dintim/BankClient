using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using n = NLog;

namespace KKB.BankKlient.BLL.Account
{
    public class ServiceAccount
    {
        private static n.Logger logger = n.LogManager.GetCurrentClassLogger();
        Random rnd = new Random();
        public Account CreateAccount(User.User user, currency cur)
        {
            Account account = new Account();
            account.CreateDate = DateTime.Now.AddMonths(rnd.Next(1, 12) * -1);
            account.Number = cur.ToString().ToUpper() + rnd.Next();
            account.Balance = 0;
            account.UserId = user.Id;
            account.Currency = cur;
            return account;
        }
        public bool CreateAccountDb(Account account, out string message)
        {
            try
            {
                using (var db = new LiteDatabase(@"kkb.db"))
                {
                    var accounts = db.GetCollection<Account>("Account");

                    accounts.Insert(account);
                }

                message = string.Format("Создание счета {0} прошло успешно", account.Number);
                return true;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return false;
            }
        }

        public static List<Account> GetAccountsByUserId(int id)
        {
            try
            {
                using (var db = new LiteDatabase(@"kkb.db"))
                {
                    var accounts = db.GetCollection<Account>("Account");

                    //return accounts.FindAll().ToList();
                    return accounts.Find(x => x.UserId.Equals(id)).ToList();

                }
            }
            catch (Exception ex)
            {
                logger.Error("Get account by user Id has error: " + ex.Message);
                return null;
            }
        }

        public static bool DeleteAccountDb(int AccId, out string message)
        {

            try
            {
                using (var db = new LiteDatabase(@"kkb.db"))
                {
                    var accounts = db.GetCollection<Account>("Account");
                    accounts.Delete(x => x.Id.Equals(AccId));
                }
                message = string.Format("Счет {0} удален", AccId);
                return true;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return false;
                throw;
            }
        }

        public static bool UpdateAccountBalaceDB(Account acc, int sum, out string message)
        {
            try
            {
                using (var db = new LiteDatabase(@"kkb.db"))
                {
                    var accounts = db.GetCollection<Account>("Account");
                    acc.Balance += sum;
                    accounts.Update(acc);
                    if (sum >= 0)
                        message = string.Format("Баланс счета {0} пополнен на {1} {2}", acc.Number, sum, acc.Currency);
                    else
                        message = string.Format("Баланс счета {0} уменьшен на {1} {2}", acc.Number, sum * (-1), acc.Currency);
                    return true;
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return false;
            }
        }
    }
}
