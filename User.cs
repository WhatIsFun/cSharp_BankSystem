using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using BCrypt.Net;

namespace cSharp_BankSystem
{
    public class User
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string HashedPassword { get; set; }
        public List<Account> Accounts { get; set; }

        public User(string name, string email, string password)
        {
            Name = name;
            Email = email;
            HashedPassword = HashPassword(password);
            Accounts = new List<Account>();
        }

        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
    }
}
