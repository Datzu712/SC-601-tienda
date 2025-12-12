using System;

namespace ProyectoProgramacionAvanzada.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public decimal Total { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
