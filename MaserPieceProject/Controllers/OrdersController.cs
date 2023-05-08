using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MaserPieceProject.Models;

namespace MaserPieceProject.Controllers
{
    public class OrdersController : Controller
    {
        private masterpes3Entities1 db = new masterpes3Entities1();

        // GET: Orders
        public ActionResult Index()
        {
            var orders = db.Orders.Where(o => o.Isaccepted == null);
            return View(orders.ToList());
        }

        public ActionResult chared()
        {
           
            var orders = db.Orders.Where(o => o.Isaccepted == true);
            return View(orders.ToList());
        }
        //public ActionResult AcceptOrders(int orderid)
        //{
        //    //var ID = User.Identity.GetUserId();
        //    var orders1 = db.Orders.Include(o => o.AspNetUser);
        //    //var products = db.Products.Include(p => p.Category).Include(p => p.Resturant).Where(p=>p.r);
        //    var allorders = db.Orders.Where(p => p.OrderId == orderid & p.Isaccepted == null);
        //    var orders = db.Orders.Find(orderid);
        //    orders.Isaccepted = true;
        //    db.SaveChanges();
        //    //var orders = db.Orders.Where(p => p.ID==id).Select(p => p.Status);
        //    //orders.status
        //    return View("Index", allorders);
        //}



        // GET: Orders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // GET: Orders/Create
        public ActionResult Create()
        {
            ViewBag.Id = new SelectList(db.AspNetUsers, "Id", "Email");
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OrderId,OrderPrice,isCheckout,Id,Adress,Email,FirstName,LastName,Phone,City,OrderDate,PaymentMethod,Isaccepted")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Orders.Add(order);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Id = new SelectList(db.AspNetUsers, "Id", "Email", order.Id);
            return View(order);
        }

        // GET: Orders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id = new SelectList(db.AspNetUsers, "Id", "Email", order.Id);
            return View(order);
        }

        public ActionResult accept(int? id)
        {
            Order orderdone = db.Orders.Find(id);
            orderdone.Isaccepted = true;
            db.SaveChanges();
            return RedirectToAction("Index", "Orders");
        }





        public ActionResult reject(int? id)
        {
            Order orderdone = db.Orders.Find(id);
            orderdone.Isaccepted = false;
            db.SaveChanges();
            return RedirectToAction("Index", "Orders");

        }

        public ActionResult Searchacc(string search)
        {
            var x1 = db.Orders.Where(x =>( (x.City.Contains(search) || x.OrderDate.ToString().Contains(search)) && x.Isaccepted==null)).ToList() ;
            return View("Index", x1);
        }


        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OrderId,OrderPrice,isCheckout,Id,Adress,Email,FirstName,LastName,Phone,City,OrderDate,PaymentMethod,Isaccepted")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Id = new SelectList(db.AspNetUsers, "Id", "Email", order.Id);
            return View(order);
        }

        // GET: Orders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Order order = db.Orders.Find(id);
            db.Orders.Remove(order);
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
