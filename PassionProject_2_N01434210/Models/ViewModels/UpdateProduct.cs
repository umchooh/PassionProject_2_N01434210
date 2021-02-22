using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PassionProject_2_N01434210.Models.ViewModels
{
    public class UpdateProduct
    {
        //base information about the product
        public ProductDto Product { get; set; }
        //display all orders that this sponsor is sponsoring
        public IEnumerable<OrderDto> Productsorders { get; set; }
        //display teams which could be sponsored in a dropdownlist
        public IEnumerable<OrderDto> Allorders { get; set; }
    }
}