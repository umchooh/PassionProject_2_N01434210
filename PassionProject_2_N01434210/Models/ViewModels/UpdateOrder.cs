using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PassionProject_2_N01434210.Models.ViewModels
{
    public class UpdateOrder
    {
        
        //base information about the Order
        public OrderDto order { get; set; }
        //display all orders that this sponsor is sponsoring
        public IEnumerable<ProductDto> Productsorders { get; set; }
        //display all orders in dropdownlist
        public IEnumerable<ProductDto> Allorders { get; set; }

        public CustomerDto Customer { get; set; }
        //display all customers in dropdownlist
        public IEnumerable<CustomerDto> Allcustomers { get; set; }

    }
}
