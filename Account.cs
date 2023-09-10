using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using System.Transactions;


namespace cSharp_BankSystem
{
    public class Account
    {
        public int AccountNumber { get; set; }
        public string AccountHolderName { get; set; }
        public decimal Balance { get; set; }
        public List<Transaction> TransactionHistory { get; private set; }


        public Account()
        {
            TransactionHistory = new List<Transaction>();
        }

        public Account(int accountNumber, string accountHolderName, decimal balance)
        {
            AccountNumber = accountNumber;
            AccountHolderName = accountHolderName;
            Balance = balance;
            TransactionHistory = new List<Transaction>();
        }
    }
}
