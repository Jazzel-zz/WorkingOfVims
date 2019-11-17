using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VIMS.Models;

namespace WebApplication19.Controllers
{
    [Authorize(Roles = "Employee")]
    public class ReportingController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: Reporting
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Vehicle()
        {
            var data = from item in db.VehicleInformations
                                      orderby item.VehicleInformationId descending
                                      select item;
            return View(data);
        }
        public ActionResult Claim()
        {
            var data = from item in db.ClaimDetails
                       orderby item.ClaimDetailId descending
                       select item;
            return View(data);
        }
        public ActionResult Policy()
        {
            var data = from item in db.CustomerPolicyRecords
                       orderby item.Id descending
                       select item;
            return View(data);
        }
    }
}