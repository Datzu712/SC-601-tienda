using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProyectoProgramacionAvanzada.Models
{
    public class Producto
    {
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        public string CodigoProducto { get; set; }

        [Required]
        public string Nombre { get; set; }

        [Required]
        public decimal Precio { get; set; }

        public int Inventario { get; set; }

        public bool Activo { get; set; }

        public virtual ICollection<ImagenProducto> Imagenes { get; set; }
        public virtual ICollection<Resena> Resenas { get; set; }
    }
}
