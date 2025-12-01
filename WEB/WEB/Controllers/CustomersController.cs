using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Web;
using WEB;

namespace Web.Controllers
{
    public class CustomersController : Controller
    {
        private DBADIDASEntities8 db = new DBADIDASEntities8();

        // LOGIN
        public ActionResult Edit()
        {
            return View();
        }

        // DELETE - FIXED: Changed Product to Customer
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            // FIX: Changed from Product to Customer and using correct DB set
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // DELETE CONFIRMATION (POST)
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Customer customer = db.Customers.Find(id);
                if (customer == null)
                {
                    return HttpNotFound();
                }

                db.Customers.Remove(customer);
                db.SaveChanges();

                TempData["SuccessMessage"] = "Customer deleted successfully.";
                return RedirectToAction("Index", "Admin");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while deleting the customer.";
                return RedirectToAction("Delete", new { id = id });
            }
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

                    // If cart exists, go to payment; otherwise, go to home
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

        // LOGOUT
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        // REGISTER
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

        public ActionResult Users()
        {
            return View(db.Customers.ToList());
        }

        // EDIT CUSTOMER - ADDED MISSING METHOD
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IDCus,NameCus,PhoneCus,EmailCus,UserName,Password")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customer).State = EntityState.Modified;
                db.SaveChanges();
                TempData["SuccessMessage"] = "Customer updated successfully.";
                return RedirectToAction("Index");
            }
            return View(customer);
        }

        // DETAILS - ADDED MISSING METHOD
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}