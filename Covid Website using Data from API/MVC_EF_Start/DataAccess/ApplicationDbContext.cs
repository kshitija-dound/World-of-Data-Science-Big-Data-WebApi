using Microsoft.EntityFrameworkCore;
using MVC_EF_Start;
using MVC_EF_Start.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC_EF_Start.DataAccess
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :
            base(options)
        { }

        /*public DbSet<Parks> Parks { get; set; }
        public DbSet<Models.Park> Park { get; set; }*/
        public DbSet<Covid_Condition> Covid_Conditions_data { get; set; }

        public DbSet<Covid_Conditions> Covid_Conditions { get; set; }
        public DbSet<Covid_Group> Covid_Group_data { get; set; }

        public DbSet<Covid_Groups> Covid_Groups { get; set; }

    }
}