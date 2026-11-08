using System;
using System.Collections.Generic;
using System.Text;


namespace UniTask.TaskManagement.Domain.Exceptions;

public sealed class InvalidCostException : DomainException
{
    public decimal AttemptedCost { get; }

    public InvalidCostException(decimal attemptedCost)
        : base(
            code: "task.invalid_cost",
            message: $"Your attempted cost ({attemptedCost:F2}) is more than the possible cost or less than zero.")
    {
        AttemptedCost = attemptedCost;
    }
}

    