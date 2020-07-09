using System;
using System.Data;

namespace SupportBank
{
    class UserInput
    {
        private string input;
        private string[] inputArray;
        private string inputName;
        
        public void askUser(AccountsManager AM, DataManager DM)
        {
            Console.WriteLine("What would you like to do?");
            input = Console.ReadLine();
            inputArray = input.Split(' ');
            inputName = new string("");
            if (inputArray.Length == 3 && inputArray[0] == "List")
            {
                inputName = inputArray[1] + ' ' + inputArray[2];
            }
            else if (inputArray.Length == 3 && inputArray[0] + " " + inputArray[1] == "Import File")
            {
                inputName = inputArray[2];
            }
            else if (inputArray.Length == 2)
            {
                inputName = inputArray[1];
            }
            
            if (input == "List All")
            {
                listAll(AM);
            }
            else if (inputArray[0] == "List")
            {
                listName(DM);
            }
            else if (inputArray[0] + " " + inputArray[1] == "Import File")
            {
                importFile(DM);
                foreach (var name in DM.getFrom())
                {
                    AM.addAccount(name);
                }
                AM.applyTransactions(DM.getFrom(), DM.getTo(), DM.getAmount());
            }
        }

        private void listAll(AccountsManager AM)
        {
            var accounts = AM.getAccounts();
            foreach (var account in accounts)
            {
                if (account.getBalance() > 0)
                {
                    Console.WriteLine($"{account.getName()}: is owed {account.getBalance()}");
                }
                else if (account.getBalance() == 0)
                {
                    Console.WriteLine($"{account.getName()}: neither owes or is being owed anything");
                }
                else
                {
                    Console.WriteLine($"{account.getName()}: owes {-account.getBalance()}");
                }
            }
        }
        
        private void listName (DataManager DM)
        {
            for (var i = 0; i < DM.getFrom().Count; i++)
            {
                if (DM.getFrom()[i] == inputName || DM.getTo()[i] == inputName)
                {
                    Console.WriteLine(
                        $"On {DM.getDate()[i]}, {DM.getFrom()[i]} paid {DM.getTo()[i]} {DM.getAmount()[i]} for {DM.getNarrative()[i]}");
                }
            }
        }

        private void importFile(DataManager DM)
        {
            var fileType = inputName.Split('.')[inputName.Split('.').Length - 1];
            if (fileType == "csv")
            {
                DM.readDataCSV(inputName);
            }
            else if (fileType == "json")
            {
                DM.readDataJSON(inputName);
            }
        }

        // Part of the stretch goal, would go into different class
        private DataTable createDataTable(DataManager DM)
        {
            var table = new DataTable();

            table.Columns.Add("Date", typeof(string));
            table.Columns.Add("From", typeof(string));
            table.Columns.Add("To", typeof(string));
            table.Columns.Add("Narrative", typeof(string));
            table.Columns.Add("Amount", typeof(string));

            for (var i = 0; i < DM.getFrom().Count; i++)
            {
                table.Rows.Add(DM.getDate()[i], DM.getFrom()[i], DM.getTo()[i], DM.getNarrative()[i],
                    DM.getAmount()[1]);
            }

            return table;
        }
    }
}