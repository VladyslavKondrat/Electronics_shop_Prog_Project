using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UniTask.TaskManagement.Application.Contracts;

namespace UniTask.TaskManagement.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly ITaskApplicationService _taskService;

    public TasksController(ITaskApplicationService taskService)
    {
        _taskService = taskService;
    }

    [HttpGet]
    public async Task<IActionResult> ExploreTasks()
    {
        var tasks = await _taskService.ExploreTasksAsync();
        return Ok(tasks); // 200 OK
    }

    [HttpPost]
    public async Task<IActionResult> CreateTaskRequest([FromBody] CreateTaskRequestDto request)
    {
        try
        {
            var id = await _taskService.CreateTaskRequestAsync(request.Title, request.Description, request.Cost);
            return CreatedAtAction(nameof(ExploreTasks), new { id }, new { Id = id }); // 201 Created
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { Error = "invalid_input", Message = ex.Message }); // 400 Bad Request
        }
        catch (Exception ex)
        {
            return UnprocessableEntity(new { Error = "unprocessable_entity", Message = ex.Message }); // 422 Unprocessable Entity
        }
    }

    [HttpPut("{id}/publish")]
    public async Task<IActionResult> PublishTask(Guid id)
    {
        try
        {
            await _taskService.PublishTaskAsync(id);
            return Ok(); // 200 OK
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { Error = "task_not_found", Message = ex.Message }); // 404 Not Found
        }
        catch (InvalidOperationException ex)
        {
            return UnprocessableEntity(new { Error = "invalid_state_transition", Message = ex.Message }); // 422 Unprocessable Entity
        }
    }
}

public record CreateTaskRequestDto(string Title, string Description, decimal Cost);
