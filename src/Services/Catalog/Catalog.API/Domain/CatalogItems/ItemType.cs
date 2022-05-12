using SharedKernel.Domain.Common;

namespace Catalog.API.Domain.CatalogItems;

public class ItemType : BaseEntity
{
    public string? Type { get; set; }
}