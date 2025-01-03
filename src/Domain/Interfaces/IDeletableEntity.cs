namespace Domain.Interfaces;

public interface IDeletableEntity
{
    bool IsDeleted { get; }
    DateTime DeletedOnUtc { get; }
}