using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList.Mvc;

namespace _24DH113182_LTW.Models.ViewModel
{
    public class HomeProductVM
    {
        // Tiêu chí để search sản phẩm
        public string SearchTerm { get; set; }

        // Thuộc tính hỗ trợ phân trang
        public int PageNumber { get; set; }
        public int PageSize { get; set; }


        // Danh sách sản phẩm nổi bật
        public List<Product> FeaturedProducts { get; set; }


        // Danh sách sản phẩm mới đã phân trang
        public PagedList.IPagedList<Product> NewProducts { get; set; }
    }
}