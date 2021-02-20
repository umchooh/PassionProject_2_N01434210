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
    public class CustomerController : Controller
    {
        //Http Client is the proper way to connect to a webapi
        //https://docs.microsoft.com/en-us/dotnet/api/system.net.http.httpclient?view=net-5.0

        private JavaScriptSerializer jss = new JavaScriptSerializer();
        private static readonly HttpClient client;

        static CustomerController()
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


        // GET: Customer/List
        public ActionResult List()
        {
            string url = "customerdata/getcustomers";
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                IEnumerable<CustomerDto> SelectedCustomers = response.Content.ReadAsAsync<IEnumerable<CustomerDto>>().Result;
                return View(SelectedCustomers);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Customer/Details/5
        public ActionResult Details(int id)
        {
            ShowCustomer ViewModel = new ShowCustomer();
            string url = "customerdata/findcustomer/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            //Can catch the status code (200 OK, 301 REDIRECT), etc.
            //Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                //Put data into player data transfer object
                CustomerDto SelectedCustomer = response.Content.ReadAsAsync<CustomerDto>().Result;
                ViewModel.Customer = SelectedCustomer;


                url = "customerdata/findorderforcustomer/" + id;
                response = client.GetAsync(url).Result;
                OrderDto SelectedOrder = response.Content.ReadAsAsync<OrderDto>().Result;
                ViewModel.Order = SelectedOrder;

                return View(ViewModel);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Customer/Create 
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Customer/Create
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Create(Customer CustomerInfo)
        {
            Debug.WriteLine(CustomerInfo.CustomerFirstName);
            string url = "CustomerData/AddCustomer";
            Debug.WriteLine(jss.Serialize(CustomerInfo));
            HttpContent content = new StringContent(jss.Serialize(CustomerInfo));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {

                int Customerid = response.Content.ReadAsAsync<int>().Result;
                return RedirectToAction("Details", new { id = Customerid });
            }
            else
            {
                return RedirectToAction("Error");
            }


        }

        // GET: Customer/Edit/5
        public ActionResult Edit(int id)
        {
            UpdateCustomer ViewModel = new UpdateCustomer();

            string url = "customerdata/findcustomer/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            //Can catch the status code (200 OK, 301 REDIRECT), etc.
            //Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                //Put data into player data transfer object
                CustomerDto SelectedCustomer = response.Content.ReadAsAsync<CustomerDto>().Result;
                ViewModel.Customer = SelectedCustomer;

                //get information about teams this player COULD play for.
                url = "orderdata/getorders";
                response = client.GetAsync(url).Result;
                IEnumerable<OrderDto> PotentialOrders= response.Content.ReadAsAsync<IEnumerable<OrderDto>>().Result;
                ViewModel.Allorders = PotentialOrders;

                return View(ViewModel);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // POST: Customer/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Edit(int id, Customer CustomerInfo)
        {
            Debug.WriteLine(CustomerInfo.CustomerFirstName);
            string url = "customerdata/updatecustomer/" + id;
            Debug.WriteLine(jss.Serialize(CustomerInfo));
            HttpContent content = new StringContent(jss.Serialize(CustomerInfo));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Details", new { id = id });
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: customer/Delete/5
        [HttpGet]
        public ActionResult DeleteConfirm(int id)
        {
            string url = "customerdata/findcustomer/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            //Can catch the status code (200 OK, 301 REDIRECT), etc.
            //Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                //Put data into player data transfer object
                CustomerDto SelectedCustomer = response.Content.ReadAsAsync<CustomerDto>().Result;
                return View(SelectedCustomer);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // POST: Customer/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Delete(int id)
        {
            string url = "customerdata/deletecustomer/" + id;
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
