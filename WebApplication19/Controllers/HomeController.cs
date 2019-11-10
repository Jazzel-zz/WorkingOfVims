using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using VIMS.Models;

namespace VIMS.Controllers
{
    [Authorize(Roles = "Employee,Customer")]
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult UserIndex()
        {
            return View();
        }
        public ActionResult Dashboard()
        {
            DateTime today = DateTime.Today;
            int countPolicyApplications = (from item in db.CustomerPolicyRecords
                                           select item).Count();
            ViewBag.CountPolicy = countPolicyApplications; 
            var FetchMonthlyPolicies = (from item in db.CustomerPolicyRecords
                                        group item by new { month = item.PolicyDate.Month } into g
                                        select g).ToArray()
                        .Select(p => new
                        {
                            January = p.Where(x => x.PolicyDate.Month == 1).Count(),
                            February = p.Where(x => x.PolicyDate.Month == 2).Count(),
                            March = p.Where(x => x.PolicyDate.Month == 3).Count(),
                            April = p.Where(x => x.PolicyDate.Month == 4).Count(),
                            May = p.Where(x => x.PolicyDate.Month == 5).Count(),
                            June = p.Where(x => x.PolicyDate.Month == 6).Count(),
                            July = p.Where(x => x.PolicyDate.Month == 7).Count(),
                            August = p.Where(x => x.PolicyDate.Month == 8).Count(),
                            September = p.Where(x => x.PolicyDate.Month == 9).Count(),
                            October = p.Where(x => x.PolicyDate.Month == 10).Count(),
                            November = p.Where(x => x.PolicyDate.Month == 11).Count(),
                            December = p.Where(x => x.PolicyDate.Month == 12).Count(),
                        });
            //"January=0,February=0,March=0,April=0,May=0,June=0,July=0,August=0,September=0,October=0,November=1,December=0"
            //"January:0:February:0:March:0:April:0:May:0:June:0:July:0:August:0:September:0:October:0:November:1:December:0"
            string data = "";
            foreach (var item in FetchMonthlyPolicies)
            {
                item.GetType();
                data += item;
            }
            data = data.Replace("{", "");
            data = data.Replace("}", "");
            data = data.Replace(" ", "");
            data = data.Replace(",", ":");
            data = data.Replace("=", ":");
            string[] valuesArray = data.Split(':');
            string values = "";
            foreach (var item in valuesArray)
            {
                if (IsNumeric(item))
                {
                    if (values == "")
                    {
                        values = item;
                    }
                    else
                    {
                        values += ":" + item;
                    }
                }
            }
            ViewBag.MonthlyData = values;


            dynamic VIMSINFOMODEL = new ExpandoObject();
            VIMSINFOMODEL.Vehicles = db.VehicleInformations.ToList();
            VIMSINFOMODEL.Policies = db.PolicyTypes.ToList();
            VIMSINFOMODEL.Applications = db.CustomerPolicyRecords.ToList();
            VIMSINFOMODEL.Expenses = db.Expenses.ToList();
            return View(VIMSINFOMODEL);

        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Testimonial()
        {

            return View();
        }
        public bool IsNumeric(string value)
        {
            return value.All(char.IsNumber);
        }
    }
}