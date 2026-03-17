
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations.Schema;

    


namespace OfertasApp.Models
{
    public class Oferta
    {
        public int Id { get; set; } 
        public int ProductoId { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public required decimal PrecioConDescuento { get; set; } 
        public required DateTime FechaInicio { get; set; }
        public required DateTime FechaFin { get; set; } 
        [Column(TypeName = "decimal(18,2)")]
        public required decimal PrecioOriginal { get; set; }

        // Propiedad calculada (no se guarda en DB)
        public int PorcentajeDescuento
        {
            get 
            { 
                if (PrecioOriginal == 0) return 0;
                return (int)((1 - (PrecioConDescuento / PrecioOriginal)) * 100);
            }
        }

        [JsonIgnore]
        public Producto? Producto { get; set; }
    }
}
