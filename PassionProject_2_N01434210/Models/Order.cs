using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PassionProject_2_N01434210.Models
{
    public class Order
    {
        [Key]
        public int OrderID { get; set; }

        public DateTime OrderDate { get; set; }
        public double OrderSubtotal { get; set; }

        //An order has one customer
        [ForeignKey("Customer")]
        public int CustomerID { get; set; }
        [ForeignKey("CustomerFK")]
        public virtual Customer Customer { get; set; }

        //An order will have many product
        public virtual ICollection<OrderItems> Products { get; set; }

    }

    //This class can be used totransfer information about an order
    //also known as a "Data Transfer Object"
    //vessel communication "Model-liked"
    public class OrderDto
    {
        public int OrderID { get; set; }
        [DisplayName("Date")]
        public DateTime OrderDate { get; set; }
        [DisplayName("Subtotal")]
        public double OrderSubtotal { get; set; }

        [DisplayName("CustomerID")]
        public double CustomerID { get; set; }
        [DisplayName("First Name")]
        public string CustomerFirstName { get; set; }
        [DisplayName("Last Name")]
        public string CustomerLastName { get; set; }




    }
}
