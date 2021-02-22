using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Diagnostics;
using System.Web.Script.Serialization;
using PassionProject_2_N01434210.Models;
using PassionProject_2_N01434210.Models.ViewModels;

namespace PassionProject_2_N01434210.Controllers
{
    public class OrderController : Controller
    {
        //Http Client is the proper way to connect to a webapi
        //https://docs.microsoft.com/en-us/dotnet/api/system.net.http.httpclient?view=net-5.0

        private JavaScriptSerializer jss = new JavaScriptSerializer();
        private static readonly HttpClient client;


        static OrderController()
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false
            };
            client = new HttpClient(handler);
            //change this to match your own local port number
            client.BaseAddress = new Uri("https://localhost:44355/api/");
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));


            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ACCESS_TOKEN);

        }

        // GET: Order/List
        public ActionResult List()
        {
            string url = "Orderdata/getorders";
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                IEnumerable<OrderDto> SelectedOrders = response.Content.ReadAsAsync<IEnumerable<OrderDto>>().Result;
                return View(SelectedOrders);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        //GET: Order/Details/5
        public ActionResult Details(int id)
        {
            ShowOrder ViewModel = new ShowOrder();
            string url = "orderdata/findorder/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            //Can catch the status code (200 OK, 301 REDIRECT), etc.
            //Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                //Put data into Order data transfer object
                OrderDto SelectedOrder = response.Content.ReadAsAsync<OrderDto>().Result;
                ViewModel.Order = SelectedOrder;

                //We don't need to throw any errors if this is null
                //A team not having any players is not an issue.
                url = "orderdata/GetOrdersForCustomer/ " + id;
                response = client.GetAsync(url).Result;
                //Can catch the status code (200 OK, 301 REDIRECT), etc.
                //Debug.WriteLine(response.StatusCode);
                CustomerDto SelectedCustomer = response.Content.ReadAsAsync<CustomerDto>().Result;
                ViewModel.Allorders = SelectedCustomer;


                url = "customerdata/getproductsfororder/" + id;
                response = client.GetAsync(url).Result;
                //Can catch the status code (200 OK, 301 REDIRECT), etc.
                //Debug.WriteLine(response.StatusCode);
                //Put data into Team data transfer object
                IEnumerable<ProductDto> SelectedProducts = response.Content.ReadAsAsync<IEnumerable<ProductDto>>().Result;
                ViewModel.Orderproducts = SelectedProducts;

                return View(ViewModel);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }


        // GET: Order/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Order/Create
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Create(Order OrderInfo)
        {
            Debug.WriteLine(OrderInfo.OrderID);
            string url = "orderdata/addorder";
            Debug.WriteLine(jss.Serialize(OrderInfo));
            HttpContent content = new StringContent(jss.Serialize(OrderInfo));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {

                int orderid = response.Content.ReadAsAsync<int>().Result;
                return RedirectToAction("Details", new { id = orderid });
            }
            else
            {
                return RedirectToAction("Error");
            }


        }


        // GET: Order/Edit/5
        public ActionResult Edit(int id)
        {
            string url = "Orderdata/findOrder/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            //Can catch the status code (200 OK, 301 REDIRECT), etc.
            //Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                //Put data into Team data transfer object
                OrderDto SelectedOrder = response.Content.ReadAsAsync<OrderDto>().Result;
                return View(SelectedOrder);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // POST: Order/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Edit(int id, Order OrderInfo)
        {
            Debug.WriteLine(OrderInfo.OrderID);
            string url = "orderdata/updateorder/" + id;
            Debug.WriteLine(jss.Serialize(OrderInfo));
            HttpContent content = new StringContent(jss.Serialize(OrderInfo));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Details", new { id = id });
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Order/Delete/5
        [HttpGet]
        public ActionResult DeleteConfirm(int id)
        {
            string url = "orderdata/findorder/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            //Can catch the status code (200 OK, 301 REDIRECT), etc.
            //Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                //Put data into Team data transfer object
                OrderDto SelectedOrder = response.Content.ReadAsAsync<OrderDto>().Result;
                return View(SelectedOrder);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // POST: Order/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Delete(int id)
        {
            string url = "orderdata/deleteorder/" + id;
            //post body is empty
            HttpContent content = new StringContent("");
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            //Can catch the status code (200 OK, 301 REDIRECT), etc.
            //Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {

                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        public ActionResult Error()
        {
            return View();
        }
    
    }
}
