namespace ProyectoProgramacionAvanzada.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("user")]
    public partial class User
 
        {
            public User()
            {
                product_reviews = new HashSet<Product_Reviews>();
                role = new HashSet<Role>();
            }

            public int id { get; set; }

            [Required, StringLength(100)]
            public string username { get; set; }

            [Required, StringLength(500)]
            public string hashed_password { get; set; }

            public DateTime? last_connection { get; set; }

            public bool enabled { get; set; }

            public virtual ICollection<Product_Reviews> product_reviews { get; set; }
            public virtual ICollection<Role> role { get; set; }
        }
    }
