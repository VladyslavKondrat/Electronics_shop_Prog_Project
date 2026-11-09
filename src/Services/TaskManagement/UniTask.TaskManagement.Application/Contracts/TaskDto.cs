using System;

namespace UniTask.TaskManagement.Application.Contracts;

public record TaskDto(
    Guid Id,
    string Title,
    string Description,
    decimal Cost,
    string Status,
    Guid? AssignedExecutorId,
    DateTime CreatedAt,
    DateTime? PublishedAt,
    DateTime? AssignedAt,
    DateTime? TimeSubmitted);
