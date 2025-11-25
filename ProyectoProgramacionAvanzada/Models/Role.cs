namespace ProyectoProgramacionAvanzada.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("role")]
    public partial class Role
        {
            public Role()
            {
                user = new HashSet<User>();
            }

            public int id { get; set; }

            public string name { get; set; }

            public virtual ICollection<User> user { get; set; }
        }
    }

