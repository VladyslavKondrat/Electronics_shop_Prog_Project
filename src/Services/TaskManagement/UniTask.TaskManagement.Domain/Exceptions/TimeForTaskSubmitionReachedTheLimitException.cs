using System;
using System.Collections.Generic;
using System.Text;

namespace UniTask.TaskManagement.Domain.Exceptions;

public sealed class TimeForTaskSubmitionReachedTheLimitException : DomainException
{
    public decimal TimeSinceAssigned { get; }
    public decimal MaxRequiredTime { get; }

    public TimeForTaskSubmitionReachedTheLimitException(decimal timeSinceAssigned, decimal maxRequiredTime)
        : base(
            code: "task.time_limit_reached",
            message: $"Your time since assigned ({timeSinceAssigned:F2}) is more than the required time ({maxRequiredTime:F2}).")
    {
        TimeSinceAssigned = timeSinceAssigned;
        MaxRequiredTime = maxRequiredTime;
    }
}
