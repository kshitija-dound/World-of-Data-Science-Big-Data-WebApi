using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using System.Linq;
using System.Threading.Tasks;


namespace MVC_EF_Start.Models
{

    public class ChartModel
    {
        public string ChartType { get; set; }
        public string Labels { get; set; }
        public string Data { get; set; }
        public string Title { get; set; }
    }

    //json as class
    public class Covid_Group

    {
        [Key]
        public int grp_id { get; set; }
        public string condition_group { get; set; }

        public List<Covid_Condition> Covid_Condition { get; set; }

    }

    public class Covid_Groups

    {
        [Key]
        public int grp_id { get; set; }

        public List<Covid_Group> covid_group_list { get; set; }

    }

    public class Covid_Conditions
    {
        [Key]
        public int Id { get; set; }
        public List<Covid_Condition> covid_list { get; set; }
    }

    public class Covid_Condition
    {
        [Key]
        public int CaseId { get; set; }
        public Covid_Group grp_id { get; set; }
        public string condition_group { get; set; }
        public string condition { get; set; }
        public string age_group { get; set; }
        public string covid_19_deaths { get; set; }
        public Covid_Conditions Id { get; set; }
    }

    


}



