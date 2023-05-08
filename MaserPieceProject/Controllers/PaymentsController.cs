using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using MaserPieceProject.Models;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.BuilderProperties;

namespace MaserPieceProject.Controllers
{
    public class PaymentsController : Controller
    {
        private masterpes3Entities1 db = new masterpes3Entities1();

        // GET: Payments
        public ActionResult Index()
        {   
            var payments = db.Payments.Include(p => p.Order).Include(p => p.AspNetUser);
            var user = User.Identity.GetUserId();
            var carts = db.Carts.Where(x => x.UserId == user).ToList();
            double amount = 0;
            foreach (var item in carts)
            {
                var ProPrice = item.Product.price;
                var quantity = item.quantity;
                amount += Convert.ToDouble(ProPrice*quantity);
            }
            ViewBag.amount = amount;
            return View(payments.ToList());
        }

        [HttpPost]
        public async Task<ActionResult> CheckOut( string Email, string Address, int Phone, string FirstName, string LastName, string City, string PaymentMethod, int? Quantity, int? OrderId, int? ProductId)
        {
            var user = User.Identity.GetUserId();
            TempData["id"] = User;
            ViewBag.Message = "Your application description page.";
            var userCart = db.Carts.Where(x => x.UserId == user).ToList();
            var orderItem1 = db.Carts.Where(x => x.UserId == user).ToList();

            int totalAmount = 0;
            Order order = new Order();
            orderItem orderItem = new orderItem();

                order.isCheckout = true;
                order.Id = user;
                order.Adress = Address;
                order.Email= Email;
                order.City = City;
                order.FirstName = FirstName;
                order.LastName = LastName;
                order.Phone = Convert.ToInt32(Phone);
                order.OrderDate = DateTime.Now;
                order.PaymentMethod = PaymentMethod;
                db.Orders.Add(order);
                db.SaveChanges();


               double amount = 0;
               foreach (var item in userCart)

               {
                var ProPrice = item.Product.price;
                var quantity = item.quantity;
                amount += Convert.ToDouble(ProPrice * quantity);
                //amount +=Convert.ToDouble( item.amount);
                orderItem.Quantity = item.quantity;
                orderItem.OrderId = order.OrderId;
                orderItem.ProductId = item.ProductId;
                orderItem.color = item.SelectedColor;
                orderItem.size = item.SelectedSize;
                db.orderItems.Add(orderItem);
                db.SaveChanges();

                }
        
            order.OrderPrice = amount;
               db.SaveChanges();

            ViewBag.totalAmount = totalAmount;
            Session["totalAmount"] = totalAmount;

            await db.SaveChangesAsync();
           

            var cart = db.Carts.Where(x => x.UserId == user).ToList();
            foreach (var item in cart)
            {
                db.Carts.Remove(item);
            }
            db.SaveChanges();

            try
            {
                //var email = db.AspNetUsers.Where(p => p.Id == userId).FirstOrDefault();
                //var e = email.Email.ToString();
                MailMessage mail = new MailMessage();
                mail.To.Add(Email);
                mail.From = new MailAddress("wanmarwan00100@gmail.com");
                mail.Subject = "Printfy support";
                //mail.Body = msg;
                mail.Body = $"<p>Dear {Email}<br/>We wanted to let you know that we have received your payment for your recent order. Thank you for your purchase!<br/>We appreciate your business and thank you for choosing to shop with us. If you have any questions or concerns, please don't hesitate to contact us.<br/><br/>Best regards<br>Printfiy</p>";
                mail.IsBodyHtml = true;

                SmtpClient smtp = new SmtpClient();
                smtp.Port = 587; // 25 465
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Host = "smtp.gmail.com";
                smtp.Credentials = new System.Net.NetworkCredential("wanmarwan00100@gmail.com", "kflannqejngrtzes");
                smtp.Send(mail);
            }

            catch
            {
                TempData["A"] = "invalid Email";
            }

           


            return RedirectToAction("Index","Home");

        }
        
        public PartialViewResult info()
        {  
            var userid = User.Identity.GetUserId();
            var user = db.AspNetUsers.Where(c => c.Id == userid).FirstOrDefault();
            return PartialView("info",user);
        }

      
     

        // GET: Payments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Payment payment = db.Payments.Find(id);
            if (payment == null)
            {
                return HttpNotFound();
            }
            return View(payment);
        }

        // GET: Payments/Create
        public ActionResult Create()
        {
            ViewBag.Order_Id = new SelectList(db.Orders, "OrderId", "Id");
            ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email");
            return View();
        }

        // POST: Payments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Payemnt_Id,Order_Id,Payemnt_Date,Amount,Phone,Address,UserId")] Payment payment)
        {
            if (ModelState.IsValid)
            {
                db.Payments.Add(payment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Order_Id = new SelectList(db.Orders, "OrderId", "Id", payment.Order_Id);
            ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email", payment.UserId);
            return View(payment);
        }

        // GET: Payments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Payment payment = db.Payments.Find(id);
            if (payment == null)
            {
                return HttpNotFound();
            }
            ViewBag.Order_Id = new SelectList(db.Orders, "OrderId", "Id", payment.Order_Id);
            ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email", payment.UserId);
            return View(payment);
        }

        // POST: Payments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Payemnt_Id,Order_Id,Payemnt_Date,Amount,Phone,Address,UserId")] Payment payment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(payment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Order_Id = new SelectList(db.Orders, "OrderId", "Id", payment.Order_Id);
            ViewBag.UserId = new SelectList(db.AspNetUsers, "Id", "Email", payment.UserId);
            return View(payment);
        }

        // GET: Payments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Payment payment = db.Payments.Find(id);
            if (payment == null)
            {
                return HttpNotFound();
            }
            return View(payment);
        }

        // POST: Payments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Payment payment = db.Payments.Find(id);
            db.Payments.Remove(payment);
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
