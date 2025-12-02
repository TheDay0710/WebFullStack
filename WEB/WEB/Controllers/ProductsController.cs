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
    public class ProductsController : Controller
    {

        private DBADIDASEntities8 db = new DBADIDASEntities8();

        public ActionResult SP()
        {
            var products = db.Products.Include(p => p.Category);
            return View(products.ToList());
        }
        // Thêm action ViewAll với parameter category
        public PartialViewResult ViewAll(string category = "")
        {
            var products = db.Products.Include(p => p.Category);

            if (!string.IsNullOrEmpty(category))
            {
                switch (category.ToLower())
                {
                    case "Giày Nam":
                        products = products.Where(p => p.Category.NameCate.Contains("Giày Nam") || p.Category.NameCate.Contains("Giày Nam"));
                        break;
                    case "Giày Nữ":
                        products = products.Where(p => p.Category.NameCate.Contains("Giày Nữ") || p.Category.NameCate.Contains("Giày Nữ"));
                        break;
                    case "Quần Áo":
                        products = products.Where(p => p.Category.NameCate.Contains("Quần Áo") || p.Category.NameCate.Contains("Quần Áo"));
                        break;
                    case "Sale":
                        products = products.Where(p => p.Category.NameCate.Contains("Sale") || p.Category.NameCate.Contains("Sale"));
                        break;
                }
            }

            return PartialView(products.ToList());
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

            Product product = db.Products
                .Include(p => p.ProductSizes)
                .FirstOrDefault(p => p.ProductID == id);

            if (product == null)
            {
                return HttpNotFound();
            }

            product.ViewCount = (product.ViewCount ?? 0) + 1;
            db.SaveChanges();

            return View(product);
        }

        public ActionResult Create()
        {
            ViewBag.CateID = new SelectList(db.Categories, "CateID", "NameCate");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductID,Type,NamePro,DecriptionPro,CateID,Price,ImagePro,ViewCount,NumOfReview")] Product product)
        {
            if (ModelState.IsValid)
            {
               
                string productNameTrimmed = (product.NamePro ?? string.Empty).Trim().ToLower();

               
                bool isDuplicate = db.Products.Any(p => p.NamePro.Trim().ToLower() == productNameTrimmed);

                if (isDuplicate)
                {
                    ModelState.AddModelError("NamePro", "Tên sản phẩm đã tồn tại. Vui lòng nhập tên khác.");
                }
                else
                {
                    product.ViewCount = 0;
                    product.NumOfReview = 0;
                    db.Products.Add(product);
                    db.SaveChanges();
                    return RedirectToAction("SP");
                }
            }

           
            ViewBag.CateID = new SelectList(db.Categories, "CateID", "NameCate", product.CateID);
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
            ViewBag.CateID = new SelectList(db.Categories, "CateID", "NameCate", product.CateID);
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductID,NamePro,DecriptionPro,Type,ViewCount,NumOfReview,CateID,Price,ImagePro")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("SP");
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

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("SP");
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