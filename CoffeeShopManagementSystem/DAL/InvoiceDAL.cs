using CoffeeShopManagementSystem.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShopManagementSystem.DAL
{
    public class InvoiceDAL
    {
        private static InvoiceDAL instance;

        public static InvoiceDAL Instance 
        {
            get { if (instance == null) instance = new InvoiceDAL(); return instance; } 
            private set => instance = value; 
        }

        private InvoiceDAL() { }

        public int GetUncheckOutInvoiceIdByTableID(int tableID)
        {
            DataTable data = DataProvider.Instance.executeQuery("select * from dbo.Invoice where idTable = " + tableID + " and status = 0");
            
            if (data.Rows.Count > 0)
            {
                Invoice invoice = new Invoice(data.Rows[0]);
                return invoice.ID;
            }

            return -1;
        }

        public void CheckOut(int invoiceID, int discount, float totalPrice = 0)
        {
            string query = "update dbo.Invoice set dateCheckout = GETDATE() ,status = 1, " + "discount = " + discount + ", totalPrice = " + totalPrice + " where id = " + invoiceID;

            DataProvider.Instance.executeNonQuery(query);
        }

        public void InsertInvoice(int tableID)
        {
            DataProvider.Instance.executeNonQuery("exec USP_InsertInvoice @idTable", new object[] {tableID});
        }
        public int GetMaxInvoiceID()
        {
            try
            {
                return (int)DataProvider.Instance.executeScalar("select MAX(id) from dbo.Invoice");
            }
            catch
            {
                return 1;
            }
        }

        public DataTable GetReportByDate(DateTime checkIn, DateTime checkOut)
        {
            return DataProvider.Instance.executeQuery("exec USP_GetReportByDate @DateCheckIn , @DateCheckOut", new object[] { checkIn, checkOut });
        }

        public void DeleteInvoiceByTableID(int idTbl)
        {
            DataProvider.Instance.executeQuery("delete dbo.Invoice where idTable = " + idTbl);
        }
        public int GetIdInvoiceByTableID(int idTbl)
        {
            return Convert.ToInt32(DataProvider.Instance.executeScalar("select id from dbo.Invoice where idTable = " + idTbl));
            
        }
    }
}
