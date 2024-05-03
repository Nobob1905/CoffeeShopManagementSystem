using CoffeeShopManagementSystem.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShopManagementSystem.DAL
{
    public class InvoiceDetailDAL
    {
        private static InvoiceDetailDAL instance;

        public static InvoiceDetailDAL Instance 
        {
            get { if (instance == null) instance = new InvoiceDetailDAL(); return instance; }
            private set => instance = value; 
        }

        private InvoiceDetailDAL() { }

        public void DeleteInvoiceDetailByDrinkID(int idDrink)
        {
            DataProvider.Instance.executeQuery("delete dbo.InvoiceDetail where idDrink = " + idDrink);
        }

        public List<InvoiceDetail> GetListInvoiceDetail(int id)
        {
            List<InvoiceDetail> listInvoiceDetail = new List<InvoiceDetail>();

            DataTable data = DataProvider.Instance.executeQuery("select * from dbo.InvoiceDetail where idInvoice = " + id);

            foreach (DataRow row in data.Rows)
            {
                InvoiceDetail detail = new InvoiceDetail(row);
                listInvoiceDetail.Add(detail);
            }

            return listInvoiceDetail;
        }

        public void InsertInvoiceDetail(int invoiceID, int DrinkID, int quantity)
        {
            DataProvider.Instance.executeNonQuery("exec USP_InsertInvoiceDetail @idInvoice , @idDrink , @quantity", new object[] { invoiceID, DrinkID, quantity });
        }

        public void DeleteInvoiceDetailByInvoiceID(int invoiceID)
        {
            DataProvider.Instance.executeQuery("delete dbo.InvoiceDetail where idInvoice = " + invoiceID);
        }
    }
}
