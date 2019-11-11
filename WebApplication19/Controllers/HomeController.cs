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
            var FetchMonthlyVehicles = (from item in db.VehicleInformations
                                        group item by new { month = item.RegistrationDate.Month } into g
                                        select g).ToArray()
                        .Select(p => new
                        {
                            January = p.Where(x => x.RegistrationDate.Month == 1).Count(),
                            February = p.Where(x => x.RegistrationDate.Month == 2).Count(),
                            March = p.Where(x => x.RegistrationDate.Month == 3).Count(),
                            April = p.Where(x => x.RegistrationDate.Month == 4).Count(),
                            May = p.Where(x => x.RegistrationDate.Month == 5).Count(),
                            June = p.Where(x => x.RegistrationDate.Month == 6).Count(),
                            July = p.Where(x => x.RegistrationDate.Month == 7).Count(),
                            August = p.Where(x => x.RegistrationDate.Month == 8).Count(),
                            September = p.Where(x => x.RegistrationDate.Month == 9).Count(),
                            October = p.Where(x => x.RegistrationDate.Month == 10).Count(),
                            November = p.Where(x => x.RegistrationDate.Month == 11).Count(),
                            December = p.Where(x => x.RegistrationDate.Month == 12).Count(),
                        });
            string data = "";
            string VehicleData = "";
            foreach (var item in FetchMonthlyPolicies)
            {
                data += item;
            }
            foreach (var item in FetchMonthlyVehicles)
            {
                VehicleData += item;
            }
            data = data.Replace("{", "");
            data = data.Replace("}", "");
            data = data.Replace(" ", "");
            data = data.Replace(",", ":");
            data = data.Replace("=", ":");
            VehicleData = VehicleData.Replace("{", "");
            VehicleData = VehicleData.Replace("}", "");
            VehicleData = VehicleData.Replace(" ", "");
            VehicleData = VehicleData.Replace(",", ":");
            VehicleData = VehicleData.Replace("=", ":");
            string[] valuesArray = data.Split(':');
            string[] valuesVehicleArray = VehicleData.Split(':');
            string values = "";
            string VehicleValues = "";
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
            foreach (var item in valuesVehicleArray)
            {
                if (IsNumeric(item))
                {
                    if (VehicleValues == "")
                    {
                        VehicleValues = item;
                    }
                    else
                    {
                        VehicleValues += ":" + item;
                    }
                }
            }
            ViewBag.MonthlyData = values;
            ViewBag.MonthlyVehicleData = VehicleValues;

            dynamic VIMSINFOMODEL = new ExpandoObject();
            VIMSINFOMODEL.Vehicles = db.VehicleInformations.OrderByDescending(find => find.VehicleInformationId).Take(5);
            VIMSINFOMODEL.Policies = db.PolicyTypes.OrderByDescending(find => find.PolicyTypeId).Take(5);
            VIMSINFOMODEL.Applications = db.CustomerPolicyRecords.OrderByDescending(find => find.Id).Take(5);
            VIMSINFOMODEL.Expenses = db.Expenses.OrderByDescending(find => find.ExpenseId).Take(5);
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