using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WEB.Controllers
{
    public class CartController : Controller
    {
        DBADIDASEntities8 db = new DBADIDASEntities8();

        public List<CartItem> GetCart()
        {
            List<CartItem> myCart = Session["Cart"] as List<CartItem>;
            if (myCart == null)
            {
                myCart = new List<CartItem>();
                Session["Cart"] = myCart;
            }
            return myCart;
        }

        
        [HttpPost]
        public ActionResult AddToCart(int productId, string selectedSize)
        {
            try
            {
                if (string.IsNullOrEmpty(selectedSize))
                {
                    TempData["ErrorMessage"] = "Vui lòng chọn kích cỡ";
                    return RedirectToAction("Details", "Products", new { id = productId });
                }

                var productSize = db.ProductSizes
                    .FirstOrDefault(ps => ps.ProductID == productId && ps.SizeName == selectedSize);

                if (productSize == null)
                {
                    TempData["ErrorMessage"] = "Kích cỡ không tồn tại";
                    return RedirectToAction("Details", "Products", new { id = productId });
                }

                if (productSize.Quantity <= 0)
                {
                    TempData["ErrorMessage"] = "Kích cỡ này đã hết hàng";
                    return RedirectToAction("Details", "Products", new { id = productId });
                }

                List<CartItem> myCart = GetCart();

                
                CartItem currentItem = myCart.FirstOrDefault(p =>
                    p._shopping_product.ProductID == productId &&
                    p._shopping_size == selectedSize);

                if (currentItem == null)
                {
                    Product product = db.Products.Find(productId);
                    if (product != null)
                    {
                        myCart.Add(new CartItem
                        {
                            _shopping_product = product,
                            _shopping_quantity = 1,
                            _shopping_size = selectedSize 
                        });
                    }
                }
                else
                {
                    currentItem._shopping_quantity++;
                }

                Session["Cart"] = myCart;
                TempData["SuccessMessage"] = "Đã thêm vào giỏ hàng!";

                return RedirectToAction("Details", "Products", new { id = productId });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Có lỗi xảy ra: " + ex.Message;
                return RedirectToAction("Details", "Products", new { id = productId });
            }
        }

        public ActionResult Index()
        {
            List<CartItem> myCart = GetCart();

            if (myCart.Count > 0)
            {
                ViewBag.Total = myCart.Sum(x => x._shopping_product.Price * x._shopping_quantity);
            }
            else
            {
                ViewBag.Total = 0;
            }

            return View(myCart);
        }

        [HttpPost]
        public ActionResult UpdateCart(int id, string size, FormCollection f)
        {
            List<CartItem> myCart = GetCart();
   
            CartItem currentItem = myCart.FirstOrDefault(p =>
                p._shopping_product.ProductID == id &&
                p._shopping_size == size);

            if (currentItem != null)
            {
                currentItem._shopping_quantity = int.Parse(f["Quantity"].ToString());
            }
            return RedirectToAction("Index");
        }


        public ActionResult RemoveCart(int id, string size)
        {
            List<CartItem> myCart = GetCart();
    
            CartItem currentItem = myCart.FirstOrDefault(p =>
                p._shopping_product.ProductID == id &&
                p._shopping_size == size);

            if (currentItem != null)
            {
                myCart.Remove(currentItem);
            }
            return RedirectToAction("Index");
        }
    }
}