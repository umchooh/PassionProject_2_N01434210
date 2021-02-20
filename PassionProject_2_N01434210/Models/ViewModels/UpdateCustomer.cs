using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PassionProject_2_N01434210.Models.ViewModels
{
    public class UpdateCustomer
    {
        //Information about the customer
        public CustomerDto Customer { get; set; }
        //Needed for a dropdownlist which presents the player with a choice of teams to play for
        public IEnumerable<OrderDto> Allorders { get; set; }
        public IEnumerable<OrderDto> Allcustomers { get; internal set; }
    }
}