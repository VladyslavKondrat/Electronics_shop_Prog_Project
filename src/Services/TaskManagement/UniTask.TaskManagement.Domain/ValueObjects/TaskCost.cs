using UniTask.TaskManagement.Domain.Exceptions;

namespace UniTask.TaskManagement.Domain.ValueObjects;


public sealed class TaskCost : IEquatable<TaskCost>
{
    private const decimal MinCost = 0.01m;
    private const decimal MaxCost = 100000m;

    public decimal Amount { get; }


    public TaskCost(decimal amount)
    {
        if (amount < MinCost || amount > MaxCost)
        {
            throw new InvalidCostException(amount);
        }

        Amount = amount;
    }


    public bool Equals(TaskCost? other) => other is not null && Amount == other.Amount;


    public override bool Equals(object? obj) => obj is TaskCost other && Equals(other);


    public override int GetHashCode() => Amount.GetHashCode();


    public override string ToString() => Amount.ToString("F2");


    public static implicit operator decimal(TaskCost taskCost) => taskCost.Amount;
}
