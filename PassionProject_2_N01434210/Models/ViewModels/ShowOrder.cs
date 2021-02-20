using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PassionProject_2_N01434210.Models.ViewModels
{
    public class ShowOrder
    {

        //Information about the order
        public OrderDto order { get; set; }

        //Information about all customers on that order
        public IEnumerable<CustomerDto> ordercustomers { get; set; }

        //Information about all products for that order
        public IEnumerable<ProductDto> orderproducts { get; set; }
    }
}