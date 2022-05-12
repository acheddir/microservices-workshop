namespace SharedKernel.Application.Common.Models;

public abstract class BasePaginationParameters
{
    internal virtual int MaxPageSize { get; } = 20;
    internal virtual int DefaultPageSize { get; set; } = 10;

    public virtual int PageNumber { get; set; } = 1;

    public int PageSize
    {
        get => DefaultPageSize;
        set => DefaultPageSize = value > MaxPageSize ? MaxPageSize : value;
    }
}