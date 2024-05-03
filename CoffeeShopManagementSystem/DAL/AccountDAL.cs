using CoffeeShopManagementSystem.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace CoffeeShopManagementSystem.DAL
{
    public class AccountDAL
    {
        private static AccountDAL instance;

        public static AccountDAL Instance 
        {
            get { if (instance == null) instance = new AccountDAL(); return instance; }
            private set => instance = value; 
        }

        private AccountDAL() { }

        public bool login(string username, string password)
        {
            byte[] tmp = ASCIIEncoding.ASCII.GetBytes(password);

            byte[] hasData = new MD5CryptoServiceProvider().ComputeHash(tmp);

            string hasPass = "";

            foreach(byte item in hasData)
            {
                hasPass += item;
            }

            string query = "USP_Login @username , @password";

            DataTable res = DataProvider.Instance.executeQuery(query, new object[] {username, hasPass});

            return res.Rows.Count > 0;    
        }

        public Account GetAccountByUsername(string username)
        {
            DataTable data = DataProvider.Instance.executeQuery("select * from dbo.Account where username = N'" + username + "'");
            
            foreach (DataRow row in data.Rows)
            {
                return new Account(row);
            }

            return null;
        }

        public bool UpdateAccountInfo(string username, string displayName, string password, string newPassword, string phone, string email, string address)
        {
            int results = DataProvider.Instance.executeNonQuery("exec USP_UpdateAccountInfo @username , @displayName , @password , @newPassword , @phone , @email , @address ", new object[] { username, displayName, password, newPassword, phone, email, address});
        
            return results > 0;
        }

        public DataTable GetAccounts()
        {
            return DataProvider.Instance.executeQuery("SELECT username as 'Username', displayName as 'Display name', CASE WHEN type = 1 THEN 'admin' WHEN type = 0 THEN 'staff' END AS 'Role', phone as 'Phone', email as 'Email', address as 'Address' FROM dbo.Account");
        }

        public bool InsertAccount(string username, string displayName, int type, string phone, string email, string address)
        {
            string query = string.Format("insert dbo.Account (username, displayName, type, phone, email, address, password) values (N'{0}', N'{1}', {2}, '{3}', '{4}', N'{5}', '{6}')", username, displayName, type, phone, email, address, "1962026656160185351301320480154111117132155");

            int result = DataProvider.Instance.executeNonQuery(query);

            return result > 0;
        }

        public bool UpdateAccount(string username, string displayName, int type, string phone, string email, string address)
        {
            string query = string.Format("update dbo.Account set displayName = N'{0}', type = {1}, phone = '{2}', email = '{3}', address = N'{4}' where username = N'{5}'", displayName, type, phone, email, address, username);

            int result = DataProvider.Instance.executeNonQuery(query);

            return result > 0;
        }

        public bool DeleteAccount(string username)
        {
            string query = string.Format("delete dbo.Account where username = N'{0}'", username);

            int result = DataProvider.Instance.executeNonQuery(query);

            return result > 0;
        }

        public bool PassReset(string username)
        {
            string query = string.Format("update dbo.Account set password = N'1962026656160185351301320480154111117132155' where username = N'{0}'", username);

            int result = DataProvider.Instance.executeNonQuery(query);

            return result > 0;
        }
    }
}
