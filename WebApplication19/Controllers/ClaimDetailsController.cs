using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using VIMS.Models;

namespace WebApplication19.Controllers
{
    public class ClaimDetailsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ClaimDetails
        public ActionResult Index()
        {
            var claimDetails = db.ClaimDetails.Include(c => c.CustomerPolicyRecord);
            return View(claimDetails.ToList());
        }

        // GET: ClaimDetails/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ClaimDetail claimDetail = db.ClaimDetails.Find(id);
            if (claimDetail == null)
            {
                return HttpNotFound();
            }
            return View(claimDetail);
        }

        // GET: ClaimDetails/Create
        public ActionResult Create()
        {
            ViewBag.CustomerPolicyRecordId = new SelectList(db.CustomerPolicyRecords, "Id", "ApplicationUserId");
            Random generator = new Random();
            String generated_code = "";
            var estimateCodes = from item in db.Estimates
                                select item;
            bool quit = false;
            while (quit != true)
            {
                generated_code = generator.Next(0, 999999).ToString("D6");

                if (estimateCodes.Count() == 0)
                {
                    quit = true;
                    ViewBag.Code = "E-" + generated_code;

                }
                else
                {
                    foreach (var item in estimateCodes)
                    {
                        if (generated_code != item.EstimateNumber)
                        {
                            ViewBag.Code = "E-" + generated_code;
                            quit = true;

                        }
                    }

                }
            }
            return View();
        }

        // POST: ClaimDetails/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ClaimDetailId,ClaimNumber,CustomerPolicyRecordId,PlaceOfAccident,DateOfAccident,InsuredAmount,ClaimableAmount")] ClaimDetail claimDetail)
        {
            if (ModelState.IsValid)
            {
                db.ClaimDetails.Add(claimDetail);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CustomerPolicyRecordId = new SelectList(db.CustomerPolicyRecords, "Id", "ApplicationUserId", claimDetail.CustomerPolicyRecordId);
            return View(claimDetail);
        }

        // GET: ClaimDetails/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ClaimDetail claimDetail = db.ClaimDetails.Find(id);
            if (claimDetail == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerPolicyRecordId = new SelectList(db.CustomerPolicyRecords, "Id", "ApplicationUserId", claimDetail.CustomerPolicyRecordId);
            return View(claimDetail);
        }

        // POST: ClaimDetails/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ClaimDetailId,ClaimNumber,CustomerPolicyRecordId,PlaceOfAccident,DateOfAccident,InsuredAmount,ClaimableAmount")] ClaimDetail claimDetail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(claimDetail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CustomerPolicyRecordId = new SelectList(db.CustomerPolicyRecords, "Id", "ApplicationUserId", claimDetail.CustomerPolicyRecordId);
            return View(claimDetail);
        }

        // GET: ClaimDetails/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ClaimDetail claimDetail = db.ClaimDetails.Find(id);
            if (claimDetail == null)
            {
                return HttpNotFound();
            }
            return View(claimDetail);
        }

        // POST: ClaimDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ClaimDetail claimDetail = db.ClaimDetails.Find(id);
            db.ClaimDetails.Remove(claimDetail);
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
