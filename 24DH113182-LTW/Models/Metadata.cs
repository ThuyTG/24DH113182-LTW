using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _24DH113182_LTW.Models
{
    //public class UserMetadata
    //{
    //    [Required(ErrorMessage = "Username not null")]
    //    [StringLength(30, MinimumLength = 5)]
    //    [RegularExpression("\r\n ^[a-zA-Z0-9](?!.*[._]{2})[a-zA-Z0-9._]{6,20}[a-zA-Z0-9]$ ")]
    //    public string Username { get; set; }
    //    [Required]
    //    [DataType(DataType.Password)]
    //    public string Password { get; set; }
    //    [Required]
    //    public string UserRole { get; set; }
    //}
    //public class CategoryMetadata
    //{
    //    [HiddenInput]
    //    public int CategoryID { get; set; }
    //    [Required]
    //    [StringLength(50, MinimumLength = 5)]
    //    public string CategoryName { get; set; }
    //}
    //public class CustomerMetadata { }
    //public class MyStoreModelMetadata { }
    //public class OrderMetadata { }
    //public class OrderDetailsMetadata { }


    [MetadataType(typeof(ProductMetadata))]
    public class ProductMetadata
    {
        [Required]
        public int ProductID { get; set; }

        [Required]
        public int CategoryID { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên sản phẩm")]
        [StringLength(100)]
        public string ProductName { get; set; }  

        [StringLength(500)]
        public string ProductDescription { get; set; }  
        [Required]
        public decimal ProductPrice { get; set; }

        [StringLength(200)]
        public string ProductImage { get; set; }
    }
}