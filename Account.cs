using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Text.Json.Serialization;


namespace cSharp_BankSystem
{
    public class Account
    {
        public int AccountNumber { get; set; }
        public string AccountHolderName { get; set; }
        public decimal Balance { get; set; }
        public List<Transaction> TransactionHistory { get; private set; }


        public Account(int accountNumber, string accountHolderName, decimal initialBalance)
        {
            AccountNumber = accountNumber;
            AccountHolderName = accountHolderName;
            Balance = initialBalance;
            TransactionHistory = new List<Transaction>();
        }
        public string ToJson()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
