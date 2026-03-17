using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations.Schema;

namespace OfertasApp.Models
{
    public class Producto
    {
        public int Id { get; set; }
        public required string Nombre { get; set; }
        public required string Categoria { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public required decimal PrecioOriginal { get; set; }
        public  required string ImagenUrl { get; set; }
        
        [JsonIgnore]
        public Oferta? Oferta { get; set; }
    }
}