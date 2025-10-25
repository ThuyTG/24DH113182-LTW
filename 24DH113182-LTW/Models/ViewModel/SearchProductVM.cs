using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList.Mvc;
namespace _24DH113182_LTW.Models.ViewModel
{
    public class SearchProductVM
    {
        public string SearchTerm { get; set; }

        // Search theo giá
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }

        // Thứ tự sắp xếp
        public string SortOrder { get; set; }

        // Phân trang
            // Trang hiện tại
        public int? PageNumber { get; set; }
            // Số sản phẩm trong 1 trang
        public int? PageSize { get; set; }
        
        public PagedList.IPagedList<Product> products { get; set; }
        //public List<Product> products { get; set; }
    }
}