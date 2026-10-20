using Milan.Web.Models;

namespace Milan.Web.Services;

public sealed class CatalogService
{
    private static readonly IReadOnlyList<Product> Products =
    [
        new(1, "AeroMILA Halcón P2", "Dron profesional para inspeccionar techos y fachadas con video estable y maniobras precisas.", 19.50m, "/images/products/1.png", "AeroMILA", "Profesional"),
        new(2, "AeroMILA Brisa Mini", "Dron ligero para aprender a volar y grabar paseos recreativos en espacios abiertos.", 8.50m, "/images/products/2.png", "AeroMILA", "Recreativo"),
        new(3, "AgroVuelo Prisma 4K", "Captura video aéreo detallado para el seguimiento profesional de obras y la creación de contenido comercial.", 12m, "/images/products/3.png", "AgroVuelo", "Profesional"),
        new(4, "AeroMILA Centinela S1", "Apoya la vigilancia de perímetros con recorridos programables y transmisión de video en tiempo real.", 12m, "/images/products/4.png", "AeroMILA", "Profesional"),
        new(5, "AgroVuelo Órbita Foto", "Toma fotografías aéreas panorámicas para turismo, bienes raíces y promoción de espacios.", 8.50m, "/images/products/5.png", "AgroVuelo", "Fotografía"),
        new(6, "AeroMILA Campo V3", "Permite observar cultivos desde el aire y detectar zonas que requieren atención agrícola.", 12m, "/images/products/6.png", "AeroMILA", "Profesional"),
        new(7, "AgroVuelo Terrascan RTK", "Apoya levantamientos topográficos con posicionamiento preciso y cobertura de terrenos extensos.", 12m, "/images/products/7.png", "AgroVuelo", "Profesional"),
        new(8, "AgroVuelo Vigía X", "Facilita recorridos de vigilancia y la revisión visual de instalaciones y zonas de acceso.", 8.50m, "/images/products/8.png", "AgroVuelo", "Profesional"),
        new(9, "AgroVuelo Giro Go", "Dron compacto para practicar maniobras y tomar videos recreativos desde nuevas perspectivas.", 12m, "/images/products/9.png", "AgroVuelo", "Recreativo"),
        new(10, "AeroMILA Vista Aérea 6K", "Captura panorámicas de alta resolución con estabilización para proyectos de fotografía aérea.", 12m, "/images/products/10.png", "AeroMILA", "Fotografía"),
        new(11, "AeroMILA Lente Nube", "Produce tomas aéreas para recorridos visuales de propiedades y espacios turísticos.", 8.50m, "/images/products/11.png", "AeroMILA", "Fotografía"),
        new(12, "AgroVuelo Inspección RTK", "Ayuda a revisar infraestructura y obras con imágenes aéreas y referencias de ubicación precisas.", 12m, "/images/products/12.png", "AgroVuelo", "Profesional")
    ];

    public IReadOnlyList<Product> GetAll() => Products;
    public Product? Find(int id) => Products.FirstOrDefault(product => product.Id == id);
}
