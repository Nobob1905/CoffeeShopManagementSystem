using CoffeeShopManagementSystem.DAL;
using CoffeeShopManagementSystem.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CoffeeShopManagementSystem
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txbUsername.Text;
            string password = txbPassword.Text;

            if (Login(username, password))
            {
                Account loginAcc = AccountDAL.Instance.GetAccountByUsername(username);
                FrmTableManager tableManager = new FrmTableManager(loginAcc);
                this.Hide();
                tableManager.ShowDialog();
                this.Show();
            } else
            {
                MessageBox.Show("Wrong username or password!");
            }
        }

        bool Login(string username, string password)
        {
            return AccountDAL.Instance.login(username, password);
        }

        private void frmLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Are you sure to exit the program?","Notification" , MessageBoxButtons.OKCancel) != System.Windows.Forms.DialogResult.OK)
            {
                e.Cancel = true;
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txbUsername_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
