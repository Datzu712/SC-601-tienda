using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoProgramacionAvanzada.Models
{
    [Table("product")]
    public class Product
    {
        public Product()
        {
            product_images = new HashSet<Product_Images>();
            product_reviews = new HashSet<Product_Reviews>();
        }

        public int id { get; set; }

        [Required, StringLength(100)]
        public string name { get; set; }

        public decimal price { get; set; }

        public int stock { get; set; }

        public bool status { get; set; }

        public DateTime created_at { get; set; }

        public virtual ICollection<Product_Images> product_images { get; set; }

        public virtual ICollection<Product_Reviews> product_reviews { get; set; }
    }
}
