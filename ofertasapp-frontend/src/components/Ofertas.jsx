import React, { useEffect, useState } from "react";
import axios from "axios";
import ProductCard from "./ProductCard";

function Ofertas({ onDataFetched }) {
  const [ofertas, setOfertas] = useState([]);
  const [loading, setLoading] = useState(true);
  const [searchTerm, setSearchTerm] = useState("");
  const [error, setError] = useState(null);

  const fetchOfertas = (query = "") => {
    setLoading(true);
    axios.get(`${import.meta.env.VITE_API_URL}/api/Ofertas/mercadolibre${query ? `?query=${query}` : ""}`)
      .then(res => {
        setOfertas(res.data.data);
        if (onDataFetched) onDataFetched(res.data);
        setLoading(false);
      })
      .catch(err => {
        console.error("Error al traer ofertas:", err);
        setError("No se pudieron cargar las ofertas. Asegúrate de que el backend esté corriendo.");
        setLoading(false);
      });
  };

  useEffect(() => {
    fetchOfertas();
  }, []);

  const handleSearch = (e) => {
    e.preventDefault();
    fetchOfertas(searchTerm);
  };

  return (
    <div className="fade-in">
      <div className="row mb-5 justify-content-center">
        <div className="col-md-8 text-center">
          <h2 className="mb-4 h3">Explorar Ofertas</h2>
          <form onSubmit={handleSearch} className="d-flex gap-2">
            <div className="input-group glass-card overflow-hidden" style={{ border: "none" }}>
              <span className="input-group-text bg-transparent border-0 ps-4">
                 <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="var(--text-secondary)" viewBox="0 0 16 16">
                  <path d="M11.742 10.344a6.5 6.5 0 1 0-1.397 1.398h-.001c.03.04.062.078.098.115l3.85 3.85a1 1 0 0 0 1.415-1.414l-3.85-3.85a1.007 1.007 0 0 0-.115-.1zM12 6.5a5.5 5.5 0 1 1-11 0 5.5 5.5 0 0 1 11 0z"/>
                </svg>
              </span>
              <input 
                type="text" 
                className="form-control bg-transparent border-0 py-3 text-white" 
                placeholder="Buscar en Mercado Libre (ej: tecnología, hogar...)"
                value={searchTerm}
                onChange={(e) => setSearchTerm(e.target.value)}
                style={{ boxShadow: "none" }}
              />
              <button type="submit" className="premium-btn m-1">Buscar</button>
            </div>
          </form>
        </div>
      </div>

      {loading ? (
        <div className="text-center py-5">
          <div className="spinner-border text-primary" role="status">
            <span className="visually-hidden">Loading...</span>
          </div>
          <p className="mt-3 text-secondary">Extrayendo datos de Mercado Libre...</p>
        </div>
      ) : error ? (
        <div className="alert alert-danger glass-card border-danger bg-transparent text-center py-4">
          <p className="mb-0">{error}</p>
        </div>
      ) : (
        <div className="row">
          {ofertas.length > 0 ? (
            ofertas.map((o) => (
              <ProductCard key={o.id} item={o} />
            ))
          ) : (
            <div className="col-12 text-center py-5">
              <p className="text-secondary">No se encontraron ofertas para "{searchTerm}"</p>
            </div>
          )}
        </div>
      )}
    </div>
  );
}

export default Ofertas;

