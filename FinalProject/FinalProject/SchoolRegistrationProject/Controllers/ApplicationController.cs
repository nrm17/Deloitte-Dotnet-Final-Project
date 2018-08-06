using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SchoolRegistrationProject.Models;

namespace SchoolRegistrationProject.Controllers
{
    public class ApplicationController : Controller
    {
        OAF_DBEntities7 db = new OAF_DBEntities7();
        // GET: Application
        [HttpGet]
        public ActionResult HomePage()
        {
            return View();
        }
     
        [HttpGet]
        public ActionResult InsertApplication()
        {
            var data = new SelectList(db.Branches, "branch_id", "location");
            Session["branchdata"] = data;
            return View();
        }

        [HttpPost]
        public ActionResult InsertApplication(Application ap, Processed_Application pa)
        {
           
                ap.branch_id = int.Parse(Request.Form["ddlbranch"].ToString());
                ap.classid = int.Parse(Request.Form["ddlclassid"].ToString());
                ap.name = Request.Form["txtname"].ToString();
                ap.address = Request.Form["txtaddress"].ToString();
                ap.age = int.Parse(Request.Form["txtage"].ToString());
                ap.dob = DateTime.Parse(Request.Form["txtdob"].ToString());

                pa.comments = "Submitted, But Not Yet Procesed";
                db.Applications.Add(ap);
                db.Processed_Applications.Add(pa);
                var res = db.SaveChanges();
                if (res > 0)
                {
                    ModelState.AddModelError("", "Data Inserted");
                    ModelState.AddModelError("", "Your Application has been submitted. Your Application id = " + ap.Id);

                }
            
            return View();


        }

        [HttpGet]
        public ActionResult Status()
        {



            return View();
        }

        [HttpPost]
        public ActionResult Status(Processed_Application pa)
        {
            int id = int.Parse(Request.Form["txtid"].ToString());
            var data = db.Processed_Applications.Where(x => x.applicationid == id).SingleOrDefault();
            string st = data.comments;
            string pt = data.resolvedby;
            if (st == "Submitted, But Not Yet Procesed")
            {
                ModelState.AddModelError("", st + " Check Later.");

            }
            else
            {
                ModelState.AddModelError("", st + " " +  "Please Contact" + " " +  pt);
            }
        



            return View();
        }

        [HttpGet]
        public ActionResult StatusUpdate()
        {
            return View();
        }

        [HttpPost]
        public ActionResult StatusUpdate(Processed_Application p, string Command)
        {
            if (Command == "Status")
            {
                int id = int.Parse(Request.Form["txtid"].ToString());
                var data = db.Processed_Applications.Where(x => x.applicationid == id).SingleOrDefault();
                ModelState.AddModelError("", data.comments);
                //Request.Form["txtid"] = data.comments;
                return View();
            }

            if (Command == "Update")
            {
                int id = int.Parse(Request.Form["txtid"].ToString());
                var data = db.Processed_Applications.Where(x => x.applicationid == id).SingleOrDefault();
                string comment = Request.Form["txtcomment"].ToString();
                data.comments = comment;
                data.date_of_resolve = DateTime.Now;

                var status = (from t in db.Applications
                              join b in db.Branches on t.branch_id equals b.branch_id
                              where t.Id == id
                              select b.contact_person).SingleOrDefault();

                data.resolvedby = status;

                var res = db.SaveChanges();
                if (res > 0)
                {
                    ModelState.AddModelError("", "Status Updated");
                   

                }
                return View();
            }
            return View();
        }

        [HttpGet]
        public ActionResult Report()
        {
            
            


            var data = from t in db.Applications
                       join p in db.Processed_Applications on t.Id equals p.applicationid
                       
                       orderby t.Id ascending
                       select t;
            List<POCO> lst = new List<POCO>();
            foreach (var j in data)
            {
                POCO pm = new POCO();
                pm.Id = j.Id;
                pm.Name = j.name;
                pm.Age = j.age;
                pm.DOB = j.dob;
                pm.Address = j.address;
                pm.Branch_id = j.branch_id;
                pm.Class_id = j.classid;

                lst.Add(pm);
            }
            int data7 = db.Applications.Count();
            Session["totalcount"] = data7;
            int res = db.Processed_Applications.Count(x => x.comments == "Processed");
            Session["processedcount"] = res;
            // Session["datta"] = lst;
            return View(lst);
        }
        [HttpPost]
        public ActionResult Report(POCO poo)
        {
            if (Request.Form["ddlpr"].ToString() == "Processed")
            {

                var data = from t in db.Applications
                           join p in db.Processed_Applications on t.Id equals p.applicationid
                           where p.comments == "Processed"
                           orderby t.Id ascending
                           select t;
                List<POCO> lst = new List<POCO>();
                foreach (var j in data)
                {
                    POCO pm = new POCO();
                    pm.Id = j.Id;
                    pm.Name = j.name;
                    pm.Age = j.age;
                    pm.DOB = j.dob;
                    pm.Address = j.address;
                    pm.Branch_id = j.branch_id;
                    pm.Class_id = j.classid;

                    lst.Add(pm);
                }
                // Session["datta"] = lst;
                return View(lst);
            }
            else
            {
                var data1 = (from t in db.Applications
                             join p in db.Processed_Applications on t.Id equals p.applicationid
                             where p.comments != "Processed"
                             select t).ToList();

                Session["datta1"] = data1;
                List<POCO> lst = new List<POCO>();

                foreach (var j in data1)
                {
                    POCO pm = new POCO();
                    pm.Id = j.Id;
                    pm.Name = j.name;
                    pm.Age = j.age;
                    pm.DOB = j.dob;
                    pm.Address = j.address;
                    pm.Branch_id = j.branch_id;
                    pm.Class_id = j.classid;

                    lst.Add(pm);
                }
                // Session["datta"] = lst;
                return View(lst);

               
            }


        }
    }
}