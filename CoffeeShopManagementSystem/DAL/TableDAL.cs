using CoffeeShopManagementSystem.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShopManagementSystem.DAL
{
    public class TableDAL
    {
        private static TableDAL instance;
        public static int tableWidth = 150;
        public static int tableHeight = 80;

        public static TableDAL Instance 
        {
            get { if (instance == null) instance = new TableDAL(); return instance; }
            private set => instance = value; 
        }

        private TableDAL() { }

        public void SwitchTable(int TableID1, int TableID2)
        {
            DataProvider.Instance.executeQuery("USP_SwitchTable @TableID1 , @TableID2", new object[] {TableID1, TableID2});
        }

        public List<Table> LoadTableList()
        {
            List<Table> tableList = new List<Table> ();

            DataTable data = DataProvider.Instance.executeQuery("USP_GetTableList");

            foreach (DataRow item in data.Rows)
            {
                Table table = new Table(item);
                tableList.Add(table);
            }

            return tableList;
        }

        public bool InsertTbl(string name)
        {
            string query = string.Format("insert dbo.TablesList values (N'{0}',  N'Empty')", name);

            int result = DataProvider.Instance.executeNonQuery(query);

            return result > 0;
        }

        public bool UpdateTbl(int idTbl, string name)
        {
            string query = string.Format("update dbo.TablesList set name = N'{0}' where id = {1}", name, idTbl);

            int result = DataProvider.Instance.executeNonQuery(query);

            return result > 0;
        }

        public bool DeleteTbl(int idTbl)
        {
            InvoiceDetailDAL.Instance.DeleteInvoiceDetailByInvoiceID((InvoiceDAL.Instance.GetIdInvoiceByTableID(idTbl)));
            InvoiceDAL.Instance.DeleteInvoiceByTableID(idTbl);

            string query = string.Format("delete dbo.TablesList where id = {0}", idTbl);

            int result = DataProvider.Instance.executeNonQuery(query);

            return result > 0;
        }
    }
}
