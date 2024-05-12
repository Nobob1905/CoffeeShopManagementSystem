using CoffeeShopManagementSystem.DAL;
using CoffeeShopManagementSystem.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace CoffeeShopManagementSystem
{
    public partial class frmAdmin : Form
    {
        BindingSource DrinkItem = new BindingSource();
        BindingSource CategoriesList = new BindingSource();
        BindingSource TableList = new BindingSource();
        BindingSource AccountList = new BindingSource();

        public Account loginAcc;

        public frmAdmin()
        {
            InitializeComponent();
            DataLoad();
        }

        #region methods

        List<Drink> SearchDrinkByName(string name)
        {
            List<Drink> listItems = DrinkDAL.Instance.SearchDrinkByName(name);

            return listItems;
        }

        void DataLoad()
        {
            dataGridViewItems.DataSource = DrinkItem;
            dataGridViewCategory.DataSource = CategoriesList;
            dataGridViewTbl.DataSource = TableList;
            dataGridViewAccount.DataSource = AccountList;

            LoadDateTimePicker();
            LoadInvoicesByFiltedDate(dateTimePickerFromDate.Value, dateTimePickerToDate.Value);
            LoadDrinks();
            LoadCateItemsIntoCbx(CbxItemsCate);
            LoadCategories();
            LoadTables();
            LoadAccount();
            AddItemsBinding();
            AddCatesBinding();
            AddTableBinding();
            AddAccountBinding();
        }

        private void txbItemName_TextChanged(object sender, EventArgs e)
        {

        }

        private void frmAdmin_Load(object sender, EventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void panel11_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void panel13_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dataGridViewAccount_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        
        void LoadDateTimePicker()
        {
            DateTime today = DateTime.Now;
            dateTimePickerFromDate.Value = new DateTime(today.Year, today.Month, 1);
            dateTimePickerToDate.Value = dateTimePickerFromDate.Value.AddMonths(1).AddDays(-1);
        }
        void LoadInvoicesByFiltedDate(DateTime checkIn, DateTime checkOut)
        {
            dataGridViewInvoice.DataSource = InvoiceDAL.Instance.GetReportByDate(checkIn, checkOut);
        }


        void LoadDrinks()
        {
            DrinkItem.DataSource = DrinkDAL.Instance.GetDrinks();
        }    

        void LoadCategories()
        {
            CategoriesList.DataSource = CategoryDAL.Instance.GetCategories();
        }

        void LoadTables()
        {
            TableList.DataSource = TableDAL.Instance.LoadTableList();
        }

        void LoadAccount()
        {
            AccountList.DataSource = AccountDAL.Instance.GetAccounts();
        }

        void AddItemsBinding()
        {
            txbItemName.DataBindings.Add(new Binding("Text", dataGridViewItems.DataSource, "Name", true, DataSourceUpdateMode.Never));
            txbItemID.DataBindings.Add(new Binding("Text", dataGridViewItems.DataSource, "ID", true, DataSourceUpdateMode.Never));
            numericUpDownItemPrice.DataBindings.Add(new Binding("Value", dataGridViewItems.DataSource, "Price", true, DataSourceUpdateMode.Never));
        }

        void AddCatesBinding()
        {
            txbCateID.DataBindings.Add(new Binding("Text", dataGridViewCategory.DataSource, "ID", true, DataSourceUpdateMode.Never));
            txbCateItem.DataBindings.Add(new Binding("Text", dataGridViewCategory.DataSource, "Name", true, DataSourceUpdateMode.Never));
        }

        void AddTableBinding()
        {
            txbTblID.DataBindings.Add(new Binding("Text", dataGridViewTbl.DataSource, "ID", true, DataSourceUpdateMode.Never));
            txbTblName.DataBindings.Add(new Binding("Text", dataGridViewTbl.DataSource, "name", true, DataSourceUpdateMode.Never));
        }

        void AddAccountBinding()
        {
            txbUsernameAcc.DataBindings.Add(new Binding("Text", dataGridViewAccount.DataSource, "Username", true, DataSourceUpdateMode.Never));
            txbDisplaynameAcc.DataBindings.Add(new Binding("Text", dataGridViewAccount.DataSource, "Display name", true, DataSourceUpdateMode.Never));
            numericUpDownTypeAcc.DataBindings.Add(new Binding("Value", dataGridViewAccount.DataSource, "Role", true, DataSourceUpdateMode.Never));
            txbPhone.DataBindings.Add(new Binding("Text", dataGridViewAccount.DataSource, "Phone", true, DataSourceUpdateMode.Never));
            txbEmail.DataBindings.Add(new Binding("Text", dataGridViewAccount.DataSource, "Email", true, DataSourceUpdateMode.Never));
            txbAddress.DataBindings.Add(new Binding("Text", dataGridViewAccount.DataSource, "Address", true, DataSourceUpdateMode.Never));
        }
        private void txbItemID_TextChanged(object sender, EventArgs e)
        {
            if (dataGridViewItems.SelectedCells.Count > 0)
            {
                var selectedCell = dataGridViewItems.SelectedCells[0];
                var owningRow = selectedCell.OwningRow;
                var idCell = owningRow.Cells["IdCategory"];

                if (idCell.Value == null)
                {
                    return;
                }

                int id = (int)idCell.Value;

                Category cate = CategoryDAL.Instance.GetCategoryByID(id);

                CbxItemsCate.SelectedItem = cate;

                int index = -1;
                int i = 0;

                foreach (Category item in CbxItemsCate.Items)
                {
                    if (item.Id == cate.Id)
                    {
                        index = i;
                        break;
                    }
                    i++;
                }

                CbxItemsCate.SelectedIndex = index;
            }
        }

        void LoadCateItemsIntoCbx(ComboBox cbx)
        {
            cbx.DataSource = CategoryDAL.Instance.GetCategories();
            cbx.DisplayMember = "Name";
        }

        #endregion


        #region events
        /* TAB PROCEEDS */
        private void btnViewInvoice_Click(object sender, EventArgs e)
        {
            LoadInvoicesByFiltedDate(dateTimePickerFromDate.Value, dateTimePickerToDate.Value);
        }

        /* TAB ITEMS */
        private void btnSearchItem_Click(object sender, EventArgs e)
        {
            DrinkItem.DataSource = SearchDrinkByName(txbISearchtem.Text);
        }
        private void btnAddItem_Click(object sender, EventArgs e)
        {
            string name = txbItemName.Text;
            int idCate = (CbxItemsCate.SelectedItem as Category).Id;
            float price = (float)numericUpDownItemPrice.Value;

            if (DataProvider.Instance.DrinkExists(name))
            {
                MessageBox.Show("A drink with the same name already exists. Please use a different name.");
                return;
            }

            if (DrinkDAL.Instance.InsertDrink(name, idCate, price))
            {
                MessageBox.Show("A new drink added successfully");
                LoadDrinks();

                if (insertDrink != null) { insertDrink(this, new EventArgs()); }
            }
            else
            {
                MessageBox.Show("A new drink added fail!");
            }
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            string name = txbItemName.Text;
            int idCate = (CbxItemsCate.SelectedItem as Category).Id;
            float price = (float)numericUpDownItemPrice.Value;
            int idDrink = Convert.ToInt32(txbItemID.Text);

            if (DataProvider.Instance.DrinkExists(name))
            {
                MessageBox.Show("A drink with the same name already exists. Please use a different name.");
                return;
            }

            if (DrinkDAL.Instance.UpdateDrink(idDrink, name, idCate, price))
            {
                MessageBox.Show("Drink modified successfully");
                LoadDrinks();

                if (updateDrink != null) { updateDrink(this, new EventArgs()); }
            }
            else
            {
                MessageBox.Show("Drink modified fail!");
            }
        }

        private void btnDeleteItem_Click(object sender, EventArgs e)
        {
            int idDrink = Convert.ToInt32(txbItemID.Text);

            if (DrinkDAL.Instance.DeleteDrink(idDrink))
            {
                MessageBox.Show("Delete drink successfully");
                LoadDrinks();

                if (deleteDrink != null) { deleteDrink(this, new EventArgs()); }
            }
            else
            {
                MessageBox.Show("Delete drink fail!");
            }
        }

        /* TAB CATEGORY */
        private void btnAddCate_Click(object sender, EventArgs e)
        {
            string name = txbCateItem.Text;

            if (DataProvider.Instance.CateExists(name))
            {
                MessageBox.Show("A Category with the same name already exists. Please use a different name.");
                return;
            }

            if (CategoryDAL.Instance.InsertCate(name))
            {
                MessageBox.Show("A new category added successfully");
                LoadCategories();

                if (insertCate != null) { insertCate(this, new EventArgs()); }
            }
            else
            {
                MessageBox.Show("A new category added fail!");
            }
        }

        private void btnModifyCate_Click(object sender, EventArgs e)
        {
            string name = txbCateItem.Text;
            int idCate = Convert.ToInt32(txbCateID.Text);

            if (DataProvider.Instance.CateExists(name))
            {
                MessageBox.Show("A category with the same name already exists. Please use a different name.");
                return;
            }

            if (CategoryDAL.Instance.UpdateCate(idCate, name))
            {
                MessageBox.Show("A category modified successfully");
                LoadCategories();

                if (updateCate != null) { updateCate(this, new EventArgs()); }
            }
            else
            {
                MessageBox.Show("A category modified fail!");
            }
        }

        private void btnDeleteCate_Click(object sender, EventArgs e)
        {
            int idCate = Convert.ToInt32(txbCateID.Text);

            if (CategoryDAL.Instance.DeleteCate(idCate))
            {
                MessageBox.Show("Delete a category successfully");
                LoadCategories();

                if (deleteCate != null) { deleteCate(this, new EventArgs()); }
            }
            else
            {
                MessageBox.Show("Delete a category fail!");
            }
        }

        /* TAB TABLE */
        private void btnAddTbl_Click(object sender, EventArgs e)
        {
            string name = txbTblName.Text;

            if (DataProvider.Instance.TableExists(name))
            {
                MessageBox.Show("A table with the same name already exists. Please use a different name.");
                return;
            }

            if (TableDAL.Instance.InsertTbl(name))
            {
                MessageBox.Show("A new table added successfully");
                LoadTables();

                if (insertTable != null) { insertTable(this, new EventArgs()); }
            }
            else
            {
                MessageBox.Show("A new table added fail!");
            }
        }

        private void btnModifyTbl_Click(object sender, EventArgs e)
        {
            string name = txbTblName.Text;
            int idTable = Convert.ToInt32(txbTblID.Text);

            if (DataProvider.Instance.TableExists(name))
            {
                MessageBox.Show("A table with the same name already exists. Please use a different name.");
                return;
            }

            if (TableDAL.Instance.UpdateTbl(idTable, name))
            {
                MessageBox.Show("A table modified successfully");
                LoadTables();

                if (updateTable != null) { updateTable(this, new EventArgs()); }
            }
            else
            {
                MessageBox.Show("A table modified fail!");
            }
        }

        private void btnDeleteTbl_Click(object sender, EventArgs e)
        {
            int idTbl = Convert.ToInt32(txbTblID.Text);

            if (TableDAL.Instance.DeleteTbl(idTbl))
            {
                MessageBox.Show("Delete a table successfully");
                LoadTables();

                if (deleteTable != null) { deleteTable(this, new EventArgs()); }
            }
            else
            {
                MessageBox.Show("Delete a table fail!");
            }
        }

        /* TAB ACCOUNT */

        private void btnAddAcc_Click(object sender, EventArgs e)
        {
            string username = txbUsernameAcc.Text;
            string displayName = txbDisplaynameAcc.Text;
            int type = (int)numericUpDownTypeAcc.Value;
            string phone = txbPhone.Text;
            string email = txbEmail.Text;
            string address = txbAddress.Text;

            if (DataProvider.Instance.AccUsernameExists(username))
            {
                MessageBox.Show("An account with the same username already exists. Please use a different username.");
                return;
            }

            if (AccountDAL.Instance.InsertAccount(username, displayName, type, phone, email, address))
            {
                MessageBox.Show("An new account added successfully");
                LoadAccount();

                /*if (insertAccount != null) { insertAccount(this, new EventArgs()); }*/
            }
            else
            {
                MessageBox.Show("An new account added fail!");
            }
        }

        private void btnDeleteAcc_Click(object sender, EventArgs e)
        {
            string username = txbUsernameAcc.Text;

            if (loginAcc.Username.Equals(username))
            {
                MessageBox.Show("This account is being use!");
                return;
            }

            if (AccountDAL.Instance.DeleteAccount(username))
            {
                MessageBox.Show("Delete account successfully");
                LoadAccount();

                /*if (insertAccount != null) { insertAccount(this, new EventArgs()); }*/
            }
            else
            {
                MessageBox.Show("Delete account fail!");
            }
        }

        private void btnModifyAcc_Click(object sender, EventArgs e)
        {
            string username = txbUsernameAcc.Text;
            string displayName = txbDisplaynameAcc.Text;
            int type = (int)numericUpDownTypeAcc.Value;
            string phone = txbPhone.Text;
            string email = txbEmail.Text;
            string address = txbAddress.Text;

            if (AccountDAL.Instance.UpdateAccount(username, displayName, type, phone, email, address))
            {
                MessageBox.Show("Updated account successfully");
                LoadAccount();

                /*if (insertAccount != null) { insertAccount(this, new EventArgs()); }*/
            }
            else
            {
                MessageBox.Show("Updated account fail!");
            }
        }

        private void btnResetPassword_Click(object sender, EventArgs e)
        {
            string username = txbUsernameAcc.Text;

            if (AccountDAL.Instance.PassReset(username))
            {
                MessageBox.Show("Password reseted successfully");

                /*if (insertAccount != null) { insertAccount(this, new EventArgs()); }*/
            }
            else
            {
                MessageBox.Show("Password reseted fail!");
            }
        }
        #endregion

        private event EventHandler insertDrink;
        public event EventHandler InsertDrink
        {
            add { insertDrink += value; }
            remove { insertDrink -= value; }
        }

        private event EventHandler deleteDrink;
        public event EventHandler DeleteDrink
        {
            add { deleteDrink += value; }
            remove { deleteDrink -= value; }
        }

        private event EventHandler updateDrink;
        public event EventHandler UpdateDrink
        {
            add { updateDrink += value; }
            remove { updateDrink -= value; }
        }

        private event EventHandler insertCate;
        public event EventHandler InsertCate
        {
            add { insertCate += value; }
            remove { insertCate -= value; }
        }

        private event EventHandler deleteCate;
        public event EventHandler DeleteCate
        {
            add { deleteCate += value; }
            remove { deleteCate -= value; }
        }

        private event EventHandler updateCate;
        public event EventHandler UpdateCate
        {
            add { updateCate += value; }
            remove { updateCate -= value; }
        }

        private event EventHandler insertTable;
        public event EventHandler InsertTable
        {
            add { insertTable += value; }
            remove { insertTable -= value; }
        }

        private event EventHandler deleteTable;
        public event EventHandler DeleteTable
        {
            add { deleteTable += value; }
            remove { deleteTable -= value; }
        }

        private event EventHandler updateTable;
        public event EventHandler UpdateTable
        {
            add { updateTable += value; }
            remove { updateTable -= value; }
        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void label9_Click_1(object sender, EventArgs e)
        {

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void tabPage1_Click_1(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void numericUpDownTypeAcc_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
