namespace ProyectoProgramacionAvanzada.Models
{
    public class CarritoItemViewModel
    {
        public int ProductoId { get; set; }
        public string NombreProducto { get; set; }
        public decimal Precio { get; set; }
        public int Cantidad { get; set; }

        public decimal SubTotal { get; set; }  
    }
}
