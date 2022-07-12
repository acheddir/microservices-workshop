using System.ComponentModel.DataAnnotations;
using MediatR;
using Sieve.Attributes;

namespace SharedKernel.Domain.Common;

public abstract class BaseEntity
{
    [Key]
    [Sieve(CanFilter = true, CanSort = true)]
    public virtual Guid Id { get; private set; } = Guid.NewGuid();
    
    public bool IsDeleted { get; private set; }
    
    public DateTime CreatedOn { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? LastModifiedOn { get; set; }
    public string? LastModifiedBy { get; set; }

    private readonly List<INotification> _domainEvents = new List<INotification>();
    public IReadOnlyCollection<INotification> DomainEvents => _domainEvents.AsReadOnly();

    public void UpdateCreationProperties(DateTime createdOn, string createdBy)
    {
        CreatedOn = createdOn;
        CreatedBy = createdBy;
    }
    
    public void UpdateModifiedProperties(DateTime? lastModifiedOn, string lastModifiedBy)
    {
        LastModifiedOn = lastModifiedOn;
        LastModifiedBy = lastModifiedBy;
    }
    
    public void UpdateIsDeleted(bool isDeleted)
    {
        IsDeleted = isDeleted;
    }
    
    public void QueueDomainEvent(INotification @event)
    {
        if(!DomainEvents.Contains(@event))
            AddDomainEvent(@event);
    }
    
    public void AddDomainEvent(INotification @event)
    {
        _domainEvents.Add(@event);
    }

    public void RemoveDomainEvent(INotification @event)
    {
        _domainEvents.Remove(@event);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    public bool IsTransient()
    {
        return Id == Guid.Empty;
    }
    
    public override bool Equals(object? obj)
    {
        if (obj is not BaseEntity item)
            return false;

        if (ReferenceEquals(this, item))
            return true;

        if (GetType() != item.GetType())
            return false;

        if (item.IsTransient() || IsTransient())
            return false;
        
        return item.Id == Id;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id.GetHashCode() ^ 31);
    }
}
