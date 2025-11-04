using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _24DH113182_LTW.Models.ViewModel
{
    public class ProductDetailVM
    {
        public Product product { get; set; }
        public int quantity { get; set; }

        // Giá trị tạm thời
        public decimal estimatedValue { get; set; }

        // Thuộc tính hỗ trợ phân trang
        public int PageNumber { get; set; }
        public int PageSize { get; set; } = 3;

        // Danh sách 8 sản phẩm top deal
        public PagedList.IPagedList<Product> TopProducts { get; set; }
        // Danh sách 8 sản phẩm cùng loại
        public List<Product> RelatedProducts { get; set; }
    }
}