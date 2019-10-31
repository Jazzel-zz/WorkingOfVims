﻿using System;
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
    public class CustomerBillingInformationsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: CustomerBillingInformations
        public ActionResult Index()
        {
            var customerBillingInformations = db.CustomerBillingInformations.Find(TempData["id"]);
            return View(customerBillingInformations);
        }

        // GET: CustomerBillingInformations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomerBillingInformation customerBillingInformation = db.CustomerBillingInformations.Find(id);
            if (customerBillingInformation == null)
            {
                return HttpNotFound();
            }
            return View(customerBillingInformation);
        }

        // GET: CustomerBillingInformations/Create
        public ActionResult Create()
        {
            ViewBag.GetCustomerPolicyRecordId = new SelectList(db.CustomerPolicyRecords, "Id", "ApplicationUserId");
            return View();
        }

        // POST: CustomerBillingInformations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,BillNumber,GetCustomerPolicyRecordId")] CustomerBillingInformation customerBillingInformation)
        {
            if (ModelState.IsValid)
            {
                db.CustomerBillingInformations.Add(customerBillingInformation);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.GetCustomerPolicyRecordId = new SelectList(db.CustomerPolicyRecords, "Id", "ApplicationUserId", customerBillingInformation.GetCustomerPolicyRecordId);
            return View(customerBillingInformation);
        }

        // GET: CustomerBillingInformations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomerBillingInformation customerBillingInformation = db.CustomerBillingInformations.Find(id);
            if (customerBillingInformation == null)
            {
                return HttpNotFound();
            }
            ViewBag.GetCustomerPolicyRecordId = new SelectList(db.CustomerPolicyRecords, "Id", "ApplicationUserId", customerBillingInformation.GetCustomerPolicyRecordId);
            return View(customerBillingInformation);
        }

        // POST: CustomerBillingInformations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,BillNumber,GetCustomerPolicyRecordId")] CustomerBillingInformation customerBillingInformation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customerBillingInformation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.GetCustomerPolicyRecordId = new SelectList(db.CustomerPolicyRecords, "Id", "ApplicationUserId", customerBillingInformation.GetCustomerPolicyRecordId);
            return View(customerBillingInformation);
        }

        // GET: CustomerBillingInformations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomerBillingInformation customerBillingInformation = db.CustomerBillingInformations.Find(id);
            if (customerBillingInformation == null)
            {
                return HttpNotFound();
            }
            return View(customerBillingInformation);
        }

        // POST: CustomerBillingInformations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CustomerBillingInformation customerBillingInformation = db.CustomerBillingInformations.Find(id);
            db.CustomerBillingInformations.Remove(customerBillingInformation);
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