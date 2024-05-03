using CoffeeShopManagementSystem.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShopManagementSystem.DAL
{
    public class DrinkDAL
    {
        private static DrinkDAL instance;

        public static DrinkDAL Instance
        {
            get { if (instance == null) instance = new DrinkDAL(); return instance; }
            private set => instance = value;
        }

        private DrinkDAL() { }

        public List<Drink> GetDrinksByCateID(int cateID)
        {
            List<Drink> drinks = new List<Drink>();

            string query = "select * from dbo.Drink where idCategory = " + cateID;

            DataTable data = DataProvider.Instance.executeQuery(query);

            foreach (DataRow row in data.Rows)
            {
                Drink drink = new Drink(row);
                drinks.Add(drink);
            }

            return drinks;
        }

        public List<Drink> GetDrinks()
        {
            List<Drink> drinks = new List<Drink>();

            string query = "select * from dbo.Drink";

            DataTable data = DataProvider.Instance.executeQuery(query);

            foreach (DataRow row in data.Rows)
            {
                Drink drink = new Drink(row);
                drinks.Add(drink);
            }

            return drinks;
        }

        public List<Drink> SearchDrinkByName(string name)
        {
            List<Drink> drinks = new List<Drink>();

            string query = string.Format(" select * from dbo.Drink where dbo.fuConvertToUnsign1(name) like N'%' + dbo.fuConvertToUnsign1(N'{0}') + '%'", name);

            DataTable data = DataProvider.Instance.executeQuery(query);

            foreach (DataRow row in data.Rows)
            {
                Drink drink = new Drink(row);
                drinks.Add(drink);
            }

            return drinks;
        }

        public bool InsertDrink(string name, int idCate, float price)
        {
            string query = string.Format("insert dbo.Drink (name, idCategory, price) values (N'{0}', {1}, {2})", name, idCate, price);

            int result = DataProvider.Instance.executeNonQuery(query);

            return result > 0;
        }

        public bool UpdateDrink(int idDrink, string name, int idCate, float price)
        {
            string query = string.Format("update dbo.Drink set name = N'{0}', idCategory = {1}, price = {2} where id = {3}", name, idCate, price, idDrink);

            int result = DataProvider.Instance.executeNonQuery(query);

            return result > 0;
        }

        public bool DeleteDrink(int idDrink)
        {
            InvoiceDetailDAL.Instance.DeleteInvoiceDetailByDrinkID(idDrink);

            string query = string.Format("delete dbo.Drink where id = {0}", idDrink);

            int result = DataProvider.Instance.executeNonQuery(query);

            return result > 0;
        }
    }
}
