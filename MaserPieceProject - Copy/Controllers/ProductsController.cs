using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using MaserPieceProject.Models;

namespace MaserPieceProject.Controllers
{
    public class ProductsController : Controller
    {
        private masterpes3Entities1 db = new masterpes3Entities1();

        // GET: Products



        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            var products = db.Products.Include(p => p.Category).Include(p => p.Color).Include(p => p.Size);
            return View(products.ToList());
        }

        public ActionResult Searchacc(string search)
        {
            var x1 = db.Products.Where(x => (x.ProductName.Contains(search) || x.ProductDescription.Contains(search))).ToList();
            return View("Index", x1);
        }

        // GET: Products/Details/5
        [Authorize(Roles = "Admin")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName");
            ViewBag.colorid = new SelectList(db.Colors, "colorId", "name");
            ViewBag.Sizeid = new SelectList(db.Sizes, "sizeid", "name");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "ProductId,ProductName,ProductDescription,ProductImage,price,Quantity,CategoryId,colorid,Sizeid")] Product product , HttpPostedFileBase imge)
        {
            if (ModelState.IsValid)
            {
                if (imge != null && imge.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(imge.FileName);
                    var path = Path.Combine(Server.MapPath("~/images"), fileName);
                    imge.SaveAs(path);
                    product.ProductImage = fileName;
                }
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName", product.CategoryId);
            ViewBag.colorid = new SelectList(db.Colors, "colorId", "name", product.colorid);
            ViewBag.Sizeid = new SelectList(db.Sizes, "sizeid", "name", product.Sizeid);
            return View(product);
        }

        // GET: Products/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName", product.CategoryId);
            ViewBag.colorid = new SelectList(db.Colors, "colorId", "name", product.colorid);
            ViewBag.Sizeid = new SelectList(db.Sizes, "sizeid", "name", product.Sizeid);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductId,ProductName,ProductDescription,ProductImage,price,Quantity,CategoryId,colorid,Sizeid")] Product product , HttpPostedFileBase imge)
        {
            if (ModelState.IsValid)
            {
                if (imge != null && imge.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(imge.FileName);
                    var path = Path.Combine(Server.MapPath("~/images"), fileName);
                    imge.SaveAs(path);
                    product.ProductImage = fileName;
                }
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "CategoryId", "CategoryName", product.CategoryId);
            ViewBag.colorid = new SelectList(db.Colors, "colorId", "name", product.colorid);
            ViewBag.Sizeid = new SelectList(db.Sizes, "sizeid", "name", product.Sizeid);
            return View(product);
        }

        // GET: Products/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
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
