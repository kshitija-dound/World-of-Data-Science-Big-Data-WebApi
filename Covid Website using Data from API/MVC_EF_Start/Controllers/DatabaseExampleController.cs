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
    public class DatabaseExampleController : Controller
    {
        public ApplicationDbContext dbContext;

        public DatabaseExampleController(ApplicationDbContext context)
        {
            dbContext = context;
        }


        public void covidConditionPost(Covid_Conditions covid1)
        {
            dbContext.Covid_Conditions.Add(covid1);
            dbContext.SaveChanges();
        }

        public void covidGroupPost(Covid_Groups cg1)
        {
            dbContext.Covid_Groups.Add(cg1);
            dbContext.SaveChanges();
        }
    }
}