using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShopManagementSystem.DTO
{
    public class Invoice
    {
        private int iD;
        private DateTime? dateCheckIn; 
        private DateTime? dateCheckOut;
        private int status;
        private int discount;
        
        public Invoice(int ID, DateTime? dateCheckIn, DateTime? dateCheckOut, int status, int discount = 0)
        {
            this.ID = ID;
            this.dateCheckIn = dateCheckIn; 
            this.dateCheckOut = dateCheckOut;
            this.status = status;
            this.discount = discount;
        }

        public Invoice(DataRow row) 
        {
            this.ID = (int)row["ID"];
            this.dateCheckIn = (DateTime?)row["dateCheckIn"];

            var dateCheckOutTmp = row["dateCheckOut"];

            if (dateCheckOutTmp.ToString() != "")
            {
                this.dateCheckOut = (DateTime?)dateCheckOutTmp;
            }

            this.status = (int)row["status"];
            
            if (row["discount"].ToString() != "") { this.discount = (int)row["discount"]; }
        }

        public int ID { get => iD; set => iD = value; }
        public DateTime? DataCheckIn { get => dateCheckIn; set => dateCheckIn = value; }
        public DateTime? DataCheckOut { get => dateCheckOut; set => dateCheckOut = value; }
        public int Status { get => status; set => status = value; }
        public int Discount { get => discount; set => discount = value; }
    }
}
