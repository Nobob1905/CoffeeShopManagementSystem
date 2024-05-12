using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShopManagementSystem.DTO
{
    public class Account
    {
        private string username;
        private string displayName;
        private string password;
        private int type;
        private string phone;
        private string email;
        private string address;

        public Account(string username, string displayName, int type, string phone, string email, string address, string password = null)
        {
            this.username = username;
            this.displayName = displayName;
            this.type = type;
            this.password = password;
            this.phone = phone;
            this.email = email;
            this.address = address;
        }

        public Account (DataRow row)
        {
            this.username = row["username"].ToString();
            this.displayName = row["displayName"].ToString();
            this.type = (int)row["type"];
            this.password = row["password"].ToString();
            this.phone = row["phone"].ToString();
            this.email = row["email"].ToString();
            this.address = row["address"].ToString();
        }

        public string Username { get => username; set => username = value; }
        public string DisplayName { get => displayName; set => displayName = value; }
        public string Password { get => password; set => password = value; }
        public int Type { get => type; set => type = value; }
        public string Phone { get => phone; set => phone = value; }
        public string Email { get => email; set => email = value; }
        public string Address { get => address; set => address = value; }
    }
}
