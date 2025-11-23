using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WEB
{
    public class CartItem
    {
        public Product _shopping_product { get; set; }
        public int _shopping_quantity { get; set; }
        public string _shopping_size { get; set; } // THÊM DÒNG NÀY

        // Thêm property để tính tổng tiền (tuỳ chọn)
        public decimal? TotalPrice
        {
            get { return _shopping_product?.Price * _shopping_quantity; }
        }
    }
}