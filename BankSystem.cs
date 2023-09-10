using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;
using System.Text.Json.Serialization;
using System.Net.Http.Json;

namespace cSharp_BankSystem
{
    internal class BankSystem
    {
        private List<User> users;
        private List<Account> accounts;
        private User loggedInUser;
        private string userDataDirectory = "Bank System";
        private string userDataFile = "Bank System/users.json"; // Folder to store user data called 'Bank System'
        private readonly HttpClient httpClient = new HttpClient();
        private const string ExchangeRateApiUrl = "https://v6.exchangerate-api.com/v6/2d8754c1bf6d68b8bbea954d/latest/USD";
        public BankSystem()
        {
            users = new List<User>();
            accounts = new List<Account>();
        }

        public bool RegisterUser(string name, string email, string password)
        {
            // Check if a user with the given email already exists.
            if (users.Any(u => u.Email == email))
            {
                return false; // User with this email already exists.
            }

            User newUser = new User(name, email, password);
            users.Add(newUser);
            SaveUserData();
            return true; // Registration successful.
        }
        static async Task ViewExchangeRates()
        {
            Console.WriteLine("Exchange Rates:");
            return;
            //try
            //{
            //    HttpResponseMessage response = await httpClient.GetAsync(ExchangeRateApiUrl);

            //    if (response.IsSuccessStatusCode)
            //    {
            //        string responseBody = await response.Content.ReadAsStringAsync();

            //        string exchangeRateData = JsonConvert.DeserializeObject<ExchangeRateData>(responseBody);

            //        Console.WriteLine("Exchange Rates (USD as base currency):");
            //        foreach (var currency in exchangeRateData.Rates)
            //        {
            //            Console.WriteLine($"{currency.Key}: {currency.Value}");
            //        }
            //    }
            //}
            //catch (HttpRequestException)
            //{
            //    Console.WriteLine("Failed to retrieve exchange rates. Please try again later.");
            //    return;
            //}
        }
        public bool Login(string email, string password)
        {
            // Find the user with the provided email.
            loggedInUser = users.FirstOrDefault(u => u.Email == email);

            // Check if the user exists and the password matches (you should compare hashed passwords here).
            if (loggedInUser != null && loggedInUser.HashedPassword == password)
            {
                return true; // Login successful.
            }

            loggedInUser = null; // Reset logged-in user on failed login.
            return false; // Login failed.
        }

        public void HandleLoggedInUser()
        {
            if (loggedInUser == null)
            {
                Console.WriteLine("You must log in first.");
                return;
            }

            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("1. Create Bank Account");
                Console.WriteLine("2. My Bank Accounts");
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\n$ $$ Operations $$ $\n");
                Console.ResetColor();

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("3. Deposit");
                Console.WriteLine("4. Withdraw");
                Console.WriteLine("5. Check Balance");
                Console.WriteLine("6. Transfer Money");
                Console.WriteLine("7. Exchange Rates");
                Console.WriteLine("8. Account history");
                Console.ResetColor();
                Console.WriteLine("9. Logout");
                Console.Write("Select an option: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        CreateAccount();
                        break;
                    case "2":
                        UserAccounts();
                        break;
                    case "3":
                        Deposit();
                        break;
                    case "4":
                        Withdraw();
                        break;
                    case "5":
                        CheckBalance();
                        break;
                    case "6":
                        Transfer();
                        break;
                    case "7":
                        ViewExchangeRates();
                        break;
                    case "8":
                        GetAccountHistory();
                        break;
                    case "9":
                        loggedInUser = null; // Logout the user.
                        return;

                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }
        private int GenerateAccountNumber()
        {
            Random random = new Random();
            int accountNumber;
            do
            {
                accountNumber = random.Next(0000, 10000); // To Generate an account number
            } while (accounts.Any(a => a.AccountNumber == accountNumber));

            return accountNumber;
        }
        public void CreateAccount()
        {
            if (loggedInUser == null)
            {
                Console.WriteLine("You must log in first.");
                return;
            }

            Console.Write("Enter initial balance: ");
            if (decimal.TryParse(Console.ReadLine(), out decimal initialBalance))
            {
                string accountHolderName = loggedInUser.Name;
                int accountNumber = GenerateAccountNumber();
                Account newAccount = new Account(accountNumber, accountHolderName, initialBalance);
                loggedInUser.Accounts.Add(newAccount);
                accounts.Add(newAccount);
                SaveUserData(); // Save the updated user data to the JSON file.
                Console.WriteLine($"Account created with account number: {accountNumber}");
                return;
            }
            else
            {
                Console.WriteLine("Invalid initial balance.");
                return;
            }

        }

        public void UserAccounts()
        {
            Console.WriteLine($"Accounts for user: {loggedInUser.Name}");
            foreach (var account in loggedInUser.Accounts)
            {
                Console.WriteLine($"Account Number: {account.AccountNumber}");
                Console.WriteLine($"Account Holder: {account.AccountHolderName}");
                Console.WriteLine($"Current Balance: {account.Balance} OMR");
                Console.WriteLine();
                return;
            }
        }

        public void Deposit()
        {
            if (loggedInUser == null)
            {
                Console.WriteLine("You must log in first.");
                return;
            }
            Console.Write("Enter the account number to deposit into: ");
            if (!int.TryParse(Console.ReadLine(), out int accountNumber))
            {
                Console.WriteLine("Invalid account number.");
                return;
            }
            Account account = GetAccountByNumber(accountNumber);

            if (account == null)
            {
                Console.WriteLine("Account not found.");
                return;
            }

            Console.Write("Enter the amount to deposit: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal amount) || amount <= 0)
            {
                Console.WriteLine("Invalid deposit amount.");
                return;
            }

            account.Balance += amount;
            SaveUserData(); // Save the updated user data to the JSON file
            Console.WriteLine($"Deposited {amount} OMR into account {accountNumber}. New balance: {account.Balance} OMR");
            account.TransactionHistory.Add(new Transaction
            {
                TransactionType = "Deposit",
                Amount = amount,
                Timestamp = DateTime.Now
            });
            return;
        }
        public void Withdraw()
        {
            if (loggedInUser == null)
            {
                Console.WriteLine("You must log in first.");
                return;
            }
            Console.Write("Enter the account number to withdraw from: ");
            if (!int.TryParse(Console.ReadLine(), out int accountNumber))
            {
                Console.WriteLine("Invalid account number.");
                return;
            }
            Account account = GetAccountByNumber(accountNumber);

            if (account == null)
            {
                Console.WriteLine("Account not found.");
                return;
            }
            Console.Write("Enter the amount to withdraw: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal amount) || amount <= 0)
            {
                Console.WriteLine("Invalid deposit amount.");
                return;
            }
            if (amount <= 0 || amount > account.Balance)
            {
                Console.WriteLine("Invalid withdrawal amount or insufficient funds.");
                return;
            }

            account.Balance -= amount;
            SaveUserData(); // Save the updated user data to the JSON file
            Console.WriteLine($"Withdrawn {amount} OMR from account {accountNumber}. New balance: {account.Balance} OMR");
            account.TransactionHistory.Add(new Transaction
            {
                TransactionType = "Withdrawal",
                Amount = amount,
                Timestamp = DateTime.Now
            });
            return;
        }
        public void CheckBalance()
        {
            if (loggedInUser == null)
            {
                Console.WriteLine("You must log in first.");
                return;
            }
            Console.Write("Enter the account number to check balance from: ");
            if (!int.TryParse(Console.ReadLine(), out int accountNumber))
            {
                Console.WriteLine("Invalid account number.");
            }
            Account account = GetAccountByNumber(accountNumber);

            if (account == null)
            {
                Console.WriteLine("Account not found.");
                return;
            }

            Console.WriteLine($"Account {accountNumber} balance: {account.Balance} OMR");
            return;
        }
        public void Transfer()
        {
            if (loggedInUser == null)
            {
                Console.WriteLine("You must log in first.");
                return;
            }

            Console.Write("Enter the account number want to transfer from: ");
            if (!int.TryParse(Console.ReadLine(), out int sourceAccountNumber))
            {
                Console.WriteLine("Invalid account number.");
                return;
            }

            Account sourceAccount = GetAccountByNumber(sourceAccountNumber);

            if (sourceAccount == null)
            {
                Console.WriteLine("Source account not found.");
                return;
            }

            Console.Write("Enter the account number want to transfer to: ");
            if (!int.TryParse(Console.ReadLine(), out int targetAccountNumber))
            {
                Console.WriteLine("Invalid target account number.");
                return;
            }

            Account targetAccount = GetAccountByNumber(targetAccountNumber);
            
            if (targetAccount == null)
            {
                Console.WriteLine("Target account not found.");
                return;
            }
            Console.WriteLine($"Account holder name:{0}\nAccount number: {1}",targetAccount.AccountHolderName, targetAccount.AccountNumber);
            Console.Write("Enter the amount to transfer: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal amount) || amount <= 0)
            {
                Console.WriteLine("Invalid transfer amount.");
                return;
            }

            if (sourceAccount.Balance < amount)
            {
                Console.WriteLine("Insufficient funds in your account.");
                return;
            }
            sourceAccount.Balance -= amount;
            targetAccount.Balance += amount;

            SaveUserData(); // Save the updated user data to the JSON file

            Console.WriteLine($"Transferred {amount} OMR from account {sourceAccountNumber} to account {targetAccountNumber}.");
            Console.WriteLine($"Source account balance: {sourceAccount.Balance} OMR");
            sourceAccount.TransactionHistory.Add(new Transaction
            {
                TransactionType = "Transfer (To)",
                Amount = amount,
                Timestamp = DateTime.Now
            });

            targetAccount.TransactionHistory.Add(new Transaction
            {
                TransactionType = "Transfer (From)",
                Amount = amount,
                Timestamp = DateTime.Now
            });
            return;
        }
        public void GetAccountHistory()
        {
            if (loggedInUser == null)
            {
                Console.WriteLine("You must log in first.");
                return;
            }
            Console.Write("Enter the account number to show the history: ");
            if (!int.TryParse(Console.ReadLine(), out int accountNumber))
            {
                Console.WriteLine("Invalid account number.");
            }
            Account account = GetAccountByNumber(accountNumber);

            if (account == null)
            {
                Console.WriteLine("Account not found.");
                return;
            }

            Console.WriteLine($"Transaction History for Account {accountNumber}:");
            foreach (var transaction in account.TransactionHistory)
            {
                Console.WriteLine($"Type: {transaction.TransactionType}, Amount: {transaction.Amount} OMR, Date: {transaction.Timestamp}");
            }
        }
        private Account GetAccountByNumber(int accountNumber)
        {
            if (loggedInUser == null)
            {
                return null; 
            }
            return loggedInUser.Accounts.FirstOrDefault(account => account.AccountNumber == accountNumber);
        }
        public void LoadUserData()
        {
            try
            {
                if (!Directory.Exists(userDataDirectory))
                {
                    Directory.CreateDirectory(userDataDirectory);
                }
                if (File.Exists(userDataFile))
                {
                    string userDataJson = File.ReadAllText(userDataFile);
                    users = JsonSerializer.Deserialize<List<User>>(userDataJson);
                }
                else
                {
                    // Create a new user list if the file doesn't exist.
                    users = new List<User>();
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Error loading user data: {ex.Message}");
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Error parsing user data: {ex.Message}");
            }
        }

        private void SaveUserData()
        {
            try
            {
                string userDataJson = JsonSerializer.Serialize(users);
                File.WriteAllText(userDataFile, userDataJson);
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Error saving user data: {ex.Message}");
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Error serializing user data: {ex.Message}");
            }
        }
        
        
    }

}
