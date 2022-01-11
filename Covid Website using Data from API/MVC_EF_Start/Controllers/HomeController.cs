using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using MVC_EF_Start.DataAccess;
using Microsoft.AspNetCore.Mvc;
using MVC_EF_Start.Models;
using Newtonsoft.Json;

namespace MVC_EF_Start.Controllers
{
    public class HomeController : Controller
    {


        public ApplicationDbContext dbContext;

        public HomeController(ApplicationDbContext context)
        {
            dbContext = context;
        }
        //add new 1
        HttpClient httpClient;
        static string api_link = "https://data.cdc.gov/resource/hk9y-quqm.json?$where=`group`=%27By%20Total%27&state=United%20States&age_group=All%20Ages";
        HttpClient httpclient = new HttpClient();

        Covid_Conditions covid_conditions = new Covid_Conditions();

        Covid_Groups cglist = new Covid_Groups();

        public IActionResult Data()
        {
            httpclient.BaseAddress = new Uri(api_link);
            httpclient.DefaultRequestHeaders.Accept.Clear();
            HttpResponseMessage response = httpclient.GetAsync(api_link).GetAwaiter().GetResult();

            string covidData = null;

            //var responseTask = httpclient.GetAsync(api_link);
            //responseTask.Wait();
            //var result = responseTask.Result;
            if (response.IsSuccessStatusCode)
            {
                //Get the data from api and store it as string in covidData variable
                covidData = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            }
            if (!covidData.Equals(""))
            {
                // JsonConvert is part of the NewtonSoft.Json Nuget package which convert string to json
                covid_conditions.covid_list = JsonConvert.DeserializeObject<List<Covid_Condition>>(covidData);
                cglist.covid_group_list = JsonConvert.DeserializeObject<List<Covid_Group>>(covidData);

                /*              
                 *               cc.condition_group = JsonConvert.DeserializeObject<List<Covid_Group>>(covidData);
                */
            }

            cglist.covid_group_list = cglist.covid_group_list.Distinct().ToList();

            
            DatabaseExampleController d = new DatabaseExampleController(dbContext);


            //add the data to db
            if (d.dbContext.Covid_Conditions_data.ToList().Count == 0)
            {
                //post the data into database

                d.covidConditionPost(covid_conditions);
            }

            if (d.dbContext.Covid_Group_data.ToList().Count == 0)
            {
                //post the data into database
                d.covidGroupPost(cglist);

            }



            

            return View(d.dbContext.Covid_Conditions_data.ToList());
        }
        public IActionResult Condition(string val)
        {
            if (TempData.Count != 0)
            {
                //to display a message to the client like updated or deleted
                ViewBag.Message = TempData["shortMessage"].ToString();
            }
            return View(dbContext.Covid_Conditions_data.Where(c => c.condition_group == val));



           
        }


        public IActionResult Add()
        {

            /*DatabaseExampleController d = new DatabaseExampleController(dbContext);

            //fetch the records which match the given condition
            var covid1 = d.dbContext.Covid_Conditions_data.First();
            add covid1 in the view
*/

            return View();


        }

        [HttpPost]
        public IActionResult AddNew(Covid_Condition data)
        {
            var rec = dbContext.Covid_Conditions_data.FirstOrDefault();


            Covid_Condition d = new Covid_Condition();

            d.condition_group = data.condition_group;
            d.condition = data.condition;
            d.covid_19_deaths = data.covid_19_deaths;
            dbContext.Add(d);
            dbContext.SaveChanges();




            return RedirectToAction("Data");
        }



        public IActionResult Update(string cond)
        {
            DatabaseExampleController d = new DatabaseExampleController(dbContext);
            //fetch the records which match the given condition
            var covid1 = d.dbContext.Covid_Conditions_data.Where(c => c.condition == cond).First();

            return View(covid1);
        }
        [HttpPost]
        public IActionResult UpdateRecord(Covid_Condition data)
        {
            var rec = dbContext.Covid_Conditions_data.FirstOrDefault(x => x.CaseId == data.CaseId);

            if (rec != null)
            {
                rec.covid_19_deaths = data.covid_19_deaths;
                dbContext.SaveChanges();
            }

            return RedirectToAction("Condition", new { val = rec.condition_group });
        }


        public IActionResult Delete(string cond)
        {
            var rec = dbContext.Covid_Conditions_data.FirstOrDefault(x => x.condition == cond);
            if (rec != null)
            {
                dbContext.Covid_Conditions_data.Remove(rec);
                dbContext.SaveChanges();
                TempData["shortMessage"] = "Deleted Successfully";
            }

            return RedirectToAction("Condition", new { val = rec.condition_group });
        }

       
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult main()
        {
            return View();
        }

        public IActionResult helpfulresources()
        {
            return View();
        }

        public IActionResult AboutUs()
        {
            return View();
        }
        

        public ViewResult Chart()
        {

            DatabaseExampleController db = new DatabaseExampleController(dbContext);

            if (dbContext.Covid_Conditions_data.ToList().Count == 0)
            {
                db.covidConditionPost(covid_conditions);
            }

            var results = from p in dbContext.Covid_Conditions_data
                          group p by p.condition_group into g
                          select new { condition_group = g.Key, covid_19_deaths = g.Sum(c => Convert.ToInt64(c.covid_19_deaths)) };

            List<string> ChartLabels = new List<string>();
            ChartLabels = results.Select(p => p.condition_group).ToList();
            List<long> ChartData = new List<long>();
            ChartData = results.Select(p => p.covid_19_deaths).ToList();

            ChartModel Model = new ChartModel
            {
                Labels = String.Join(",", ChartLabels.Select(d => "'" + d + "'")),
                Data = String.Join(",", ChartData.Select(d => d)),
                Title = "Covid deaths by Condition Group"
            };
            return View(Model);
        }
    }
}






