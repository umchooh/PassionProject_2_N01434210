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
    public class ProductDataController : ApiController
    {
        //The Database Access Point
        private PassionDbContext db = new PassionDbContext();

        // This api contorller form form WebAPI2 with read/write action with EF.

        /// <summary>
        /// Gets a list of Customer in the database alongside a status code (200 OK)
        /// </summary>
        /// <returns>A list of Orders including their ID, Date and Subtotal</returns>
        ///<example> GET: api/ProductData/GetProducts </example>
        [ResponseType(typeof(IEnumerable<ProductDto>))]
        public IEnumerable<ProductDto> GetProducts()
        {
            List<Product> Products = db.Products.ToList();
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

            return (ProductDtos);

        }

        /// <summary>
        /// Finds a particular Product in the database with a 200 status code. If the customer is not found, return 404.
        /// </summary>
        /// <param name="id">The product id</param>
        /// <returns>Information about the player, including player id, bio, first and last name, and teamid</returns>
        // <example>
        // GET: api/ProductData/FindProduct/5
        // </example>
        [HttpGet]
        [ResponseType(typeof(ProductDto))]
        public IHttpActionResult FindProduct(int id)
        {
            //Find a product Data
            Product Product = db.Products.Find(id);
            //If not found, return 404 status code
            if (Product == null)
            {
                return NotFound();
            }

            //put into a 'friendly object format'
            ProductDto ProductDto = new ProductDto
            {
                ProductID = Product.ProductID,
                ProductName = Product.ProductName,
                ProductDescription = Product.ProductDescription,
                ProductPrice = Product.ProductPrice
            };

            //pass along data as 200 status code OK response
            return Ok(ProductDto);
        }
    }
}