namespace Milan.Web.Models;

public sealed record Product(int Id, string Name, string Description, decimal Price, string ImagePath, string Brand, string Type);
