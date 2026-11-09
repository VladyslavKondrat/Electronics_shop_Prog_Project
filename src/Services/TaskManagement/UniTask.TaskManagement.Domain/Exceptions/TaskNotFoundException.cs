
using System;

namespace TaskManagement.Domain.Exceptions
{
    public class TaskNotFoundException : DomainException
    {
        public Guid TaskId { get; }

        public TaskNotFoundException(Guid taskId) 
            : base($"Task {taskId} was not found.", "task_not_found")
        {
            TaskId = taskId;
        }
    }
}
