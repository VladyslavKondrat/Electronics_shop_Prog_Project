using UniTask.TaskManagement.Domain.ValueObjects;

namespace UniTask.TaskManagement.Domain.Entities;

/// <summary>
/// Represents a payment line item within a Task aggregate.
/// Payment is an internal entity (not an aggregate root) with local identity.
/// It can only be created and modified through the Task aggregate root.
/// </summary>
internal sealed class Payment
{
    /// <summary>
    /// Local identity – unique only within the context of its parent Task, not globally.
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// The cost associated with this payment (immutable value object).
    /// </summary>
    public TaskCost Cost { get; }

    /// <summary>
    /// The executor who performed the work and is entitled to this payment.
    /// References the separate Executor aggregate by Guid identifier only.
    /// </summary>
    public Guid ExecutorId { get; }

    /// <summary>
    /// When this payment was recorded.
    /// </summary>
    public DateTime RecordedAt { get; }

    /// <summary>
    /// Whether this payment has been processed/settled.
    /// </summary>
    public bool IsProcessed { get; private set; }

    /// <summary>
    /// Internal constructor: only the Task aggregate root can instantiate Payment.
    /// This prevents EShop.Application from creating payments outside the aggregate boundary.
    /// </summary>
    internal Payment(Guid executorId, TaskCost cost)
    {
        if (executorId == Guid.Empty)
            throw new ArgumentException("Executor ID cannot be empty.", nameof(executorId));

        if (cost is null)
            throw new ArgumentNullException(nameof(cost), "Cost cannot be null.");

        Id = Guid.NewGuid();
        ExecutorId = executorId;
        Cost = cost;
        RecordedAt = DateTime.UtcNow;
        IsProcessed = false;
    }

    /// <summary>
    /// Internal method: only the Task aggregate root can mark payments as processed.
    /// </summary>
    internal void MarkAsProcessed()
    {
        IsProcessed = true;
    }
}
