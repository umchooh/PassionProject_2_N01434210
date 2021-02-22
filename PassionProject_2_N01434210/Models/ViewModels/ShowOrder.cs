using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PassionProject_2_N01434210.Models.ViewModels
{
    public class ShowOrder
    {

        //Information about the order
        public OrderDto Order { get; set; }
        public IEnumerable<CustomerDto> Allcustomers { get; set; }

        //Information about all customers on that order
        public IEnumerable<CustomerDto> Ordercustomers { get; set; }

        //Information about all products for that order
        public IEnumerable<ProductDto> Orderproducts { get; set; }
        public IEnumerable<OrderDto> Allorders { get; set; }
    }
}