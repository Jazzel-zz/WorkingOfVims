using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using VIMS.Models;

namespace VIMS.Controllers
{
    public class VehicleInformationsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: VehicleInformations
        public ActionResult Index()
        {
            var vehicleInformations = db.VehicleInformations.Include(v => v.ApplicationUser);
            return View(vehicleInformations.ToList());
        }

        [HttpGet]
        public ActionResult UserIndex()
        {
            return View(db.VehicleInformations.ToList());
        }
        [HttpPost]
        public ActionResult UserIndex(string search)
        {
            if (search != "")
            {
                ViewBag.Search = search;
                return View(db.VehicleInformations.Where(find => find.VehicleName.Contains(search) || find.VehicleModel == search || find.VehicleColor.Contains(search)));
            }
            else
            {
                return View(db.VehicleInformations.ToList());
            }

        }

        // GET: VehicleInformations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VehicleInformation vehicleInformation = db.VehicleInformations.Find(id);
            ViewBag.Images = vehicleInformation.VehicleImages;
            if (vehicleInformation == null)
            {
                return HttpNotFound();
            }
            return View(vehicleInformation);
        }

        // GET: VehicleInformations/Create
        public ActionResult Create()
        {
            ViewBag.ApplicationUserId = new SelectList(db.Users, "Id", "Name");
            return View();
        }

        // POST: VehicleInformations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,VehicleCompany,VehicleName,ApplicationUserId,VehicleColor,VehicleModel,VehicleRate,VehicleBodyNumber,VehicleEngineNumber,VehicleNumber,VehicleWarranty,VehicleDescription,VehicleImages,VehicleStatus")] VehicleInformation vehicleInformation, IEnumerable<HttpPostedFileBase> file)
        {
            vehicleInformation.VehicleStatus = true;
            //vehicleInformation.VehicleWarranty = "5 years";
            string upload = "";
            foreach (var item in file)
            {
                if (item == null)
                {
                    break;
                }
                string filename = Guid.NewGuid() + Path.GetExtension(item.FileName);
                string filepath = "../../Data/" + filename;
                item.SaveAs(Path.Combine(Server.MapPath("~/Data"), filename));
                if (upload == "")
                {
                    upload = filepath;
                }
                else
                {
                    upload += ":" + filepath;
                }
            }
            vehicleInformation.VehicleImages = upload;
            if (ModelState.IsValid)
            {
                db.VehicleInformations.Add(vehicleInformation);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ApplicationUserId = new SelectList(db.Users, "Id", "Name", vehicleInformation.ApplicationUserId);
            return View(vehicleInformation);
        }

        // GET: VehicleInformations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VehicleInformation vehicleInformation = db.VehicleInformations.Find(id);
            ViewBag.Images = vehicleInformation.VehicleImages;
            if (vehicleInformation == null)
            {
                return HttpNotFound();
            }
            ViewBag.ApplicationUserId = new SelectList(db.Users, "Id", "Name", vehicleInformation.ApplicationUserId);
            return View(vehicleInformation);
        }

        // POST: VehicleInformations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,VehicleCompany,VehicleName,ApplicationUserId,VehicleColor,VehicleModel,VehicleRate,VehicleBodyNumber,VehicleEngineNumber,VehicleNumber,VehicleDescription,VehicleImages,VehicleStatus")] VehicleInformation vehicleInformation, string images, IEnumerable<HttpPostedFileBase> file)
        {
            string upload = "";
            foreach (var item in file)
            {
                if (item == null)
                {
                    break;
                }
                string filename = Guid.NewGuid() + Path.GetExtension(item.FileName);
                string filepath = "../../Data/" + filename;
                item.SaveAs(Path.Combine(Server.MapPath("~/Data"), filename));
                if (upload == "")
                {
                    upload = filepath;
                }
                else
                {
                    upload += ":" + filepath;
                }
            }
            string[] moreImages = images.Split(':');
            foreach (var image in moreImages)
            {
                if (image == null)
                {
                    break;
                }
                if (upload == "")
                {
                    upload = image;
                }
                else
                {
                    upload += ":" + image;
                }
            }
            vehicleInformation.VehicleImages = upload;
            if (ModelState.IsValid)
            {
                db.Entry(vehicleInformation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ApplicationUserId = new SelectList(db.Users, "Id", "Name", vehicleInformation.ApplicationUserId);
            return View(vehicleInformation);
        }

        // GET: VehicleInformations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VehicleInformation vehicleInformation = db.VehicleInformations.Find(id);
            if (vehicleInformation == null)
            {
                return HttpNotFound();
            }
            return View(vehicleInformation);
        }

        // POST: VehicleInformations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            VehicleInformation vehicleInformation = db.VehicleInformations.Find(id);
            ViewBag.Images = vehicleInformation.VehicleImages;
            db.VehicleInformations.Remove(vehicleInformation);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
