namespace UniTask.TaskManagement.Domain.Aggregates;

/// <summary>
/// Represents the lifecycle status of a task.
/// </summary>
public enum TaskStatus
{
    /// <summary>
    /// Task has been created but not yet published.
    /// </summary>
    Draft = 0,

    /// <summary>
    /// Task has been published and is available for executors.
    /// </summary>
    Published = 1,

    /// <summary>
    /// Task has been assigned to an executor.
    /// </summary>
    Assigned = 2,

    /// <summary>
    /// Task completion has been submitted and is pending review.
    /// </summary>
    Completed = 3,

    /// <summary>
    /// Task has been cancelled.
    /// </summary>
    Cancelled = 4
}
