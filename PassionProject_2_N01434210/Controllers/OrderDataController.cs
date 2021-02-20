using System;
using System.IO;
using System.Web;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using System.Diagnostics;
using PassionProject_2_N01434210.Models;

namespace PassionProject_2_N01434210.Controllers
{
    public class OrderDataController : ApiController
    {
        //The Database Access Point
        private PassionDbContext db = new PassionDbContext();

        // This api contorller form form WebAPI2 with read/write action with EF.

        /// <summary>
        /// Gets a list of Order in the database alongside a status code (200 OK)
        /// </summary>
        /// <returns>A list of Orders including their ID, Date and Subtotal</returns>
        ///<example> GET: api/OrderData/GetOrders </example>
        [ResponseType(typeof(IEnumerable<OrderDto>))]
        public IEnumerable<OrderDto> GetOrders()
        {
            List<Order> Orders = db.Orders.ToList();
            List<OrderDto> OrderDtos = new List<OrderDto> { };

            //Here you can choose which information is exposed to the API
            foreach (var Order in Orders)
            {
                OrderDto NewOrder = new OrderDto
                {
                    OrderID = Order.OrderID,
                    OrderDate = Order.OrderDate,
                    OrderSubtotal = Order.OrderSubtotal
                };
                OrderDtos.Add(NewOrder);
            }

            return (OrderDtos);
        }

        /// <summary>
        /// Finds a particular Order in the database given a Customer id with a 200 status code. If the order is not found, return 404.
        /// </summary>
        /// <param name="id">The customer id</param>
        /// <returns>Information about the customer, including Customer id, first and last name, phone, email and shillping address</returns>
        // <example>
        // GET: api/OrderData/FindCustomerForOrder/5
        // </example>
        [HttpGet]
        [ResponseType(typeof(CustomerDto))]
        public IHttpActionResult GetOrderForCustomer(int id)
        {
            //Finds the first team which has any players
            //that match the input playerid
            Customer Customer = db.Customers
                .Where(c => c.Orders.Any(o => o.CustomerID == id))
                .FirstOrDefault();
            //if not found, return 404 status code.
            if (Customer == null)
            {
                return NotFound();
            }

            CustomerDto CustomerDto = new CustomerDto
            {
                CustomerID = Customer.CustomerID,
                CustomerFirstName = Customer.CustomerFirstName,
                CustomerLastName = Customer.CustomerLastName,
                CustomerPhoneNum = Customer.CustomerPhoneNum,
                CustomerEmail = Customer.CustomerEmail,
                CustomerShipping = Customer.CustomerShipping
            };


            //pass along data as 200 status code OK response
            return Ok(CustomerDto);


        }
    }
}