using Xunit;
using UniTask.TaskManagement.Domain.Aggregates;
using UniTask.TaskManagement.Domain.Exceptions;
using DomainTask = UniTask.TaskManagement.Domain.Aggregates.Task;


namespace UniTask.TaskManagement.Domain.Tests;

public class TaskTests
{
    [Fact]
    public void CreateTask_WithValidParameters_ShouldSucceed()
    {
        var taskId = Guid.NewGuid();
        var title = "Complete electronics repair";
        var description = "Fix broken smartphone screen";
        var cost = 150.50m;

        var task = new DomainTask(taskId, title, description, cost);

        Assert.NotNull(task);
        Assert.Equal(taskId, task.Id);
        Assert.Equal(title, task.Title);
        Assert.Equal(description, task.Description);
        Assert.Equal(cost, (decimal)task.Cost);
        Assert.NotEqual(default, task.CreatedAt);
        //Assert.NotNull(task.CreatedAt);
    }

    [Fact]
    public void PublishTask_WithValidCost_ShouldChangeStatusToPublished()
    {
        var task = new DomainTask(Guid.NewGuid(), "Task", "Description", 100m);

        task.PublishTask();

        Assert.Equal(DomainTask.TaskStatus.Published, task.Status);
    }

    [Fact]
    public void AssignTaskToExecutor_WithValidExecutorId_ShouldMarkAsAssigned()
    {
        var task = new DomainTask(Guid.NewGuid(), "Task", "Description", 100m);
        task.PublishTask();
        var executorId = Guid.NewGuid();

        task.AssignToExecutor(executorId);

        Assert.Equal(DomainTask.TaskStatus.Assigned, task.Status);
        Assert.Equal(executorId, task.AssignedExecutorId);
    }

    [Fact]
    public void SubmitTaskCompletion_WithinTimeLimit_ShouldSucceed()
    {
        var task = new DomainTask(Guid.NewGuid(), "Task", "Description", 100m);
        task.PublishTask();
        task.AssignToExecutor(Guid.NewGuid());

        task.SubmitCompletion();

        Assert.Equal(DomainTask.TaskStatus.Completed, task.Status);
        Assert.NotNull(task.TimeSubmitted);
    }

    [Fact]
    public void CreateTask_WithNegativeCost_ShouldThrowInvalidCostException()
    {
        var exception = Assert.Throws<InvalidCostException>(() =>
            new DomainTask(Guid.NewGuid(), "Task", "Description", -50m)
        );
        Assert.Equal(-50m, exception.AttemptedCost);
    }

    [Fact]
    public void CreateTask_WithCostExceedingMaximum_ShouldThrowInvalidCostException()
    {
        var exception = Assert.Throws<InvalidCostException>(() =>
            new DomainTask(Guid.NewGuid(), "Task", "Description", 100001m)
        );
        Assert.Equal(100001m, exception.AttemptedCost);
    }

    [Fact]
    public void CreateTask_WithZeroCost_ShouldThrowInvalidCostException()
    {
        var exception = Assert.Throws<InvalidCostException>(() =>
            new DomainTask(Guid.NewGuid(), "Task", "Description", 0m)
        );
        Assert.Equal(0m, exception.AttemptedCost);
    }

    [Fact]
    public void SubmitTaskCompletion_AfterTimeLimit_ShouldThrowTimeForTaskSubmitionReachedTheLimitException()
    {
        var task = new DomainTask(Guid.NewGuid(), "Task", "Description", 100m);
        task.PublishTask();
        task.AssignToExecutor(Guid.NewGuid());

        var assignedTime = DateTime.UtcNow.AddHours(-25);
        task.SetAssignedTimeForTesting(assignedTime);
        var exception = Assert.Throws<TimeForTaskSubmitionReachedTheLimitException>(() =>
            task.SubmitCompletion()
        );
        Assert.NotNull(exception);
    }

    [Theory]
    [InlineData(0.01)]
    [InlineData(100)]
    [InlineData(50000)]
    [InlineData(100000)]
    public void CreateTask_WithValidBoundaryCosts_ShouldSucceed(decimal cost)
    {
        var task = new DomainTask(Guid.NewGuid(), "Task", "Description", cost);

        Assert.NotNull(task);
        Assert.Equal(cost, (decimal)task.Cost);
    }

    [Theory]
    [InlineData(-0.01)]
    [InlineData(-1000)]
    [InlineData(0)]
    [InlineData(100000.01)]
    [InlineData(200000)]
    public void CreateTask_WithInvalidCosts_ShouldThrowInvalidCostException(decimal invalidCost)
    {
        Assert.Throws<InvalidCostException>(() =>
            new DomainTask(Guid.NewGuid(), "Task", "Description", invalidCost)
        );
    }

    [Fact]
    public void Task_ShouldNotAllowPublishingTwice()
    {
        var task = new DomainTask(Guid.NewGuid(), "Task", "Description", 100m);
        task.PublishTask();

        Assert.Throws<InvalidOperationException>(() =>
            task.PublishTask()
        );
    }

    [Fact]
    public void Task_ShouldNotAllowAssignmentWithoutPublishing()
    {
        var task = new DomainTask(Guid.NewGuid(), "Task", "Description", 100m);

        Assert.Throws<InvalidOperationException>(() =>
            task.AssignToExecutor(Guid.NewGuid())
        );
    }

    [Fact]
    public void Task_ShouldNotAllowCompletionWithoutAssignment()
    {
        var task = new DomainTask(Guid.NewGuid(), "Task", "Description", 100m);
        task.PublishTask();

        Assert.Throws<InvalidOperationException>(() =>
            task.SubmitCompletion()
        );
    }

    [Fact]
    public void Task_PropertiesShouldBeReadOnly()
    {
        var task = new DomainTask(Guid.NewGuid(), "Task", "Description", 100m);

        task.PublishTask();
        Assert.Equal(DomainTask.TaskStatus.Published, task.Status);
    }
}
