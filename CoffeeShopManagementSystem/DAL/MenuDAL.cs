using CoffeeShopManagementSystem.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShopManagementSystem.DAL
{
    public class MenuDAL
    {
        private static MenuDAL instance;

        public static MenuDAL Instance 
        {
            get { if (instance == null) instance = new MenuDAL(); return instance; } 
            private set => instance = value; 
        }

        private MenuDAL() { }

        public List<Menu> GetListMenuByTable(int idTable)
        {
            List<Menu> menu = new List<Menu> ();

            string query = "select D.name, ID.quantity, D.price, D.price*ID.quantity as totalPrice from dbo.InvoiceDetail as ID, dbo.Invoice as I, dbo.Drink as D where ID.idInvoice = I.id and ID.idDrink = D.id and I.status = 0 and I.idTable = " + idTable;

            DataTable data = DataProvider.Instance.executeQuery(query);

            foreach (DataRow row in data.Rows)
            {
                Menu menuRecord = new Menu(row);
                menu.Add(menuRecord);
            }

            return menu;
        }
    }
}
