
using System;

namespace TaskManagement.Domain.Exceptions
{
    public class TaskAlreadyPublishedException : DomainException
    {
        public Guid TaskId { get; }

        public TaskAlreadyPublishedException(Guid taskId) 
            : base($"Task {taskId} is already published.", "task_already_published")
        {
            TaskId = taskId;
        }
    }
}
