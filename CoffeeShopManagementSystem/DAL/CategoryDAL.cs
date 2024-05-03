using CoffeeShopManagementSystem.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShopManagementSystem.DAL
{
    public class CategoryDAL
    {
        private static CategoryDAL instance;

        public static CategoryDAL Instance 
        {
            get { if (instance == null) instance = new CategoryDAL(); return instance; }
            private set => instance = value; 
        }

        private CategoryDAL() { }

        public List<Category> GetCategories()
        {
            List<Category> categories = new List<Category>();

            string query = "select * from dbo.DrinkCategory";

            DataTable data = DataProvider.Instance.executeQuery(query);

            foreach (DataRow row in data.Rows)
            { 
                Category category = new Category(row);
                categories.Add(category);
            }

            return categories;
        }

        public Category GetCategoryByID(int id)
        {
            Category cate = null;

            string query = "select * from dbo.DrinkCategory where id = " + id;

            DataTable data = DataProvider.Instance.executeQuery(query);

            foreach (DataRow row in data.Rows)
            {
                cate = new Category(row);
                return cate;
            }

            return cate;
        }

        public bool InsertCate(string name)
        {
            string query = string.Format("insert dbo.DrinkCategory values (N'{0}')", name);

            int result = DataProvider.Instance.executeNonQuery(query);

            return result > 0;
        }

        public bool DeleteCate(int idCate)
        {
            string query = string.Format("delete dbo.DrinkCategory where id = {0}", idCate);

            int result = DataProvider.Instance.executeNonQuery(query);

            return result > 0;
        }

        public bool UpdateCate(int idCate, string name)
        {
            string query = string.Format("update dbo.DrinkCategory set name = N'{0}' where id = {1}", name, idCate);

            int result = DataProvider.Instance.executeNonQuery(query);

            return result > 0;
        }
    }
}
