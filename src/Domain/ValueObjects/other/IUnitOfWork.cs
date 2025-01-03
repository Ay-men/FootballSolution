namespace Domain.Entities.Common;

public interface IUnitOfWork
{
  Task CommitAsync(CancellationToken cancellationToken = default);
}