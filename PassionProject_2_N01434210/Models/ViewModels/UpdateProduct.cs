using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PassionProject_2_N01434210.Models.ViewModels
{
    public class UpdateProduct
    {
        //base information about the product
        public ProductDto product { get; set; }
        //display all orders that this sponsor is sponsoring
        public IEnumerable<OrderDto> productsorders { get; set; }
        //display teams which could be sponsored in a dropdownlist
        public IEnumerable<OrderDto> allorders { get; set; }
    }
}