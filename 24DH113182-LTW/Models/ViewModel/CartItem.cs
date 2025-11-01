﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _24DH113182_LTW.Models.ViewModel
{
    public class CartItem
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string ProductImage { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        // Total Price
        public decimal TotalPrice => Quantity * UnitPrice;
    }
}