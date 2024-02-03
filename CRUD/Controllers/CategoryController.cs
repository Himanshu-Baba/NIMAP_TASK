using CRUD.DB;
using CRUD.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;

namespace CRUD.Controllers
{
    public class CategoryController : Controller
    {
        private readonly AdoHelper adoHelper;

        public CategoryController()
        {
            adoHelper = new AdoHelper();
        }

        // GET: Category/Index
        public ActionResult Index()
        {
            DataTable dt = adoHelper.ExecuteQuery("SELECT * FROM Category");
            List<Category> categories = new List<Category>();

            foreach (DataRow row in dt.Rows)
            {
                categories.Add(new Category
                {
                    CategoryId = Convert.ToInt32(row["CategoryId"]),
                    CategoryName = row["CategoryName"].ToString()
                });
            }

            return View(categories);
        }

        // GET: Category/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Category/Create
        [HttpPost]
        public ActionResult Create(Category category)
        {
            adoHelper.ExecuteNonQuery("INSERT INTO Category (CategoryName) VALUES (@CategoryName)",
                parameters: new Dictionary<string, object> { { "@CategoryName", category.CategoryName } });
            return RedirectToAction("Index");
        }

        // GET: Category/Edit/5
        public ActionResult Edit(int id)
        {
            DataTable dt = adoHelper.ExecuteQuery("SELECT * FROM Category WHERE CategoryId = @CategoryId",
                parameters: new Dictionary<string, object> { { "@CategoryId", id } });

            if (dt.Rows.Count == 0)
                return HttpNotFound();

            Category category = new Category
            {
                CategoryId = Convert.ToInt32(dt.Rows[0]["CategoryId"]),
                CategoryName = dt.Rows[0]["CategoryName"].ToString()
            };

            return View(category);
        }

        // POST: Category/Edit/5
        [HttpPost]
        public ActionResult Edit(Category category)
        {
            adoHelper.ExecuteNonQuery("UPDATE Category SET CategoryName = @CategoryName WHERE CategoryId = @CategoryId",
                parameters: new Dictionary<string, object>
                {
                { "@CategoryName", category.CategoryName },
                { "@CategoryId", category.CategoryId }
                });

            return RedirectToAction("Index");
        }

        // GET: Category/Delete/5
        public ActionResult Delete(int id)
        {
            adoHelper.ExecuteNonQuery("DELETE FROM Category WHERE CategoryId = @CategoryId",
                parameters: new Dictionary<string, object> { { "@CategoryId", id } });

            return RedirectToAction("Index");
        }
    }
}