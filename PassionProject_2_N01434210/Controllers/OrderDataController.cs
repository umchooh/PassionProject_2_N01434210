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
                    OrderSubtotal = Order.OrderSubtotal,
                    //Add customer ID
                    CustomerID = Order.CustomerID,
                   
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
        public IHttpActionResult GetOrdersForCustomer(int id)
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

        /// <summary>
        /// Gets a list or Products in the database alongside a status code (200 OK).
        /// </summary>
        /// <param name="id">The input orderid</param>
        /// <returns>A list of Productss including their ID, name, Description and price.</returns>
        /// <example>
        /// GET: api/ProductData/GetProductsForOrder
        /// </example>
        [ResponseType(typeof(IEnumerable<ProductDto>))]
        public IHttpActionResult GetProductsForOrder(int id)
        {
            List<Product> Products = db.Products
                .Where(s => s.Orders.Any(t => t.OrderID == id))
                .ToList();
            List<ProductDto> ProductDtos = new List<ProductDto> { };

            //Here you can choose which information is exposed to the API
            foreach (var Product in Products)
            {
                ProductDto NewProduct = new ProductDto
                {
                    ProductID = Product.ProductID,
                    ProductName = Product.ProductName,
                    ProductDescription = Product.ProductDescription,
                    ProductPrice = Product.ProductPrice
                                   
                };
                ProductDtos.Add(NewProduct);
            }

            return Ok(ProductDtos);
        }

        /// <summary>
        /// Finds a particular Order in the database with a 200 status code. If the Team is not found, return 404.
        /// </summary>
        /// <param name="id">The Order id</param>
        /// <returns>Information about the Order, including Order id, bio, first and last name, and teamid</returns>
        // <example>
        // GET: api/OrderData/FindOrder/5
        // </example>
        [HttpGet]
        [ResponseType(typeof(OrderDto))]
        public IHttpActionResult FindOrder(int id)
        {
            //Find the data
            Order Order = db.Orders.Find(id);
            //if not found, return 404 status code.
            if (Order == null)
            {
                return NotFound();
            }

            //put into a 'friendly object format'
            OrderDto OrderDto = new OrderDto
            {
                OrderID = Order.OrderID,
                CustomerID = Order.CustomerID,
                OrderDate = Order.OrderDate,
                OrderSubtotal = Order.OrderSubtotal
            };


            //pass along data as 200 status code OK response
            return Ok(OrderDto);
        }

        /// <summary>
        /// Updates a Order in the database given information about the Order.
        /// </summary>
        /// <param name="id">The Order id</param>
        /// <param name="Order">A Order object. Received as POST data.</param>
        /// <returns></returns>
        /// <example>
        /// POST: api/OrderData/UpdateOrder/5
        /// FORM DATA: Order JSON Object
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateOrder(int id, [FromBody] Order Order)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != Order.OrderID)
            {
                return BadRequest();
            }

            db.Entry(Order).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }


        /// <summary>
        /// Adds a Order to the database.
        /// </summary>
        /// <param name="Order">A Team object. Sent as POST form data.</param>
        /// <returns>status code 200 if successful. 400 if unsuccessful</returns>
        /// <example>
        /// POST: api/OrderData/AddOrder
        ///  FORM DATA: Order JSON Object
        /// </example>
        [ResponseType(typeof(Order))]
        [HttpPost]
        public IHttpActionResult AddOrder([FromBody] Order Order)
        {
            //Will Validate according to data annotations specified on model
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Orders.Add(Order);
            db.SaveChanges();

            return Ok(Order.OrderID);
        }

        /// <summary>
        /// Deletes an Order in the database
        /// </summary>
        /// <param name="id">The id of the Order to delete.</param>
        /// <returns>200 if successful. 404 if not successful.</returns>
        /// <example>
        /// POST: api/OrderData/DeleteOrder/5
        /// </example>
        [HttpPost]
        public IHttpActionResult DeleteOrder(int id)
        {
            Order Order = db.Orders.Find(id);
            if (Order == null)
            {
                return NotFound();
            }

            db.Orders.Remove(Order);
            db.SaveChanges();

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Finds a Order in the system. Internal use only.
        /// </summary>
        /// <param name="id">The Order id</param>
        /// <returns>TRUE if the Order exists, false otherwise.</returns>
        private bool OrderExists(int id)
        {
            return db.Orders.Count(e => e.OrderID == id) > 0;
        }
    }
}