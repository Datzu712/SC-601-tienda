using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoProgramacionAvanzada.Models
{
    public class ImagenProducto
    {
        public int Id { get; set; }

        public int ProductoId { get; set; }

        [ForeignKey("ProductoId")]
        public virtual Producto Producto { get; set; }

        public byte[] DatosImagen { get; set; }

        public string ContentType { get; set; }
    }
}
