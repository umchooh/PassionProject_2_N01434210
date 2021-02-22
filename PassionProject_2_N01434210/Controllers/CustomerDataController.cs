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
    public class CustomerDataController : ApiController
    {
        //The Database Access Point
        private PassionDbContext db = new PassionDbContext();

        // This api contorller form form WebAPI2 with read/write action with EF.

        /// <summary>
        /// Gets a list of Customer in the database alongside a status code (200 OK)
        /// </summary>
        /// <returns>A list of Orders including their ID, Date and Subtotal</returns>
        ///<example> GET: api/CustomerData/GetCustomers </example>
        [ResponseType(typeof(IEnumerable<CustomerDto>))]
        public IEnumerable<CustomerDto> GetCustomers()
        {
            List<Customer> Customers = db.Customers.ToList();
            List<CustomerDto> CustomerDtos = new List<CustomerDto> { };

            //Here you can choose which information is exposed to the API
            foreach (var Customer in Customers)
            {
                CustomerDto NewCustomer = new CustomerDto
                {
                    CustomerID = Customer.CustomerID,
                    CustomerFirstName = Customer.CustomerFirstName,
                    CustomerLastName = Customer.CustomerLastName,
                    CustomerPhoneNum = Customer.CustomerPhoneNum,
                    CustomerEmail = Customer.CustomerEmail,
                    CustomerShipping = Customer.CustomerShipping,
                    
                };
                CustomerDtos.Add(NewCustomer);
            }

            return (CustomerDtos);
        }

        /// <summary>
        /// Finds a particular customer in the database with a 200 status code. If the customer is not found, return 404.
        /// </summary>
        /// <param name="id">The customer id</param>
        /// <returns>Information about the player, including player id, bio, first and last name, and teamid</returns>
        // <example>
        // GET: api/CustomerData/FindCustomer/5
        // </example>
        [HttpGet]
        [ResponseType(typeof(CustomerDto))]
        public IHttpActionResult FindCustomer(int id)
        {
            //Find a customer Data
            Customer Customer = db.Customers.Find(id);
            //If not found, return 404 status code
            if (Customer == null)
            {
                return NotFound();
            }

            //put into a 'friendly object format'
            CustomerDto CustomerDto = new CustomerDto
            {
                CustomerID = Customer.CustomerID,
                CustomerFirstName = Customer.CustomerFirstName,
                CustomerLastName = Customer.CustomerLastName,
                CustomerPhoneNum = Customer.CustomerPhoneNum,
                CustomerEmail = Customer.CustomerEmail,
                CustomerShipping = Customer.CustomerShipping,
                
                
            };

            //pass along data as 200 status code OK response
            return Ok(CustomerDto);
        }

        /// <summary>
        /// Gets a list of orders in the database alongside a status code (200 OK).
        /// </summary>
        /// <param name="id">The input customerid</param>
        /// <returns>A list of customers associated with the order</returns>
        /// <example>
        /// GET: api/OrderData/GetOrdersForCustomer
        /// </example>
        [ResponseType(typeof(IEnumerable<OrderDto>))]
        public IHttpActionResult FindOrdersForCustomer(int id)
        {
            List<Order> Orders = db.Orders.Where(c => c.CustomerID == id)
                .ToList();
            List<OrderDto> OrderDtos = new List<OrderDto> { };

            //Here you can choose which information is exposed to the API
            foreach (var Order in Orders)
            {
                OrderDto OrderDto = new OrderDto
                {
                    OrderID = Order.OrderID,
                    OrderDate = Order.OrderDate,
                    OrderSubtotal = Order.OrderSubtotal
                };
                
            }

            return Ok(OrderDtos);
        }

        /// <summary>
        /// Updates a Customer in the database given information about the Customer.
        /// </summary>
        /// <param name="id">The Customer id</param>
        /// <param name="Customer">A Team object. Received as POST data.</param>
        /// <returns></returns>
        /// <example>
        /// POST: api/CustomerData/UpdateCustomer/5
        /// FORM DATA: Customer JSON Object
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateCustomer(int id, [FromBody] Customer Customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != Customer.CustomerID)
            {
                return BadRequest();
            }

            db.Entry(Customer).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(id))
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
        /// Adds a Customer to the database.
        /// </summary>
        /// <param name="customer">A Customer object. Sent as POST form data.</param>
        /// <returns>status code 200 if successful. 400 if unsuccessful</returns>
        /// <example>
        /// POST: api/CustomerData/AddCustomer
        ///  FORM DATA: Customer JSON Object
        /// </example>
        [ResponseType(typeof(Customer))]
        [HttpPost]
        public IHttpActionResult AddCustomer([FromBody] Customer customer)
        {
            //Will Validate according to data annotations specified on model
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Customers.Add(customer);
            db.SaveChanges();

            return Ok(customer.CustomerID);
        }

        /// <summary>
        /// Deletes a customer in the database
        /// </summary>
        /// <param name="id">The id of the customer to delete.</param>
        /// <returns>200 if successful. 404 if not successful.</returns>
        /// <example>
        /// POST: api/CustomerData/DeleteCustomer/5
        /// </example>
        [HttpPost]
        public IHttpActionResult DeleteCustomer(int id)
        {
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return NotFound();
            }

            db.Customers.Remove(customer);
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
        /// Finds a customer in the system. Internal use only.
        /// </summary>
        /// <param name="id">The customer id</param>
        /// <returns>TRUE if the customer exists, false otherwise.</returns>
        private bool CustomerExists(int id)
        {
            return db.Customers.Count(e => e.CustomerID == id) > 0;
        }
    }
}