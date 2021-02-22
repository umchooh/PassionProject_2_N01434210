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
    public class ProductController : Controller
    {
        //Http Client is the proper way to connect to a webapi
        //https://docs.microsoft.com/en-us/dotnet/api/system.net.http.httpclient?view=net-5.0

        private JavaScriptSerializer jss = new JavaScriptSerializer();
        private static readonly HttpClient client;


        static ProductController()
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

        // GET: Product/List
        public ActionResult List()
        {
            string url = "productdata/getproducts";
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                IEnumerable<ProductDto> SelectedTeams = response.Content.ReadAsAsync<IEnumerable<ProductDto>>().Result;
                return View(SelectedTeams);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        /*/ GET: Product/Details/5
        public ActionResult Details(int id)
        {
            UpdateSponsor ViewModel = new UpdateSponsor();

            string url = "sponsordata/findsponsor/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            //Can catch the status code (200 OK, 301 REDIRECT), etc.
            //Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                //Put data into Sponsor data transfer object
                SponsorDto SelectedSponsor = response.Content.ReadAsAsync<SponsorDto>().Result;
                ViewModel.sponsor = SelectedSponsor;

                //find teams that are sponsored by this sponsor
                url = "sponsordata/getteamsforsponsor/" + id;
                response = client.GetAsync(url).Result;

                //Put data into Sponsor data transfer object
                IEnumerable<TeamDto> SelectedTeams = response.Content.ReadAsAsync<IEnumerable<TeamDto>>().Result;
                ViewModel.sponsoredteams = SelectedTeams;

                return View(ViewModel);
            }
            else
            {
                return RedirectToAction("Error");

            }

        }
*/

        // GET: Product/Create
        public ActionResult Create()
        {
            return View();
        }
        

        // POST: Product/Create
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Create(Product ProductInfo)
        {
            Debug.WriteLine(ProductInfo.ProductName);
            string url = "Productdata/addProduct";
            Debug.WriteLine(jss.Serialize(ProductInfo));
            HttpContent content = new StringContent(jss.Serialize(ProductInfo));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {

                int Productid = response.Content.ReadAsAsync<int>().Result;
                return RedirectToAction("Details", new { id = Productid });
            }
            else
            {
                return RedirectToAction("Error");
            }


        }

        // GET: Product/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Product/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Product/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Product/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
