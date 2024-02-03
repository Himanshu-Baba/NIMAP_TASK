using NIMAP_TASK.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NIMAP_TASK.Controllers
{
    public class ProductController : Controller
    {
        public ActionResult Index(int page = 1, int pageSize = 10)
        {
            List<ProductModel> products = GetProducts(page, pageSize);

            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalProducts = GetTotalProductsCount();

            return View(products);
        }

        public List<ProductModel> GetProducts(int page, int pageSize)
        {
            List<ProductModel> products = new List<ProductModel>();
            int skip = (page - 1) * pageSize;
                string con = ConfigurationManager.ConnectionStrings["Test"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(con))
            {
                string query = "SELECT TOP(@PageSize) * FROM (SELECT ROW_NUMBER() OVER (ORDER BY ProductId) AS RowNum, " +
                               "ProductId, ProductName, CategoryId, CategoryName FROM Products) AS P WHERE RowNum > @Skip";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@PageSize", pageSize);
                    command.Parameters.AddWithValue("@Skip", skip);

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        ProductModel product = new ProductModel
                        {
                            ProductId = reader.GetInt32(1),
                            ProductName = reader.GetString(2),
                            CategoryId = reader.GetInt32(3),
                            CategoryName = reader.GetString(4)
                        };
                        products.Add(product);
                    }
                }
            }

            return products;
        }

        public int GetTotalProductsCount()
        {
            int totalProducts = 0;
            string con = ConfigurationManager.ConnectionStrings["Test"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(con))
            {
                string query = "SELECT COUNT(*) FROM Products";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    totalProducts = (int)command.ExecuteScalar();
                }
            }

            return totalProducts;
        }
    }

}