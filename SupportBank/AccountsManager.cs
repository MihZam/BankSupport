using System;
using System.Collections.Generic;

namespace SupportBank
{
    class AccountsManager
    {
        private List<string> names = new List<string>();
        private List<BankAccount> accounts = new List<BankAccount>();
        private int lastUpdate = 0;

        public List<BankAccount> getAccounts()
        {
            return accounts;
        }

        public void addAccount(string accName)
        {
            if (!names.Contains(accName))
            {
                names.Add(accName);
                var newAccount = new BankAccount();
                newAccount.setName(accName);
                accounts.Add(newAccount);
            }
        }

        private BankAccount accessAccount(string name)
        {
            BankAccount result = new BankAccount();
            foreach (var ba in accounts)
            {
                if (ba.getName() == name)
                {
                    result = ba;
                }
            }

            return result;
        }

        public void applyTransactions(List<string> listFrom, List<string> listTo, List<string> listAmount)
        {
            for (var i = lastUpdate; i < listFrom.Count; i++)
            {
                accessAccount(listFrom[i]).Pay(Convert.ToDouble(listAmount[i]), accessAccount(listTo[i]));
            }

            lastUpdate = listFrom.Count;
        }
    }
}