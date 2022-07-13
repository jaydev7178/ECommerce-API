using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ecommerce.Models
{
    public class Bean
    {
    }
    public class login
    {
        public string userName { get; set; }
        public string password { get; set; }
    }

    public class Admin
    {
        public int? id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public bool? status { get; set; }
    }
    public class Customer 
    { 
        public int? id { get; set; }
        public string  name { get; set; }
        public string email { get; set; }
        public string password  { get; set; }
        public string gender  { get; set; }
        public string mobileNo  { get; set; }
        public string address { get; set; }
        public bool? status { get; set; }
    }
    public class Cart
    {
        public int? id { get; set; }
        public int? productId  { get; set; }
        public int? customerId { get; set; }
        public int? quantity { get; set; }
        public bool? status { get; set; }
    }
    public class Product
    {
        public int? id { get; set; }
        public string name { get; set; }
        public int? quantity { get; set; }
        public int? price { get; set; }
        public string description { get; set; }
        public int? categoryId { get; set; }
        public bool? status { get; set; }
    }
    public class Category
    {
        public int? id { get; set; }
        public string  name  { get; set; }

        public bool?  status { get; set; }
    }
    public class Delivery
    {
        public int? Id { get; set; }
        public int? ProductId { get; set; }
        public bool? Delivered { get; set; }
        public bool? status { get; set; }

        public int? CustomerId { get; set; }
    }
}
