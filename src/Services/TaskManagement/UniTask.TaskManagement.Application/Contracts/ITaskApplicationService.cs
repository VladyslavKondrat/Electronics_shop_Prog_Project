using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UniTask.TaskManagement.Application.Contracts;

public interface ITaskApplicationService
{
    Task<Guid> CreateTaskRequestAsync(string title, string description, decimal cost);
    Task PublishTaskAsync(Guid taskId);
    Task<IEnumerable<TaskDto>> ExploreTasksAsync();
}
