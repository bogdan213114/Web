using Microsoft.AspNetCore.Mvc;

namespace Web_Api.Services;

public interface IUserService
{
    Task<IActionResult> GetByIdAsync(long userId);

    Task<IActionResult> GetTasksAsync(long userId);
    Task<IActionResult> AddTaskAsync(long userId, string taskTitle, string taskDescription, bool taskIsDone);
    Task<IActionResult> DeleteTaskAsync(long userId, long taskId);
    Task<IActionResult> ChangeTaskAsync(long userId, long taskId, bool? newIsDoneStatus, string newTitle, string newDescription, long[] newGroupsId);

    Task<IActionResult> GetGroupsAsync(long id);
    Task<IActionResult> AddGroupAsync(long userId, string groupTitle);
    Task<IActionResult> DeleteGroupAsync(long userId, long groupId);
    Task<IActionResult> ChangeGroupAsync(long userId, long groupId, string newTitle, long[] tasksId);
}
