using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Xunit;
using UniTask.TaskManagement.Application.Contracts;
using UniTask.TaskManagement.Application.Services;
using DomainTask = UniTask.TaskManagement.Domain.Aggregates.Task;

namespace UniTask.TaskManagement.Application.Tests;

public class TaskApplicationServiceTests
{
    private readonly Mock<ITaskRepository> _repositoryMock;
    private readonly TaskApplicationService _sut;

    public TaskApplicationServiceTests()
    {
        _repositoryMock = new Mock<ITaskRepository>();
        _sut = new TaskApplicationService(_repositoryMock.Object);
    }

    [Fact]
    public async Task CreateTaskRequestAsync_ValidData_ShouldCallAddAsyncAndReturnId()
    {
        // Arrange
        var title = "Math Homework";
        var description = "Calculus 101 Help";
        var cost = 50.0m;

        // Act
        var result = await _sut.CreateTaskRequestAsync(title, description, cost);

        // Assert
        Assert.NotEqual(Guid.Empty, result);
        _repositoryMock.Verify(r => r.AddAsync(
            It.Is<DomainTask>(t => t.Title == title && t.Description == description), 
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task PublishTaskAsync_ExistingDraftTask_ShouldPublishAndUpdateRepository()
    {
        // Arrange
        var task = new DomainTask(Guid.NewGuid(), "Title", "Description", 100m);
        _repositoryMock.Setup(r => r.GetByIdAsync(task.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(task);

        // Act
        await _sut.PublishTaskAsync(task.Id);

        // Assert
        Assert.Equal(DomainTask.TaskStatus.Published, task.Status);
        _repositoryMock.Verify(r => r.UpdateAsync(task, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task PublishTaskAsync_NonExistingTask_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        var taskId = Guid.NewGuid();
        DomainTask? nullTask = null;
        _repositoryMock.Setup(r => r.GetByIdAsync(taskId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(nullTask);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _sut.PublishTaskAsync(taskId));
    }

    [Fact]
    public async Task ExploreTasksAsync_ShouldReturnMappedDtos()
    {
        // Arrange
        var task1 = new DomainTask(Guid.NewGuid(), "Task 1", "Desc 1", 30m);
        var task2 = new DomainTask(Guid.NewGuid(), "Task 2", "Desc 2", 40m);
        var tasks = new List<DomainTask> { task1, task2 };

        _repositoryMock.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(tasks);

        // Act
        var result = await _sut.ExploreTasksAsync();

        // Assert
        var resultList = result.ToList();
        Assert.Equal(2, resultList.Count);
        Assert.Contains(resultList, dto => dto.Id == task1.Id && dto.Cost == 30m);
        Assert.Contains(resultList, dto => dto.Id == task2.Id && dto.Cost == 40m);
    }
}
