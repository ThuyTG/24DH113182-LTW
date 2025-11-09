using System.Collections.Generic;
using _24DH113182_LTW.Models;

namespace _24DH113182_LTW.Models.ViewModel
{
    public class MyOrderVM
    {
        public List<Order> Orders { get; set; }
        public string CurrentTab { get; set; }
        public string SearchTerm { get; set; }
    }
}