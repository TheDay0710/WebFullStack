using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WEB;

namespace WEB.Controllers
{
    public class CustomersController : Controller
    {
        private DBADIDASEntities db = new DBADIDASEntities();
        public ActionResult Users()
        {
            return View(db.Customers.ToList());
        }
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string UserName, string Password)
        {
            if (ModelState.IsValid)
            {
                var user = db.Customers.FirstOrDefault(u => u.UserName.Equals(UserName) && u.Password.Equals(Password));

                if (user != null)
                {

                    Session["IDCus"] = user.IDCus;
                    Session["NameCus"] = user.NameCus;
                    Session["UserName"] = user.UserName;


                    if (Session["Cart"] != null)
                    {
                        return RedirectToAction("Index", "Payment");
                    }


                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.Error = "Tên đăng nhập hoặc mật khẩu không đúng!";
                }
            }
            return View();
        }


        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }


        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IDCus,NameCus,PhoneCus,EmailCus,UserName,Password")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                var checkUser = db.Customers.FirstOrDefault(s => s.UserName == customer.UserName);

                if (checkUser == null)
                {

                    db.Customers.Add(customer);
                    db.SaveChanges();


                    Session["IDCus"] = customer.IDCus;
                    Session["NameCus"] = customer.NameCus;
                    Session["UserName"] = customer.UserName;


                    if (Session["Cart"] != null)
                    {
                        return RedirectToAction("Index", "Payment");
                    }

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.Error = "Tên đăng nhập này đã tồn tại!";
                    return View(customer);
                }
            }
            return View(customer);
        }


        public ActionResult Index()
        {
            return View(db.Customers.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Customer customer = db.Customers.Find(id);
            if (customer == null) return HttpNotFound();
            return View(customer);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Customer customer = db.Customers.Find(id);
            if (customer == null) return HttpNotFound();
            return View(customer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IDCus,NameCus,PhoneCus,EmailCus,UserName,Password")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(customer);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Customer customer = db.Customers.Find(id);
            if (customer == null) return HttpNotFound();
            return View(customer);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Customer customer = db.Customers.Find(id);
            db.Customers.Remove(customer);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}
