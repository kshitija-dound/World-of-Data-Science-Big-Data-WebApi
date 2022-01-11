

using MVC_EF_Start.DataAccess;
using MVC_EF_Start.Controllers;
using MVC_EF_Start.Models;

using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MVC_EF_Start.Controllers
{
    public class ChartController : Controller
    {
        public ApplicationDbContext dbContext;

        public ChartController(ApplicationDbContext context)
        {
            dbContext = context;
        }
        static string api_link = "https://data.cdc.gov/resource/hk9y-quqm.json?$where=`group`=%27By%20Total%27&state=United%20States&age_group=All%20Ages";

        HttpClient httpclient = new HttpClient();

        Covid_Conditions covid_conditions = new Covid_Conditions();

        public ViewResult Chart()
        {
            httpclient.BaseAddress = new Uri(api_link);

            HttpResponseMessage response = httpclient.GetAsync(api_link).GetAwaiter().GetResult();
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
                ChartType = "bar",
                Labels = String.Join(",", ChartLabels.Select(d => "'" + d + "'")),
                Data = String.Join(",", ChartData.Select(d => d)),
                Title = "Covid deaths by Condition Group"
            };
            return View(Model);
            }
    }
}

