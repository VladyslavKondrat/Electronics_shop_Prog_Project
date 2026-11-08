using UniTask.TaskManagement.Domain.Exceptions;

namespace UniTask.TaskManagement.Domain.ValueObjects;

/// <summary>
/// Represents an immutable task cost value object.
/// Encapsulates the cost amount and validates it falls within acceptable business bounds (0.01m - 100000m).
/// Value objects have no identity (two TaskCost instances with the same amount are equal).
/// </summary>
public sealed class TaskCost : IEquatable<TaskCost>
{
    private const decimal MinCost = 0.01m;
    private const decimal MaxCost = 100000m;

    /// <summary>
    /// The monetary amount of this task cost.
    /// </summary>
    public decimal Amount { get; }

    /// <summary>
    /// Creates a new TaskCost value object.
    /// </summary>
    /// <param name="amount">The cost amount. Must be between 0.01m and 100000m.</param>
    /// <exception cref="InvalidCostException">Thrown when amount is outside the valid range.</exception>
    public TaskCost(decimal amount)
    {
        if (amount < MinCost || amount > MaxCost)
        {
            throw new InvalidCostException(amount);
        }

        Amount = amount;
    }

    /// <summary>
    /// Determines whether two TaskCost instances are equal by their amount.
    /// </summary>
    public bool Equals(TaskCost? other) => other is not null && Amount == other.Amount;

    /// <summary>
    /// Determines whether the specified object is equal to the current TaskCost.
    /// </summary>
    public override bool Equals(object? obj) => obj is TaskCost other && Equals(other);

    /// <summary>
    /// Serves as the default hash function.
    /// </summary>
    public override int GetHashCode() => Amount.GetHashCode();

    /// <summary>
    /// Returns a string representation of the cost.
    /// </summary>
    public override string ToString() => Amount.ToString("F2");

    /// <summary>
    /// Implicitly converts a TaskCost to its decimal Amount.
    /// </summary>
    public static implicit operator decimal(TaskCost taskCost) => taskCost.Amount;
}
