// namespace Domain.Entities.Common;
//
// public interface IRepository<TAggregate, TId>
//     where TAggregate : AggregateRoot<TId>
//     where TId : class
// {
//   Task<TAggregate> GetByIdAsync(TId id);
//   Task AddAsync(TAggregate aggregate);
//   Task UpdateAsync(TAggregate aggregate);
//   Task DeleteAsync(TAggregate aggregate);
// }