using System;
using System.Collection.Generic;
using System.Linq;
using System.Web;

namespace _24DH113182_LTW.Models.ViewModel
{
    public class SearchProductVM
    {
        public string SearchTerm { get; set; }
        public List<Product> Products { get; set; }
    }
}
