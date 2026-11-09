using System;
using UniTask.TaskManagement.Domain.ValueObjects;

namespace UniTask.TaskManagement.Domain.Entities;


internal sealed class Payment
{
    public Guid Id { get; }
    public TaskCost Cost { get; }
    public Guid ExecutorId { get; }
    public DateTime RecordedAt { get; }
    public bool IsProcessed { get; private set; }

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

    internal void MarkAsProcessed()
    {
        IsProcessed = true;
    }
}