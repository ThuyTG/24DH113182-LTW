using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _24DH113182_LTW.Models.ViewModel
{
    public class Cart
    {
        private List<CartItem> items = new List<CartItem>();
        public IEnumerable<CartItem> Items => items;
        // Add item to cart
        public void AddItem(int productID, string productImage, string productName, int quantity, decimal unitPrice, string category)
        {
            var existingItem = items.FirstOrDefault(item => item.ProductID == productID);
            if(existingItem != null)
            {
                items.Add(new CartItem { 
                    ProductID = productID,
                    ProductName = productName,
                    ProductImage = productImage,
                    Quantity = quantity,
                    UnitPrice = unitPrice,
                });
            }
            else
            {
                existingItem.Quantity = quantity;
            }
        }
        public void RemoveItem(int productID)
        {
            items.RemoveAll(item => item.ProductID == productID);
        }
        public decimal TotalValue()
        {
            return items.Sum(item => item.TotalPrice);
        }
        public void Clear()
        {
            items.Clear();
        }
        public void UpdateQuantity(int quantity, int productID)
        {
            var item = items.FirstOrDefault(i => i.ProductID == productID);
            if(item != null)
            {
                item.Quantity = quantity;
            }
        }
    }
}