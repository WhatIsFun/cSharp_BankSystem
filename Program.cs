namespace cSharp_BankSystem
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("      Welcome To    ");
            Console.WriteLine(" +-+-+-+-+-+-+-+-+-+\r\n |W|h|a|t|I|s|F|u|n|\r\n +-+-+-+-+-+-+-+-+-+");
            Console.WriteLine("    Banking System      \n");
            Console.ResetColor();

            mainMenu();


            void mainMenu()
            {
                BankSystem bankSystem = new BankSystem();
                bankSystem.LoadUserData();
                while (true)
                {
                    Console.WriteLine("1. Register");
                    Console.WriteLine("2. Login");
                    Console.WriteLine("3. Exit");
                    Console.Write("Select an option: ");
                    string choice = Console.ReadLine();

                    switch (choice)
                    {
                        case "1":
                            Console.Write("Enter your name: ");
                            string name = Console.ReadLine();
                            Console.Write("Enter your email: ");
                            string email = Console.ReadLine();
                            Console.Write("Enter your password: ");
                            string password = Console.ReadLine();

                            if (bankSystem.RegisterUser(name, email, password))
                            {
                                Console.WriteLine("Registration successful.");
                            }
                            else
                            {
                                Console.WriteLine("Registration failed. User with this email already exists.");
                            }
                            break;

                        case "2":
                            Console.Write("Enter your email: ");
                            string loginEmail = Console.ReadLine();
                            Console.Write("Enter your password: ");
                            string loginPassword = Console.ReadLine();

                            if (bankSystem.Login(loginEmail, loginPassword))
                            {
                                Console.WriteLine("Login successful.");
                                bankSystem.HandleLoggedInUser();
                            }
                            else
                            {
                                Console.WriteLine("Login failed. Invalid email or password.");
                            }
                            break;

                        case "3":
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write("Are you sure you want to exit? (y/n) "); // Check if the user want to exit the application
                            string ExitInput = Console.ReadLine();
                            ExitInput.ToLower();
                            Console.ResetColor();
                            if (ExitInput.Equals("y", StringComparison.OrdinalIgnoreCase))
                            {
                                Console.Write("Thank You");
                                Environment.Exit(0);
                            }
                            else
                            {
                                mainMenu();
                            }
                            break;

                        default:
                            Console.WriteLine("Invalid option. Please try again.");
                            break;
                    }
                }
            }
        }
    }
}