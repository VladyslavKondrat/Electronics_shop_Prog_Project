using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniTask.TaskManagement.Application.Contracts;
using DomainTask = UniTask.TaskManagement.Domain.Aggregates.Task;

namespace UniTask.TaskManagement.Application.Services;

public class TaskApplicationService : ITaskApplicationService
{
    private readonly ITaskRepository _repository;

    public TaskApplicationService(ITaskRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> CreateTaskRequestAsync(string title, string description, decimal cost)
    {
        var id = Guid.NewGuid();
        var task = new DomainTask(id, title, description, cost);
        await _repository.AddAsync(task);
        return task.Id;
    }

    public async Task PublishTaskAsync(Guid taskId)
    {
        var task = await _repository.GetByIdAsync(taskId);
        if (task == null)
        {
            throw new KeyNotFoundException($"Task with ID '{taskId}' was not found.");
        }

        task.PublishTask();
        await _repository.UpdateAsync(task);
    }

    public async Task<IEnumerable<TaskDto>> ExploreTasksAsync()
    {
        var tasks = await _repository.GetAllAsync();
        return tasks.Select(t => new TaskDto(
            t.Id,
            t.Title,
            t.Description,
            t.Cost.Amount,
            t.Status.ToString(),
            t.AssignedExecutorId,
            t.CreatedAt,
            t.PublishedAt,
            t.AssignedAt,
            t.TimeSubmitted
        ));
    }
}
