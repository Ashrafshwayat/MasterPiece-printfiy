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
    public class orderItemsController : Controller
    {
        private masterpes3Entities1 db = new masterpes3Entities1();

        // GET: orderItems
        public ActionResult Index()
        {
            var orderItems = db.orderItems.Where(o => o.Shipping==null).Include(o => o.Product);
            return View(orderItems.ToList());
        }




        public ActionResult chared()
        {

              var orderItems = db.orderItems.Where(o => o.Shipping==true).Include(o => o.Product);
            return View(orderItems.ToList());
        }


        public ActionResult Searchacc(string search)
        {
            var x1 = db.orderItems.Where(x => ((x.Product.ProductName.Contains(search) || x.Order.City.ToString().Contains(search)) && x.Shipping == null)).ToList();
            return View("Index", x1);
        }


        // GET: orderItems/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            orderItem orderItem = db.orderItems.Find(id);
            if (orderItem == null)
            {
                return HttpNotFound();
            }
            return View(orderItem);
        }

        // GET: orderItems/Create
        public ActionResult Create()
        {
            ViewBag.OrderId = new SelectList(db.Orders, "OrderId", "Id");
            ViewBag.ProductId = new SelectList(db.Products, "ProductId", "ProductName");
            return View();
        }

        // POST: orderItems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "orderItemsId,Quantity,OrderId,ProductId,color,size,Shipping")] orderItem orderItem)
        {
            if (ModelState.IsValid)
            {
                db.orderItems.Add(orderItem);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.OrderId = new SelectList(db.Orders, "OrderId", "Id", orderItem.OrderId);
            ViewBag.ProductId = new SelectList(db.Products, "ProductId", "ProductName", orderItem.ProductId);
            return View(orderItem);
        }

        // GET: orderItems/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            orderItem orderItem = db.orderItems.Find(id);
            if (orderItem == null)
            {
                return HttpNotFound();
            }
            ViewBag.OrderId = new SelectList(db.Orders, "OrderId", "Id", orderItem.OrderId);
            ViewBag.ProductId = new SelectList(db.Products, "ProductId", "ProductName", orderItem.ProductId);
            return View(orderItem);
        }

         public ActionResult accept(int? id)
        {
            orderItem orderItem = db.orderItems.Find(id);
            //Order orderdone = db.Orders.Find(id);
            orderItem.Shipping= true;
            db.SaveChanges();
            return RedirectToAction("Index", "orderItems");
        }





        public ActionResult reject(int? id)
        {
            orderItem orderItem = db.orderItems.Find(id);
            //Order orderdone = db.Orders.Find(id);
            orderItem.Shipping = false;
            db.SaveChanges();
            return RedirectToAction("Index", "orderItems");

        }




        // POST: orderItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "orderItemsId,Quantity,OrderId,ProductId,color,size,Shipping")] orderItem orderItem)
        {
            if (ModelState.IsValid)
            {
                db.Entry(orderItem).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.OrderId = new SelectList(db.Orders, "OrderId", "Id", orderItem.OrderId);
            ViewBag.ProductId = new SelectList(db.Products, "ProductId", "ProductName", orderItem.ProductId);
            return View(orderItem);
        }

        // GET: orderItems/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            orderItem orderItem = db.orderItems.Find(id);
            if (orderItem == null)
            {
                return HttpNotFound();
            }
            return View(orderItem);
        }

        // POST: orderItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            orderItem orderItem = db.orderItems.Find(id);
            db.orderItems.Remove(orderItem);
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
