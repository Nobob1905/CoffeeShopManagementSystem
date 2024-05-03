using CoffeeShopManagementSystem.DTO;
using CoffeeShopManagementSystem.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Principal;
using System.Security.Cryptography;
using System.Windows.Data;

namespace CoffeeShopManagementSystem
{
    public partial class frmAccountInfo : Form
    {
        private Account loginAccount;

        public Account LoginAccount
        {
            get { return loginAccount; }
            set { loginAccount = value; ShowAccount(loginAccount); }
        }

        public frmAccountInfo(Account account)
        {
            InitializeComponent();

            this.LoginAccount = account;
            ShowAccount(loginAccount);
        }

        void ShowAccount(Account account)
        {
            txbUsername.Text = account.Username;
            txbDisplayName.Text = account.DisplayName;
            txbIPhone.Text = account.Phone;
            txbIEmail.Text = account.Email;
            txbIAddress.Text = account.Address;
        }

        void UpdateAccountInfo()
        {
            string displayName = txbDisplayName.Text;
            string password = txbPassword.Text;
            string newPassword = txbNewPassword.Text;
            string verifyPassword = txbVerifyPassword.Text;
            string phone = txbIPhone.Text;
            string email = txbIEmail.Text;
            string address = txbIAddress.Text;
            string username = txbUsername.Text;

            byte[] tmp = ASCIIEncoding.ASCII.GetBytes(password);

            byte[] hasData = new MD5CryptoServiceProvider().ComputeHash(tmp);

            string hasPass = "";

            foreach (byte item in hasData)
            {
                hasPass += item;
            }

            byte[] tmpNewPass = ASCIIEncoding.ASCII.GetBytes(newPassword);

            byte[] hasNewData = new MD5CryptoServiceProvider().ComputeHash(tmpNewPass);

            string hasNewPass = "";

            if (newPassword != null && newPassword != "" && verifyPassword != null & newPassword != "")
            {
                foreach (byte item in hasNewData)
                {
                    hasNewPass += item;
                }
            } 

            if (!newPassword.Equals(verifyPassword))
            {
                MessageBox.Show("The verify password does not match with the new password!");
            }
            else
            {
                if (AccountDAL.Instance.UpdateAccountInfo(username, displayName, hasPass, hasNewPass, phone, email, address))
                {
                    MessageBox.Show("Updated successfully");
                    if (updateAccInfo != null)
                    {
                        updateAccInfo(this, new AccountEvent(AccountDAL.Instance.GetAccountByUsername(username)));
                    }
                }
                else
                {
                    MessageBox.Show("The original password was wrong");
                }
            }
        }
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            UpdateAccountInfo();
        }

        private event EventHandler<AccountEvent> updateAccInfo;
        public event EventHandler<AccountEvent> UpdateAccInfo
        {
            add { updateAccInfo += value; }
            remove { updateAccInfo -= value; }
        }

        private void frmAccountInfo_Load(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }

    public class AccountEvent : EventArgs
    {
        private Account acc;

        public Account Acc { get => acc; set => acc = value; }

        public AccountEvent(Account acc)
        {
            this.Acc = acc;
        }
    }
}
