using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShopManagementSystem.DTO
{
    public class Menu
    {
        private string foodName;
        private int quantity;
        private float price;
        private float totalPrice;

        public Menu(string foodName, int quantity, float price, float totalPrice = 0)
        {
            this.foodName = foodName;
            this.quantity = quantity;
            this.price = price;
            this.totalPrice = totalPrice;
        }
        public Menu(DataRow row)
        {
            this.foodName = row["name"].ToString();
            this.quantity = (int)row["quantity"];
            this.price = (float)Convert.ToDouble(row["price"].ToString());
            this.totalPrice = (float)Convert.ToDouble(row["totalPrice"].ToString());
        }

        public string FoodName { get => foodName; set => foodName = value; }
        public int Quantity { get => quantity; set => quantity = value; }
        public float Price { get => price; set => price = value; }
        public float TotalPrice { get => totalPrice; set => totalPrice = value; }
    }
}
