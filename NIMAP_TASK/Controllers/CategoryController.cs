using NIMAP_TASK.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NIMAP_TASK.Controllers
{
    public class CategoryController : Controller
    {
        public ActionResult Index()
        {
            List<CategoryModel> categories = GetCategoriesFromDatabase();
            return View(categories);
        }

        private List<CategoryModel> GetCategoriesFromDatabase()
        {
            List<CategoryModel> categories = new List<CategoryModel>();
            
            string con = ConfigurationManager.ConnectionStrings["Test"].ConnectionString;

            using (var connection = new SqlConnection(con))
            {
                connection.Open();
                using (var command = new SqlCommand("SELECT * FROM Categories", connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            CategoryModel category = new CategoryModel
                            {
                                CategoryId = reader.GetInt32(0),
                                CategoryName = reader.GetString(1)
                            };
                            categories.Add(category);
                        }
                    }
                }
            }

            return categories;
        }
    }

}