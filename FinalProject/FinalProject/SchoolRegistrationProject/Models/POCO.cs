using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolRegistrationProject.Models
{
    public class POCO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public DateTime DOB { get; set; }
        public int Age { get; set; }
        public int Branch_id { get; set; }
        public int Class_id { get; set; }
        //  public string Comments { get; set; }

    }
}