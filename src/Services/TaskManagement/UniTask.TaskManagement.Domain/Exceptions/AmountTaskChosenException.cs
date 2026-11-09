using System;
using System.Collections.Generic;
using System.Text;

namespace UniTask.TaskManagement.Domain.Exceptions;

public sealed class AmountTaskChosenException : DomainException
{
    public decimal CurrentTaskAmount { get; }
    public decimal MaxTaskAmount { get; }

    public AmountTaskChosenException(decimal currentTaskAmount, decimal maxTaskAmount)
        : base(
            code: "task.task_amount_exceeded",
            message: $"Your current task amount ({currentTaskAmount:F2}) is more than the maximum allowed ({maxTaskAmount:F2}).")
    {
        CurrentTaskAmount = currentTaskAmount;
        MaxTaskAmount = maxTaskAmount;
    }
}
