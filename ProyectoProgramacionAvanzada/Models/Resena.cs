using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoProgramacionAvanzada.Models
{
  
    public class Resena
    {
        public int Id { get; set; }

    
        public int ProductoId { get; set; }

        [ForeignKey("ProductoId")]
        public virtual Producto Producto { get; set; }

        
        [Required]
        public string UsuarioId { get; set; }

        [ForeignKey("UsuarioId")]
        public virtual ApplicationUser Usuario { get; set; }

        [Required]
        [Range(1, 5)]
        public int Calificacion { get; set; }  // 1-5 estrellas

        [Required]
        [StringLength(500)]
        public string Comentario { get; set; }

        public DateTime FechaCreacion { get; set; }

        // Estado de moderación: Pendiente, Aprobada, Rechazada
        [Required]
        [StringLength(20)]
        public string Estado { get; set; }
    }
}
