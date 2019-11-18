using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
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
        public ActionResult Vehicle(string type)
        {
            var data = from item in db.VehicleInformations
                       orderby item.VehicleInformationId descending
                       select item;
            return View(data);
        }
        public ActionResult Claim(string type)
        {
            var data = from item in db.ClaimDetails
                       orderby item.ClaimDetailId descending
                       select item;
            return View(data);
        }

        public ActionResult Policy(string type, int month)
        {
            ICollection<CustomerPolicyRecord> policyRecord = null;
            DateTime date = DateTime.Now.Date;
            // Show All
            if (type == "" && month == 0)
            {
                policyRecord = db.CustomerPolicyRecords.OrderByDescending(x => x.Id).ToList();

            }
            // Show Today's Record
            else if (type == "t" && month == 0)
            {
                policyRecord = db.CustomerPolicyRecords.Where(x => EntityFunctions.TruncateTime(x.PolicyDate) == date).OrderByDescending(x => x.Id).ToList();
            }
            // Fixing clash between month and today
            else if (type == "t" && month != 0)
            {
                return HttpNotFound();
            }
            // Show Monthly Record
            else if (type == "m" && month != 0)
            {
                try
                {
                    if (month == 0)
                    {
                        //policyRecord = from item in db.CustomerPolicyRecords
                        //       orderby item.PolicyDate.Month descending
                        //       select item;
                    }
                    else if (month != 0)
                    {
                        //policyRecord = from item in db.CustomerPolicyRecords
                        //       orderby item.PolicyDate.Month descending
                        //       where item.PolicyDate.Month == month
                        //       select item;
                    }
                    else
                    {
                        //policyRecord = from item in db.CustomerPolicyRecords
                        //       orderby item.PolicyDate.Month descending
                        //       select item;
                    }
                }
                catch (Exception error)
                {
                    // monthly  order -- DIY
                    policyRecord = db.CustomerPolicyRecords.OrderByDescending(x => x.Id).ToList();

                }


            }
            // Fixing clash between month(0) and type
            else if (type == "m" && month == 0)
            {
                return HttpNotFound();
            }
            // Just to Complete the Rule
            else
            {
                policyRecord = db.CustomerPolicyRecords.OrderByDescending(x => x.Id).ToList();

            }
            return View(policyRecord);
        }

    }

}