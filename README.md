Bank System Development in C#
You are tasked with developing a bank system in C# that simulates the basic operations 
of a bank. The system should allow users to register, login, create bank accounts, perform 
transactions such as deposits, withdrawals, and money transfers, maintain a record of 
account balances, and provide exchange rate information.
Requirements:
1. User Registration:
 - Users should be able to register an account by providing their personal information such as name, email, and 
password.
 - The system should validate and enforce unique email addresses for each user during the registration process.
 - The user's password should be securely stored using appropriate encryption techniques.
2. User Login:
 - Registered users should be able to log in to their account using their email and password.
 - The system should authenticate user credentials and provide access to the user's account.
3. Account Creation:
 - Once logged in, users should be able to create bank accounts.
 - Each account should have a unique account number, account holder name, and an initial balance.
 - The system should validate and enforce unique account numbers for each account.
4. Account Operations:
 - Users should be able to perform the following operations on their accounts:
 - Deposit: Users can deposit a specified amount into their account.
 - Withdrawal: Users can withdraw a specified amount from their account, provided they have sufficient funds.
 - Money Transfer: Users can transfer a specified amount from their account to another user's account, provided 
they have sufficient funds.
 - The system should validate and enforce sufficient funds for withdrawals and money transfers.
5. Account Information:
 - Users should be able to retrieve their account information, including the account number, account holder name, 
and current balance.
6. Transaction History:
 - The system should maintain a transaction history for each account, recording all deposits, withdrawals, and 
money transfers.
 - The transaction history should include details such as the transaction type (deposit, withdrawal, or transfer), 
amount, recipient/sender information, and timestamp.
7. Exchange Rates:
 - The system should provide the ability to show exchange rates for different currencies.
 - Users should be able to view the current exchange rates between different currencies, such as USD to EUR, GBP 
to JPY, etc.
 - The exchange rate information should be retrieved from a reliable external source, such as an API, and displayed 
to the users.
8. Error Handling:
 - The system should handle and display appropriate error messages for invalid operations, such as insufficient 
funds, incorrect account numbers, invalid login credentials, or failed exchange rate retrieval.
9. Persistence:
 - The system should provide persistence for user and account data, allowing user accounts and associated 
information to be saved and loaded from storage.
10. Security:
 - The system should implement appropriate security measures to protect sensitive user information, such as 
passwords and account balances.
 - User authentication and authorization should be enforced to ensure that only authorized users can access and 
modify their accounts.
11. User Interface:
 - The system should have a user-friendly interface that allows users to interact with the bank system and perform 
operations easily.
 - The user interface should provide clear prompts and instructions for user registration, login, account creation, 
transactions, accessing account information, and viewing exchange rates.
Your task is to design and implement the bank system in C#, ensuring that it meets the requirements outlined above. 
Your solution should demonstrate good programming practices, such as the use of classes, methods, error handling, 
appropriate data structures, secure password storage.
