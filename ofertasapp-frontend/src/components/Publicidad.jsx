import React from "react";
import "./Publicidad.css";

const Publicidad = ({ tipo = "banner" }) => {
  // Simulamos diferentes tipos de anuncios para maximizar ganancias
  const isBanner = tipo === "banner";

  return (
    <div className={`publicidad-container ${tipo} fade-in`}>
      <div className="ad-badge">Anuncio Premium</div>
      <div className="ad-content">
        {isBanner ? (
          <div className="banner-ad">
            <h3>¡Ofertas Exclusivas en Tecnología!</h3>
            <p>Ahorra hasta un 40% en productos seleccionados.</p>
            <button className="btn-ad">Ver Más</button>
          </div>
        ) : (
          <div className="sidebar-ad">
            <div className="ad-image-placeholder">
              <span>🚀 AD</span>
            </div>
            <h4>Impulsa tu Negocio</h4>
            <p>Publicita aquí y llega a miles de usuarios.</p>
            <button className="btn-ad-sm">Info</button>
          </div>
        )}
      </div>
      <div className="ad-footer">Espacio Publicitario</div>
    </div>
  );
};

export default Publicidad;
