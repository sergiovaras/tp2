import React from "react";

function ProductCard({ item }) {
  const formatPrice = (price) => {
    return new Intl.NumberFormat('es-AR', {
      style: 'currency',
      currency: 'ARS',
      maximumFractionDigits: 0
    }).format(price);
  };

  return (
    <div className="col-lg-4 col-md-6 mb-4 fade-in">
      <div className="glass-card h-100 d-flex flex-column overflow-hidden">
        <div className="position-relative">
          <img 
            src={item.imagenUrl || "https://via.placeholder.com/400x300"} 
            className="w-100" 
            alt={item.nombre}
            style={{ height: "240px", objectFit: "contain", padding: "20px", background: "#fff" }}
          />
          {item.descuento && (
            <div className="position-absolute top-0 end-0 m-3">
              <span className="badge-discount">{item.descuento}</span>
            </div>
          )}
        </div>
        
        <div className="p-4 d-flex flex-column flex-grow-1">
          <div className="mb-2">
            <span className="text-secondary small">{item.vendedor}</span>
          </div>
          <h5 className="mb-3 text-truncate-2" style={{ 
            fontSize: "1.1rem", 
            display: "-webkit-box", 
            WebkitLineClamp: "2", 
            WebkitBoxOrient: "vertical", 
            overflow: "hidden",
            height: "2.8rem"
          }}>
            {item.nombre}
          </h5>
          
          <div className="mt-auto">
            <div className="d-flex align-items-baseline gap-2 mb-2">
              <span className="h4 mb-0" style={{ color: "var(--accent)", fontWeight: "800" }}>
                {formatPrice(item.precioConDescuento)}
              </span>
              {item.precioOriginal > item.precioConDescuento && (
                <span className="text-secondary text-decoration-line-through small">
                  {formatPrice(item.precioOriginal)}
                </span>
              )}
            </div>
            
            {item.envioGratis && (
              <div className="mb-3">
                <span className="badge-free-shipping">ENVÍO GRATIS</span>
              </div>
            )}

            <a 
              href={item.permalink} 
              target="_blank" 
              rel="noopener noreferrer" 
              className="premium-btn w-100 justify-content-center"
            >
              Ver Oferta
              <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" viewBox="0 0 16 16">
                <path fillRule="evenodd" d="M8.636 3.5a.5.5 0 0 0-.5-.5H1.5A1.5 1.5 0 0 0 0 4.5v10A1.5 1.5 0 0 0 1.5 16h10a1.5 1.5 0 0 0 1.5-1.5V7.864a.5.5 0 0 0-1 0V14.5a.5.5 0 0 1-.5.5h-10a.5.5 0 0 1-.5-.5v-10a.5.5 0 0 1 .5-.5h6.636a.5.5 0 0 0 .5-.5z"/>
                <path fillRule="evenodd" d="M16 .5a.5.5 0 0 0-.5-.5h-5a.5.5 0 0 0 0 1h3.793L6.146 9.146a.5.5 0 1 0 .708.708L15 1.707V5.5a.5.5 0 0 0 1 0v-5z"/>
              </svg>
            </a>
          </div>
        </div>
      </div>
    </div>
  );
}

export default ProductCard;
