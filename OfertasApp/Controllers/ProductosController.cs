using Microsoft.AspNetCore.Mvc;
using OfertasApp.Models;
using OfertasApp.Data;
using Microsoft.EntityFrameworkCore;

namespace OfertasApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductosController(AppDbContext context)
        {
            _context = context;
        }

    // GET: api/productos
        [HttpGet]
        public IActionResult GetProductos()
        {
            var productos = _context.Productos.Include(p => p.Oferta).ToList();
            return Ok(productos);
        }

    // GET: api/productos/5
        [HttpGet("{id}")]
        public IActionResult GetProducto(int id)
        {
            var producto = _context.Productos.Include(p => p.Oferta).FirstOrDefault(p => p.Id == id);
            if (producto == null) return NotFound();
            return Ok(producto);
        }

    // POST: api/productos
       [HttpPost]
       public IActionResult CrearProducto([FromBody] Producto producto)
        {
            if(!ModelState.IsValid) return BadRequest(ModelState);

            _context.Productos.Add(producto);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetProducto), new { id = producto.Id }, producto);
        }

    // POST: api/productos/{id}/oferta
        [HttpPost("{id}/oferta")]
        public IActionResult CrearOferta(int id, [FromBody] Oferta oferta)
        {
            var producto = _context.Productos.Find(id);
            if (producto == null) return NotFound("Producto no encontrado");

            if (oferta.FechaFin <= oferta.FechaInicio) 
                return BadRequest("La fecha de fin debe ser posterior a la fecha de inicio");

            oferta.ProductoId = id;
            oferta.PrecioOriginal = producto.PrecioOriginal;
            producto.Oferta = oferta;

            _context.Ofertas.Add(oferta);
            _context.SaveChanges();

            return Ok(oferta);
        }

        // PUT: api/productos/5
        [HttpPut("{id}")]
        public IActionResult ActualizarProducto(int id, [FromBody] Producto producto)
        {
            if (id != producto.Id)
                return BadRequest("El ID del producto no coincide");

            var productoExistente = _context.Productos.Include(p => p.Oferta).FirstOrDefault(p => p.Id == id);
            if (productoExistente == null)
                return NotFound("Producto no encontrado");

            // Actualizar propiedades
            productoExistente.Nombre = producto.Nombre;
            productoExistente.Categoria = producto.Categoria;
            productoExistente.PrecioOriginal = producto.PrecioOriginal;
            productoExistente.ImagenUrl = producto.ImagenUrl;

            _context.SaveChanges();
            return NoContent();
        }

        // DELETE: api/productos/5
        [HttpDelete("{id}")]
        public IActionResult EliminarProducto(int id)
        {
            var producto = _context.Productos.Find(id);
            if (producto == null)
                 return NotFound("Producto no encontrado");

            _context.Productos.Remove(producto);
            _context.SaveChanges();

            return NoContent();
        }
    }    
}
