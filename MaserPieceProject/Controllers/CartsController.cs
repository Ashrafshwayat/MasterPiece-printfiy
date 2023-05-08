using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MaserPieceProject.Models;
using Microsoft.AspNet.Identity;

namespace MaserPieceProject.Controllers
{
    public class CartsController : Controller
    {
        private masterpes3Entities1 db = new masterpes3Entities1();

        public ActionResult Cart()
        {



            var id = User.Identity.GetUserId();
            ViewBag.userId = id;
            var carts = db.Carts.Where(x => x.AspNetUser.Id == id).Include(c => c.Product);
            return View(carts.ToList());

        }
        [HttpPost]
        public ActionResult Updatecart(FormCollection form)
        {
            var userid = User.Identity.GetUserId();
            var Usercart = db.Carts.Where(user => user.UserId == userid);
            
            foreach (var key in form.AllKeys)
            {
                if (key.StartsWith("quantity-"))
                {
                    int cartItemId = int.Parse(key.Replace("quantity-", ""));
                    int quantity = int.Parse(form[key]);
                    var cartItem = db.Carts.Find(cartItemId);
                    if (cartItem != null)
                    {
                        cartItem.quantity = quantity;
                    }
                }
            }

            db.SaveChanges();
            return RedirectToAction("Cart");

        }

        public ActionResult Buy(int id)
        {



            if (User.Identity.GetUserId() == null)
                return RedirectToAction("Login", "Account", "");
            string email = User.Identity.GetUserName();

            AspNetUser customer = (AspNetUser)db.AspNetUsers.SingleOrDefault(c => c.Email == email);
            Product product = db.Products.Find(id);
            int? price = Convert.ToInt32(product.price);
         
            var Cart = new Cart()
            {
                UserId = customer.Id,
                ProductId = id,
                quantity = 1,
                amount = price
               
            };
            db.Carts.Add(Cart);
            db.SaveChanges();

            return RedirectToAction("Cart");
        }

        // GET: Carts
        public ActionResult Index()
        {
            var carts = db.Carts.Include(c => c.AspNetUser).Include(c => c.Product).Include(c => c.Size).Include(c => c.Color);
            return View(carts.ToList());
        }

        // GET: Carts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cart cart = db.Carts.Find(id);
            if (cart == null)
            {
                return HttpNotFound();
            }
            return View(cart);
        }

        // GET: Carts/Create
        public ActionResult Create()
        {
            ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email");
            ViewBag.ProductId = new SelectList(db.Products, "ProductId", "ProductName");
            ViewBag.SelectedSize = new SelectList(db.Sizes, "sizeid", "name");
            ViewBag.SelectedColor = new SelectList(db.Colors, "colorId", "name");
            return View();
        }

        // POST: Carts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Cart_Id,quantity,SelectedSize,SelectedColor,ProductId,UserId")] Cart cart)
        {
            if (ModelState.IsValid)
            {
                db.Carts.Add(cart);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email", cart.UserId);
            ViewBag.ProductId = new SelectList(db.Products, "ProductId", "ProductName", cart.ProductId);
            ViewBag.SelectedSize = new SelectList(db.Sizes, "sizeid", "name", cart.SelectedSize);
            ViewBag.SelectedColor = new SelectList(db.Colors, "colorId", "name", cart.SelectedColor);
            return View(cart);
        }

        // GET: Carts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cart cart = db.Carts.Find(id);
            if (cart == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email", cart.UserId);
            ViewBag.ProductId = new SelectList(db.Products, "ProductId", "ProductName", cart.ProductId);
            ViewBag.SelectedSize = new SelectList(db.Sizes, "sizeid", "name", cart.SelectedSize);
            ViewBag.SelectedColor = new SelectList(db.Colors, "colorId", "name", cart.SelectedColor);
            return View(cart);
        }

        // POST: Carts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Cart_Id,quantity,SelectedSize,SelectedColor,ProductId,UserId")] Cart cart)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cart).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email", cart.UserId);
            ViewBag.ProductId = new SelectList(db.Products, "ProductId", "ProductName", cart.ProductId);
            ViewBag.SelectedSize = new SelectList(db.Sizes, "sizeid", "name", cart.SelectedSize);
            ViewBag.SelectedColor = new SelectList(db.Colors, "colorId", "name", cart.SelectedColor);
            return View(cart);
        }

        // GET: Carts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cart cart = db.Carts.Find(id);
         
            if (cart == null)
            {
                return HttpNotFound();
            }
            db.Carts.Remove(cart);
            db.SaveChanges();
            return RedirectToAction("Cart");
        }

        // POST: Carts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Cart cart = db.Carts.Find(id);
            db.Carts.Remove(cart);
            db.SaveChanges();
            return RedirectToAction("Cart");
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
