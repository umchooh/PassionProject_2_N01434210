using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PassionProject_2_N01434210.Models.ViewModels
{
    public class ShowCustomer
    {
        public CustomerDto Customer { get; set; }
        //information about the order the customer orders for
        public OrderDto Order { get; set; }
    }
}