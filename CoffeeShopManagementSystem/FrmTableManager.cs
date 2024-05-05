using CoffeeShopManagementSystem.DAL;
using CoffeeShopManagementSystem.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CoffeeShopManagementSystem
{
    public partial class FrmTableManager : Form
    {
        private Account loginAccount;

        public Account LoginAccount 
        {
            get { return loginAccount; }
            set { loginAccount = value; ChangeAccount(loginAccount.Type); }
        }

        public FrmTableManager(Account account)
        {
            InitializeComponent();
            this.LoginAccount = account;

            Form_Load();
            LoadTableList();
            loadCategories();
            LoadCbBoxTable(cbTblSwicth);
        }

        private Color originalBackColor;
        private Color originalForeColor;

        private void Form_Load()
        {
            originalBackColor = adminToolStripMenuItem.BackColor;
            originalForeColor = adminToolStripMenuItem.ForeColor;

            adminToolStripMenuItem.AutoToolTip = false;

            adminToolStripMenuItem.MouseEnter += (s, args) =>
            {
                adminToolStripMenuItem.BackColor = Color.Gray;
                adminToolStripMenuItem.ForeColor = Color.Black;
            };

            adminToolStripMenuItem.MouseLeave += (s, args) =>
            {
                adminToolStripMenuItem.BackColor = originalBackColor;
                adminToolStripMenuItem.ForeColor = originalForeColor;
            };


            accountManagementToolStripMenuItem.AutoToolTip = false;

            accountManagementToolStripMenuItem.MouseEnter += (s, args) =>
            {
                accountManagementToolStripMenuItem.BackColor = Color.Gray;
                accountManagementToolStripMenuItem.ForeColor = Color.Black;
            };

            accountManagementToolStripMenuItem.MouseLeave += (s, args) =>
            {
                accountManagementToolStripMenuItem.BackColor = originalBackColor;
                accountManagementToolStripMenuItem.ForeColor = originalForeColor;
            };

            logOutToolStripMenuItem1.AutoToolTip = false;

            logOutToolStripMenuItem1.MouseEnter += (s, args) =>
            {
                logOutToolStripMenuItem1.BackColor = Color.Gray;
                logOutToolStripMenuItem1.ForeColor = Color.Black;
            };

            logOutToolStripMenuItem1.MouseLeave += (s, args) =>
            {
                logOutToolStripMenuItem1.BackColor = originalBackColor;
                logOutToolStripMenuItem1.ForeColor = originalForeColor;
            };
        }

        #region Methods
        void ChangeAccount(int type)
        {
            adminToolStripMenuItem.Enabled = type == 1;
            accountManagementToolStripMenuItem.Text += " [" + loginAccount.DisplayName + "]";
        }

        void LoadTableList()
        {
            flowLayoutTable.Controls.Clear();

            List<Table> tableList = TableDAL.Instance.LoadTableList();

            foreach (Table table in tableList)
            {
                Button button = new Button() { Width = TableDAL.tableWidth, Height = TableDAL.tableHeight };

                button.Text = table.Name + Environment.NewLine + "Status:" + Environment.NewLine + table.Status;
                button.Click += Button_Click;
                button.Tag = table;

                switch (table.Status)
                {
                    case "Empty":
                        button.BackColor = Color.LightGreen;
                        break;
                    default:
                        button.BackColor = Color.IndianRed;
                        break;
                }

                flowLayoutTable.Controls.Add(button);
            }
        }
        void DisplayInvoice(int id)
        {
            lsvInvoice.Items.Clear();
            List<CoffeeShopManagementSystem.DTO.Menu> invoiceDetailList = MenuDAL.Instance.GetListMenuByTable(id);
            float totalPrice = 0;

            
            foreach (CoffeeShopManagementSystem.DTO.Menu invoice in invoiceDetailList)
            {
                ListViewItem items = new ListViewItem(invoice.FoodName.ToString());
                items.SubItems.Add(invoice.Quantity.ToString());
                items.SubItems.Add(invoice.Price.ToString());
                items.SubItems.Add(invoice.TotalPrice.ToString());
                totalPrice += invoice.TotalPrice;
                lsvInvoice.Items.Add(items);
            }
            CultureInfo cul = new CultureInfo("vi-VN");

            txbTotalPrice.Text = totalPrice.ToString("c", cul);
        }
        void loadCategories()
        {
            List<Category> categories = CategoryDAL.Instance.GetCategories();
            cbCategory.DataSource = categories;
            cbCategory.DisplayMember = "name";
        }
        void loadDrinkListByCateID(int ID)
        {
            List<Drink> drinks = DrinkDAL.Instance.GetDrinksByCateID(ID);
            cbDrinks.DataSource = drinks;
            cbDrinks.DisplayMember = "name";
        }

        void LoadCbBoxTable(ComboBox combo)
        {
            combo.DataSource = TableDAL.Instance.LoadTableList();
            combo.DisplayMember = "name";
        }

        #endregion

        #region Events
        void Button_Click(object sender, EventArgs e)
        {
            int tableID = ((sender as Button).Tag as Table).ID;
            lsvInvoice.Tag = (sender as Button).Tag;
            DisplayInvoice(tableID);
        }
        private void logOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void yourProfileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAccountInfo accountInfo = new frmAccountInfo(loginAccount);
            accountInfo.UpdateAccInfo += accountInfo_updateAccInfo;
            accountInfo.ShowDialog();
        }

        private void accountManagementToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAccountInfo accountInfo = new frmAccountInfo(loginAccount);
            accountInfo.UpdateAccInfo += accountInfo_updateAccInfo;
            accountInfo.ShowDialog();
        }

        private void logOutToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void accountInfo_updateAccInfo(object sender, AccountEvent e)
        {
            accountManagementToolStripMenuItem.Text = "Account information [" + e.Acc.DisplayName + "]";
        }

        private void adminToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAdmin admin = new frmAdmin();

            admin.loginAcc = loginAccount;

            admin.InsertDrink += admin_InsertDrink;
            admin.UpdateDrink += admin_UpdateDrink;
            admin.DeleteDrink += admin_DeleteDrink;

            admin.InsertCate += admin_InsertCate;
            admin.UpdateCate += admin_UpdateCate;
            admin.DeleteCate += admin_DeleteCate;

            admin.InsertTable += admin_InsertTbl;
            admin.UpdateTable += admin_UpdateTbl;
            admin.DeleteTable += admin_DeleteTbl;
            admin.ShowDialog();
        }

        void admin_InsertDrink(object sender, EventArgs e)
        {
            loadDrinkListByCateID((cbCategory.SelectedItem as Category).Id);

            if (lsvInvoice.Tag != null) { DisplayInvoice((lsvInvoice.Tag as Table).ID); }
        }

        void admin_UpdateDrink(object sender, EventArgs e)
        {
            loadDrinkListByCateID((cbCategory.SelectedItem as Category).Id);

            if (lsvInvoice.Tag != null) { DisplayInvoice((lsvInvoice.Tag as Table).ID); }
        }

        void admin_DeleteDrink(object sender, EventArgs e)
        {
            loadDrinkListByCateID((cbCategory.SelectedItem as Category).Id);

            if (lsvInvoice.Tag != null) { DisplayInvoice((lsvInvoice.Tag as Table).ID); }

            LoadTableList();
        }

        void admin_InsertCate(object sender, EventArgs e)
        {
            loadCategories();
        }

        void admin_UpdateCate(object sender, EventArgs e)
        {
            loadCategories();
        }

        void admin_DeleteCate(object sender, EventArgs e)
        {
            loadCategories();
        }

        void admin_InsertTbl(object sender, EventArgs e)
        {
            LoadTableList();
        }

        void admin_UpdateTbl(object sender, EventArgs e)
        {
            LoadTableList();
        }

        void admin_DeleteTbl(object sender, EventArgs e)
        {
            LoadCbBoxTable(cbTblSwicth);
            LoadTableList();         
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void flowLayoutTable_Paint(object sender, PaintEventArgs e)
        {

        }


        private void accountToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void FrmTableManager_Load(object sender, EventArgs e)
        {

        }

        private void cbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            int ID = 0;

            ComboBox combo = sender as ComboBox;

            if (combo.SelectedItem == null) return; 

            Category category = combo.SelectedItem as Category;
            ID = category.Id;

            loadDrinkListByCateID(ID);
        }

        private void btnAddItem_Click(object sender, EventArgs e)
        {
            Table table = lsvInvoice.Tag as Table;

            if (table == null) return;

            int invoiceID = InvoiceDAL.Instance.GetUncheckOutInvoiceIdByTableID(table.ID);
            int DrinkID = (cbDrinks.SelectedItem as Drink).Id;
            int quantity = (int)numericUpDownQuantity.Value;

            if (invoiceID == -1)
            {
                InvoiceDAL.Instance.InsertInvoice(table.ID);
                InvoiceDetailDAL.Instance.InsertInvoiceDetail(InvoiceDAL.Instance.GetMaxInvoiceID(), DrinkID, quantity);
            } 
            else
            {
                InvoiceDetailDAL.Instance.InsertInvoiceDetail(invoiceID, DrinkID, quantity);
            }

            DisplayInvoice(table.ID);
            LoadTableList();
            numericUpDownQuantity.Value = 1;
        }

        private void btnPayment_Click(object sender, EventArgs e)
        {
            Table table = lsvInvoice.Tag as Table;

            if (table == null) return;

            int InvoiceID = InvoiceDAL.Instance.GetUncheckOutInvoiceIdByTableID(table.ID);
            int discount = (int)numericUpDownDiscount.Value;


            CultureInfo VNcul = new CultureInfo("vi-VN");
            double totalInvoice;
            if (!double.TryParse(txbTotalPrice.Text, NumberStyles.Any, VNcul, out totalInvoice))
            {
                MessageBox.Show("Invalid number format");
                return;
            }
            double totalPrice = totalInvoice - (totalInvoice / 100) * discount;

            if (InvoiceID != -1) 
            {
                if (MessageBox.Show(string.Format("Invoice Detail For {0}\n Total Invoice: {1}\n Discount: {2}%\n Total price: {3}", table.Name, totalInvoice, discount, totalPrice),"Notification", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
                {
                    InvoiceDAL.Instance.CheckOut(InvoiceID, discount, (float)totalPrice);
                    DisplayInvoice(table.ID);

                    InvoiceExport invoiceExport = new InvoiceExport();

                    invoiceExport.InvoiceExportion(InvoiceID, discount);

                    invoiceExport.ShowDialog();

                    LoadTableList();
                }

            }
            numericUpDownDiscount.Value = 0;
        }

        private void btnTblSwitch_Click(object sender, EventArgs e)
        {
            Table tblID1 = (lsvInvoice.Tag as Table);
            Table tblID2 = (cbTblSwicth.SelectedItem as Table);

            if (tblID1 == null || tblID2 == null)
            {
                MessageBox.Show("One of the tables is not selected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            } else
            {
                int tblID01 = (lsvInvoice.Tag as Table).ID;
                int tblID02 = (cbTblSwicth.SelectedItem as Table).ID;

                if (MessageBox.Show(string.Format("Switch from {0} to {1}", (lsvInvoice.Tag as Table).Name, (cbTblSwicth.SelectedItem as Table).Name), "Notification", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
                {
                    TableDAL.Instance.SwitchTable(tblID01, tblID02);
                    LoadTableList();
                }
            }
        }
        #endregion

        private void cbTblSwicth_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
        }
    }
}
