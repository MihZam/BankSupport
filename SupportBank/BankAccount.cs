namespace SupportBank
{
    class BankAccount
    {
        private double balance = 0;
        private string name;

        public void Pay(double amount, BankAccount receiver)
        {
            this.balance -= amount;
            receiver.balance += amount;
        }

        public string getName()
        {
            return name;
        }

        public void setName(string _name)
        {
            name = _name;
        }

        public double getBalance()
        {
            return balance;
        }
    }
}