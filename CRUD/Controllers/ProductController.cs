using System;
using System.Collections.Generic;
using System.Web.Mvc;
using CRUD.DB;
using System.Data;
using CRUD.Models;
using System.Data.SqlClient;
using System.Configuration;

namespace CRUD.Controllers
{
    public class ProductController : Controller
    {
        private readonly AdoHelper adoHelper;

        public ProductController()
        {
            adoHelper = new AdoHelper();
        }

        public ActionResult Index(int page = 1, int pageSize = 5)
        {
            List<Product> products = new List<Product>();
            string connectionString = ConfigurationManager.ConnectionStrings["Test"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string sql = "SELECT p.ProductId, p.ProductName, p.CategoryId, c.CategoryName " +
                             "FROM Product p " +
                             "JOIN Category c ON p.CategoryId = c.CategoryId " +
                             "ORDER BY p.ProductId OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";

                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@Offset", (page - 1) * pageSize);
                command.Parameters.AddWithValue("@PageSize", pageSize);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Product product = new Product();
                        product.ProductId = Convert.ToInt32(reader["ProductId"]);
                        product.ProductName = reader["ProductName"].ToString();
                        product.CategoryId = Convert.ToInt32(reader["CategoryId"]);
                        product.CategoryName = reader["CategoryName"].ToString();
                        products.Add(product);
                    }
                }

                // Get total number of products
                int totalProducts = GetTotalProductsCount(connection);

                ViewBag.CurrentPage = page;
                ViewBag.TotalProducts = totalProducts;
            }

            return View(products);
        }

        private int GetTotalProductsCount(SqlConnection connection)
        {
            string countSql = "SELECT COUNT(*) FROM Product";
            SqlCommand countCommand = new SqlCommand(countSql, connection);
            int totalProducts = (int)countCommand.ExecuteScalar();
            return totalProducts;
        }

        // GET: Product/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Product/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                adoHelper.ExecuteNonQuery("INSERT INTO Product (ProductName, CategoryId) VALUES (@ProductName, @CategoryId)",
                    parameters: new Dictionary<string, object>
                    {
                { "@ProductName", product.ProductName },
                { "@CategoryId", product.CategoryId }
                    });
                return RedirectToAction("Index");
            }
            return View(product);
        }

        // GET: Product/Edit/5
        public ActionResult Edit(int id)
        {
            DataTable dt = adoHelper.ExecuteQuery("SELECT * FROM Product WHERE ProductId = @ProductId",
                parameters: new Dictionary<string, object> { { "@ProductId", id } });

            if (dt.Rows.Count == 0)
                return HttpNotFound();

            Product product = new Product
            {
                ProductId = Convert.ToInt32(dt.Rows[0]["ProductId"]),
                ProductName = dt.Rows[0]["ProductName"].ToString(),
                CategoryId = Convert.ToInt32(dt.Rows[0]["CategoryId"])
            };

            return View(product);
        }

        // POST: Product/Edit/5
        [HttpPost]
        public ActionResult Edit(Product product)
        {
            adoHelper.ExecuteNonQuery("UPDATE Product SET ProductName = @ProductName, CategoryId = @CategoryId WHERE ProductId = @ProductId",
                parameters: new Dictionary<string, object>
                {
                { "@ProductName", product.ProductName },
                { "@CategoryId", product.CategoryId },
                { "@ProductId", product.ProductId }
                });

            return RedirectToAction("Index");
        }

        // GET: Product/Delete/5
        public ActionResult Delete(int id)
        {
            adoHelper.ExecuteNonQuery("DELETE FROM Product WHERE ProductId = @ProductId",
                parameters: new Dictionary<string, object> { { "@ProductId", id } });

            return RedirectToAction("Index");
        }
    }
}