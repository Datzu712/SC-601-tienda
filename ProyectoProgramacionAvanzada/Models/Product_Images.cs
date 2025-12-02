namespace ProyectoProgramacionAvanzada.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;


    {
        [Table("product_images")]
        public class Product_Images
        {
            public int id { get; set; }
            public int product_id { get; set; }
            public string image_url { get; set; }

            public virtual Product product { get; set; }
        }
    }

