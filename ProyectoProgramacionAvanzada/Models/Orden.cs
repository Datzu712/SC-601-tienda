using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoProgramacionAvanzada.Models
{
    public class Orden
    {
        public int Id { get; set; }

        // Usuario que realizó la compra
        [Required]
        public string UsuarioId { get; set; }

        [ForeignKey("UsuarioId")]
        public virtual ApplicationUser Usuario { get; set; }

        public DateTime FechaOrden { get; set; }

        // Total de la orden (se calcula al momento de confirmar la compra)
        public decimal Total { get; set; }

        // Estado de la orden (Ej: "Procesada", "Cancelada")
        [StringLength(20)]
        public string Estado { get; set; }

        public virtual ICollection<DetalleOrden> Detalles { get; set; }
    }
}
