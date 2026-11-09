using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DomainTask = UniTask.TaskManagement.Domain.Aggregates.Task;

namespace UniTask.TaskManagement.Application.Contracts;

public interface ITaskRepository
{
    Task<DomainTask?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<DomainTask>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync(DomainTask task, CancellationToken cancellationToken = default);
    Task UpdateAsync(DomainTask task, CancellationToken cancellationToken = default);
}
