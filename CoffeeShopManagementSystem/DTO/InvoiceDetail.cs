using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShopManagementSystem.DTO
{
    public class InvoiceDetail
    {
        private int iD;
        private int idInvoice;
        private int idDrink;
        private int quantity;

        public InvoiceDetail(int ID, int IdInvoice, int IdDrink, int count) 
        {
            this.ID = ID;   
            this.IdInvoice = IdInvoice;
            this.IdDrink = IdDrink;
            this.Quantity = count;
        }

        public InvoiceDetail(DataRow row)
        {
            this.ID = (int)row["id"];
            this.IdInvoice = (int)row["idInvoice"];
            this.IdDrink = (int)row["idDrink"];
            this.Quantity = (int)row["quantity"];
        }

        public int ID { get => iD; set => iD = value; }
     
        public int IdDrink { get => idDrink; set => idDrink = value; }
        public int IdInvoice { get => idInvoice; set => idInvoice = value; }
        public int Quantity { get => quantity; set => quantity = value; }
    }
}
