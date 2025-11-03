using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace _24DH113182_LTW.Models
{
    [MetadataType(typeof(UserMetadata))]
    public partial class User
    {
        [NotMapped]
        [Compare("Password")]
        public string ConfirmedPassword { get; set; }
    }
    [MetadataType(typeof(ProductMetadata))]
    public partial class Product
    {
        [RegularExpression(@"[a-zA-Z0-9\s_\\.\-:]+(.png|.jpg|.gif|.PNG|.JPG|.GIF)$",
ErrorMessage = "[translate:Chi chap nhan: PNG, JPG va GIF]")]
        [NotMapped]
        public HttpPostedFile UploadImg { get; set; }
    }
}