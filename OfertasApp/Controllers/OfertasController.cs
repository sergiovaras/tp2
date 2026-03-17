using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using HtmlAgilityPack;

namespace OfertasApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OfertasController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public OfertasController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [AllowAnonymous]
        [HttpGet("mercadolibre")]
        public async Task<IActionResult> GetOfertas([FromQuery] string? query)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36");

                // Si no hay query, buscamos "ofertas del día"
                string searchTerm = string.IsNullOrEmpty(query) ? "ofertas del dia" : query;
                string searchUrl = $"https://www.mercadolibre.com.ar/jm/search?as_word={searchTerm.Replace(" ", "%20")}";
                
                var response = await client.GetAsync(searchUrl);
                if (!response.IsSuccessStatusCode)
                {
                    return StatusCode((int)response.StatusCode, $"Error al acceder a Mercado Libre: {response.ReasonPhrase}");
                }

                var htmlContent = await response.Content.ReadAsStringAsync();
                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(htmlContent);

                var nodes = htmlDoc.DocumentNode.SelectNodes("//li[contains(@class, 'ui-search-layout__item')]") ??
                            htmlDoc.DocumentNode.SelectNodes("//div[contains(@class, 'poly-card')]") ??
                            htmlDoc.DocumentNode.SelectNodes("//div[contains(@class, 'ui-search-result')]");
                
                var ofertas = new List<object>();

                if (nodes != null)
                {
                    foreach (var node in nodes.Take(20))
                    {
                        try 
                        {
                            var titleNode = node.SelectSingleNode(".//h2[contains(@class, 'ui-search-item__title')]") ?? 
                                           node.SelectSingleNode(".//h3[contains(@class, 'ui-search-item__title')]") ??
                                           node.SelectSingleNode(".//a[contains(@class, 'poly-component__title')]") ??
                                           node.SelectSingleNode(".//h2");
                            
                            var linkNode = node.SelectSingleNode(".//a[contains(@class, 'ui-search-link')]") ??
                                          node.SelectSingleNode(".//a[contains(@class, 'poly-component__title')]") ??
                                          node.SelectSingleNode(".//a");

                            var imgNode = node.SelectSingleNode(".//img[contains(@class, 'ui-search-result-image__element')]") ??
                                         node.SelectSingleNode(".//img[contains(@class, 'poly-component__picture')]") ??
                                         node.SelectSingleNode(".//img");

                            // Precios
                            var priceNode = node.SelectSingleNode(".//span[contains(@class, 'andes-money-amount__fraction')]");
                            var originalPriceNode = node.SelectSingleNode(".//s[contains(@class, 'andes-money-amount')]//span[contains(@class, 'andes-money-amount__fraction')]") ??
                                                   node.SelectSingleNode(".//span[contains(@class, 'ui-search-price__part--undeline')]//span[contains(@class, 'andes-money-amount__fraction')]");
                            
                            // Descuento
                            var discountNode = node.SelectSingleNode(".//span[contains(@class, 'ui-search-price__discount')]") ??
                                              node.SelectSingleNode(".//span[contains(@class, 'poly-price__discount')]");

                            // Envío gratis
                            var shippingNode = node.SelectSingleNode(".//span[contains(@class, 'ui-search-item__shipping--free')]") ??
                                              node.SelectSingleNode(".//span[contains(@class, 'poly-component__shipping')]");

                            if (titleNode != null && linkNode != null)
                            {
                                string title = titleNode.InnerText.Trim();
                                string link = linkNode.GetAttributeValue("href", "");
                                
                                // Lógica robusta para imágenes
                                string image = "";
                                if (imgNode != null)
                                {
                                    // Probamos diferentes atributos que ML suele usar para lazy loading o diferentes resoluciones
                                    var imgAttrs = new[] { "data-src", "srcset", "src", "data-srcset" };
                                    foreach (var attr in imgAttrs)
                                    {
                                        var val = imgNode.GetAttributeValue(attr, "").Trim();
                                        if (!string.IsNullOrEmpty(val) && !val.Contains("placeholder") && !val.Contains("blank.gif"))
                                        {
                                            if (attr.Contains("srcset"))
                                            {
                                                image = val.Split(' ').FirstOrDefault() ?? "";
                                            }
                                            else
                                            {
                                                image = val;
                                            }
                                            
                                            if (!string.IsNullOrEmpty(image)) break;
                                        }
                                    }
                                }

                                if (string.IsNullOrEmpty(image))
                                {
                                     // Fallback: si no encontramos imagen en el nodo img, buscamos en los atributos del contenedor
                                     image = node.SelectSingleNode(".//div[contains(@class, 'ui-search-result-image')]//img")?.GetAttributeValue("src", "") ?? "";
                                }
                                
                                decimal price = 0;
                                if (priceNode != null) 
                                {
                                    string pStr = priceNode.InnerText.Replace(".", "").Replace(",", ".");
                                    decimal.TryParse(pStr, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out price);
                                }

                                decimal originalPrice = price;
                                if (originalPriceNode != null) 
                                {
                                    string opStr = originalPriceNode.InnerText.Replace(".", "").Replace(",", ".");
                                    decimal.TryParse(opStr, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out originalPrice);
                                }

                                string discount = discountNode?.InnerText.Trim() ?? "";
                                bool freeShipping = shippingNode != null && shippingNode.InnerText.ToLower().Contains("gratis");

                                if (string.IsNullOrEmpty(image) || image.Contains("placeholder"))
                                {
                                     image = imgNode?.GetAttributeValue("data-image", "") ?? "";
                                }

                                ofertas.Add(new
                                {
                                    id = Guid.NewGuid().ToString().Substring(0, 8),
                                    nombre = title,
                                    imagenUrl = image,
                                    precioOriginal = originalPrice,
                                    precioConDescuento = price,
                                    descuento = discount,
                                    envioGratis = freeShipping,
                                    permalink = link,
                                    vendedor = "Mercado Libre (Scraped)",
                                    fechaConsulta = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                                });
                            }
                        }
                        catch { }
                    }
                }

                return Ok(new {
                    success = true,
                    query = searchTerm,
                    count = ofertas.Count,
                    data = ofertas,
                    source = "Scraping Directo de Mercado Libre",
                    info = "Datos obtenidos en tiempo real para el Trabajo Práctico"
                });
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { success = false, message = $"Error interno: {ex.Message}" });
            }
        }
    }
}
