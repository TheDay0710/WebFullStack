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
    public class ProductsController : Controller
    {
        private DBADIDASEntities1 db = new DBADIDASEntities1();

        public ActionResult SP()
        {
            var products = db.Products.Include(p => p.Category);
            return View(products.ToList());
        }

        public ActionResult Index(string searchString)
        {
            var products = db.Products.Include(p => p.Category);

            if (!String.IsNullOrEmpty(searchString))
            {
                products = products.Where(s => s.NamePro.Contains(searchString));
            }

            return View(products.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // SỬA: Include ProductSizes để lấy thông tin size
            Product product = db.Products
                .Include(p => p.ProductSizes) // THÊM DÒNG NÀY
                .FirstOrDefault(p => p.ProductID == id);

            if (product == null)
            {
                return HttpNotFound();
            }

            // Tăng view count (tuỳ chọn)
            product.ViewCount++;
            db.SaveChanges();

            return View(product);
        }

        public ActionResult Create()
        {
            ViewBag.CateID = new SelectList(db.Categories, "IDCate", "NameCate");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductID,NamePro,Type,CateID,Price,ImagePro,ViewCount,NumOfReview")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("SP"); // SỬA TỪ "Index" THÀNH "SP"
            }

            ViewBag.CateID = new SelectList(db.Categories, "IDCate", "NameCate", product.CateID);
            return View(product);
        }

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
            ViewBag.CateID = new SelectList(db.Categories, "IDCate", "NameCate", product.CateID);
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductID,NamePro,Type,CateID,Price,ImagePro")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CateID = new SelectList(db.Categories, "IDCate", "NameCate", product.CateID);
            return View(product);
        }

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

        // SỬA PHƯƠNG THỨC NÀY: Đổi RedirectToAction("Index") thành RedirectToAction("SP")
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("SP"); // SỬA TỪ "Index" THÀNH "SP"
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