import React, { useState } from "react";
import Ofertas from "./components/Ofertas";
import Publicidad from "./components/Publicidad";
import "./components/Publicidad.css";

function App() {
  const [apiResponse, setApiResponse] = useState(null);

  const handleDataFetched = (data) => {
    setApiResponse(data);
  };

  return (
    <div className="container-fluid p-0">
      {/* Premium Hero Header */}
      <header className="py-5 mb-5 text-center fade-in" style={{ 
        background: "linear-gradient(rgba(0,0,0,0.4), rgba(0,0,0,0.4)), url('https://images.unsplash.com/photo-1607082348824-0a96f2a4b9da?auto=format&fit=crop&w=1920&q=80')",
        backgroundSize: "cover",
        backgroundPosition: "center",
        minHeight: "40vh",
        display: "flex",
        flexDirection: "column",
        justifyContent: "center",
        borderBottom: "1px solid rgba(255,255,255,0.1)"
      }}>
        <div className="container">
          <h1 className="display-2 mb-2" style={{ textShadow: "0 4px 10px rgba(0,0,0,0.5)" }}>
            Mercado <span style={{ color: "var(--secondary)" }}>Libre</span> Deals
          </h1>
          <p className="lead text-secondary-emphasis" style={{ fontSize: "1.4rem", fontWeight: "300" }}>
            Las mejores ofertas del día, directo de la API a tu pantalla.
          </p>
        </div>
      </header>

      <main className="container pb-5">
        {/* Sección de ofertas */}
        <Ofertas onDataFetched={handleDataFetched} />

        {/* Banner de Publicidad para Monetización */}
        <Publicidad tipo="banner" />

        {/* Sección de Presentación API / TP */}
        <section className="api-section fade-in mt-5">
          <div className="row align-items-center">
            <div className="col-lg-6">
              <h2 className="display-6 mb-4">Ingeniería de Datos</h2>
              <p className="text-secondary">
                Este proyecto utiliza técnicas avanzadas de <strong>Web Scraping</strong> y 
                procesamiento asíncrono en .NET 8 para extraer información en tiempo real 
                de Mercado Libre.
              </p>
              <ul className="list-unstyled">
                <li className="mb-3 d-flex gap-2">
                  <span className="text-accent">✓</span>
                  <span><strong>Backend:</strong> ASP.NET Core Web API con HtmlAgilityPack.</span>
                </li>
                <li className="mb-3 d-flex gap-2">
                  <span className="text-accent">✓</span>
                  <span><strong>Frontend:</strong> React + CSS Moderno (Glassmorphism).</span>
                </li>
                <li className="mb-3 d-flex gap-2">
                  <span className="text-accent">✓</span>
                  <span><strong>Data:</strong> Análisis dinámico de selectores "Poly" de ML.</span>
                </li>
              </ul>
            </div>
            <div className="col-lg-6">
              <div className="code-block">
                <div className="d-flex justify-content-between mb-2">
                  <span className="text-secondary small">API Response Preview</span>
                  <div className="d-flex gap-1">
                    <div style={{ width: 8, height: 8, borderRadius: "50%", background: "#ff5f56" }}></div>
                    <div style={{ width: 8, height: 8, borderRadius: "50%", background: "#ffbd2e" }}></div>
                    <div style={{ width: 8, height: 8, borderRadius: "50%", background: "#27c93f" }}></div>
                  </div>
                </div>
                <pre style={{ margin: 0, fontSize: "0.8rem", color: "#8b949e" }}>
                  {apiResponse ? JSON.stringify({
                    success: true,
                    source: apiResponse.source,
                    info: apiResponse.info,
                    count: apiResponse.count,
                    sample_item: apiResponse.data[0]?.nombre.substring(0, 30) + "..."
                  }, null, 2) : "// Cargando metadatos de la API..."}
                </pre>
              </div>
            </div>
          </div>
        </section>

        {/* Publicidad secundaria antes de la música */}
        <div className="row mt-5">
          <div className="col-md-8">
            <p className="text-secondary">
              Nuestra plataforma se mantiene gratuita gracias al apoyo de nuestros patrocinadores. 
              Cada clic nos ayuda a seguir mejorando nuestros algoritmos de búsqueda.
            </p>
          </div>
          <div className="col-md-4">
            <Publicidad tipo="sidebar" />
          </div>
        </div>

        {/* Sección de música (opcional, integrada elegantemente) */}
        <div className="mt-5 pt-5 opacity-50 hover-opacity-100 transition">
          <p className="text-center small text-secondary">Acompaña tu búsqueda</p>
          <div className="ratio ratio-21x9 mx-auto" style={{ maxWidth: "600px", borderRadius: "12px", overflow: "hidden" }}>
             <iframe
              src="https://www.youtube.com/embed/98Akpf1ph2o"
              title="Musica de fondo"
              allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture"
              allowFullScreen
            ></iframe>
          </div>
        </div>
      </main>

      <footer className="py-4 text-center text-secondary border-top border-secondary-subtle mt-5">
        <p>© 2024 Trabajo Práctico 2 - Ingeniería de Software</p>
      </footer>
    </div>
  );
}

export default App;

