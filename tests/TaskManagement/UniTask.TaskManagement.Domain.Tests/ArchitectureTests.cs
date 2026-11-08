using Xunit;
using UniTask.TaskManagement.Domain.Aggregates;
using UniTask.TaskManagement.Domain.Entities;
using UniTask.TaskManagement.Domain.ValueObjects;
using DomainTask = UniTask.TaskManagement.Domain.Aggregates.Task;

namespace UniTask.TaskManagement.Domain.Tests;

/// <summary>
/// Architecture tests to enforce Clean Architecture layer boundaries at runtime.
/// These tests verify key DDD patterns are properly implemented.
/// </summary>
public class ArchitectureTests
{
    [Fact]
    public void Payment_Should_Be_Internal_Entity()
    {
        var paymentType = typeof(Payment);

        // Payment type exists and is not public
        Assert.NotNull(paymentType);
        Assert.True(paymentType.IsNotPublic,
            "Payment should be internal (visible only within Domain layer)");
    }

    [Fact]
    public void Payment_Constructor_Should_Be_Internal()
    {
        var paymentCtor = typeof(Payment).GetConstructor(
            System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic,
            null,
            new[] { typeof(Guid), typeof(TaskCost) },
            null);

        Assert.NotNull(paymentCtor);
        Assert.False(paymentCtor.IsPublic,
            "Payment constructor should be internal (only Task aggregate can create Payment)");
    }

    [Fact]
    public void TaskCost_Should_Be_Sealed_ValueObject()
    {
        var taskCostType = typeof(TaskCost);

        Assert.True(taskCostType.IsSealed, 
            "Value Object should be sealed (prevent inheritance)");
        Assert.True(typeof(IEquatable<TaskCost>).IsAssignableFrom(taskCostType),
            "Value Object should implement IEquatable<T> for value comparison");
    }

    [Fact]
    public void Task_Should_Be_Sealed_AggregateRoot()
    {
        var taskType = typeof(DomainTask);

        Assert.True(taskType.IsSealed,
            "Aggregate Root should be sealed (prevent inheritance violations)");
    }

    [Fact]
    public void Task_Payments_Should_Be_ReadOnly()
    {
        var paymentsProp = typeof(DomainTask).GetProperty("Payments");

        Assert.NotNull(paymentsProp);
        Assert.True(paymentsProp.CanRead, "Payments should be readable");
        Assert.False(paymentsProp.CanWrite, "Payments should be read-only (no setter)");

        // Verify it returns IReadOnlyCollection
        var returnType = paymentsProp.PropertyType;
        Assert.True(returnType.Name == "IReadOnlyCollection`1",
            $"Payments should return IReadOnlyCollection, not {returnType.Name}");
    }

    [Fact]
    public void Task_Cost_Should_Be_TaskCost_ValueObject()
    {
        var costProp = typeof(DomainTask).GetProperty("Cost");

        Assert.NotNull(costProp);
        Assert.Equal(typeof(TaskCost), costProp.PropertyType);
    }

    [Fact]
    public void TaskCost_Should_Support_Implicit_Decimal_Conversion()
    {
        var implicitOp = typeof(TaskCost).GetMethod(
            "op_Implicit",
            System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public,
            null,
            new[] { typeof(TaskCost) },
            null);

        Assert.NotNull(implicitOp);
        Assert.Equal(typeof(decimal), implicitOp.ReturnType);
    }
}
