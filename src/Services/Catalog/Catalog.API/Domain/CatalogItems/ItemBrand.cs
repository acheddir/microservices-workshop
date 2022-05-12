using SharedKernel.Domain.Common;

namespace Catalog.API.Domain.CatalogItems;

public class ItemBrand : BaseEntity
{
    public string? Brand { get; set; }
}