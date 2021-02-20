using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PassionProject_2_N01434210.Models
{
    public class OrderItems
    {
        [Key]
        public int OrderItemID { get; set; }
        public int OrderID { get; set; }

        public double ProductPrice { get; set; }
        public decimal Amount { get; set; }
        public int ProductID { get; set; }
        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }
    }
}