using MaserPieceProject.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Configuration;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Xml.Linq;

namespace MaserPieceProject.Controllers
{
    public class HomeController : Controller
    {
       masterpes3Entities1 db=new masterpes3Entities1();
        public ActionResult Index()
        {
            var ash = db.Categories.ToList();
            return View(ash);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        [HttpPost]
        public ActionResult Email(string email ,string msg)
            
        {
            try
            {
                //var email = db.AspNetUsers.Where(p => p.Id == userId).FirstOrDefault();
                //var e = email.Email.ToString();
                MailMessage mail = new MailMessage();
                mail.To.Add(email);
                mail.From = new MailAddress("wanmarwan00100@gmail.com");
                mail.Subject = "Printfy support";
                mail.Body = msg;
                //mail.Body = $"<p>Dear {email.Full_Name}<br/>We wanted to let you know that we have received your payment for your recent order. Thank you for your purchase!<br/>We appreciate your business and thank you for choosing to shop with us. If you have any questions or concerns, please don't hesitate to contact us.<br/><br/>Best regards<br>Plant Paradise</p>";
                mail.IsBodyHtml = true;

                SmtpClient smtp = new SmtpClient();
                smtp.Port = 587; // 25 465
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Host = "smtp.gmail.com";
                smtp.Credentials = new System.Net.NetworkCredential("wanmarwan00100@gmail.com", "kflannqejngrtzes");
                smtp.Send(mail);
            }

            catch {
                TempData["A"] = "invalid Email";
            }

                return View("Contact");
            
            
        }

        public ActionResult blog()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Product()
        {
            var ash = db.Products.ToList(); 
            //if (Request.QueryString["Categore"] == null) {
                //ash = db.Products.ToList();
            //}
            //else
            //{
            //    string Categore = Request.QueryString["Categore"];
            //     ash = db.Products.Where(c => c.Category.CategoryName == Categore).ToList();
            //}
              return View(ash);
        }
        //public ActionResult search(string Product)
        //{
        //    var Products = db.Products.Where(p => p.ProductName.StartsWith(Product));
        //    return View("Product",Products.Last());

        //}
        [HttpPost]
        public ActionResult SearchPro(string search)
        {
            //var x1 = db.p.Where(x => ((x.Contains(search) || x.OrderDate.ToString().Contains(search)) && x.Isaccepted == null)).ToList();

            var sh = db.Products.Include(v=>v.Category).Where(c=>c.ProductName.Contains(search)).ToList();
            return View("Product", sh);
        }

        public ActionResult productDetail(int id)
        {
            Session["welcome"] = "welcome";
            Session["id"] = id;
            ViewBag.Message = "Your contact page.";
            var products = db.Products.Find(id);
            return View(products);
        
           
        }
        [HttpPost]
        [Authorize]
        public ActionResult Addto_cart(int productId,int size,int color,int Quantity)
        {
            Session["welcome"] = null;
            var product = db.Products.Find(productId);
            Cart cart= new Cart();
            double price =Convert.ToDouble(product.price);
            cart.ProductId = productId;
            cart.SelectedSize = size;
            cart.SelectedColor= color;
            cart.quantity = Quantity;
            cart.amount = price * Quantity;
            
            cart.UserId = User.Identity.GetUserId();
            db.Carts.Add(cart);

            db.SaveChanges();
            return RedirectToAction("productDetail", "Home", new { id = productId });
        }

        public ActionResult shopingCart()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public static Dictionary<string, int> UniqueVisitors = new Dictionary<string, int>();
        public static int TotalVisitors = 0;
        public static int BounceVisitors = 0;

        [Authorize(Roles = "Admin")]
        public ActionResult Dashboard()
        {

            var orderCount = db.Orders.Count();
            var orderd = db.orderItems.ToList();
            var user = db.AspNetUsers.Count();
            
            

            TotalVisitors++;

            var visitorId = "";
            if (Request.Cookies["VisitorId"] == null || Request.Cookies["VisitorDate"] == null)
            {
                visitorId = Guid.NewGuid().ToString();
                Response.Cookies.Add(new HttpCookie("VisitorId", visitorId));
                Response.Cookies.Add(new HttpCookie("VisitorDate", DateTime.Now.ToShortDateString()));

                if (UniqueVisitors.ContainsKey(DateTime.Now.ToShortDateString()))
                {
                    UniqueVisitors[DateTime.Now.ToShortDateString()]++;
                }
                else
                {
                    UniqueVisitors.Add(DateTime.Now.ToShortDateString(), 1);
                }
            }
            else if (Request.Cookies["VisitorDate"].Value != DateTime.Now.ToShortDateString())
            {
                visitorId = Guid.NewGuid().ToString();
                Response.Cookies.Set(new HttpCookie("VisitorId", visitorId));
                Response.Cookies.Set(new HttpCookie("VisitorDate", DateTime.Now.ToShortDateString()));

                if (UniqueVisitors.ContainsKey(DateTime.Now.ToShortDateString()))
                {
                    UniqueVisitors[DateTime.Now.ToShortDateString()]++;
                }
                else
                {
                    UniqueVisitors.Add(DateTime.Now.ToShortDateString(), 1);
                }

                BounceVisitors++;
            }
            else
            {
                visitorId = Request.Cookies["VisitorId"].Value;

                if (!UniqueVisitors.ContainsKey(DateTime.Now.ToShortDateString()))
                {
                    UniqueVisitors.Add(DateTime.Now.ToShortDateString(), 1);
                }
                else if (!Request.Cookies.AllKeys.Contains("VisitorId"))
                {
                    UniqueVisitors[DateTime.Now.ToShortDateString()]++;
                }
            }

            ViewBag.Counter = TotalVisitors;
            ViewBag.UniqueVisitors = UniqueVisitors[DateTime.Now.ToShortDateString()];
            ViewBag.BounceRate = (float)BounceVisitors / TotalVisitors * 100;

            return View(Tuple.Create(orderCount, orderd, user));
        }







        [Authorize]
        public ActionResult review(int id, [Bind(Include = "reviewId,aspId,ProductId,comment,rating,Email,Name")] Review review, string comment, string rating,string Email,string Name)
        {
            var mainId = User.Identity.GetUserId();
            review.comment = comment;
            //review.rating = Convert.ToInt32(rating);
            review.ProductId = id;
            review.aspId= mainId;
            //review.Email = Email; 
            //review.Name= Name;
            review.rating =Convert.ToInt32(rating);
       

            //cart.User_Id = mainId;
            //cart.Qty = quant;
            //cart.Product_Id = id;
            //cart.Total_Price = totalPrice * quant;


                db.Reviews.Add(review);
                db.SaveChanges();
            


            return RedirectToAction("productDetail", new { id = id });
        }

        public ActionResult Erorr()
        {
           

            return View();
        }

    }
}