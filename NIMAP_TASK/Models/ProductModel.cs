using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NIMAP_TASK.Models
{
    public class ProductModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
    }
}