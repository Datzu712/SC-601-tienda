namespace ProyectoProgramacionAvanzada.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Product_Reviews
        {
            public int id { get; set; }

            [Required]
            public string content { get; set; }

            public bool approved { get; set; }

            public int product_id { get; set; }
            public int author_id { get; set; }

            public virtual Product product { get; set; }
            public virtual User user { get; set; }
        }
    }
