using Microsoft.AspNetCore.Mvc;
using Web_Api.Models.DataModels;
using Web_Api.Models.DataModels.Requests;
using Web_Api.Models.Validation;
using Web_Api.Services;

namespace Web_Api.Controllers;

[Route("api")]
[ApiController]
public class DataController : ControllerBase
{
    private readonly UserService _userService;
    private readonly ValidationService _validationService;

    public DataController(  
        ValidationService validationService,
        UserService userService)
    {
        _userService = userService;
        _validationService = validationService;
    }

    async Task<(bool IsValid, JWT_ValidationResult Result, ActionResult Response)> CheckJWT(string jwt)
    {

        JWT_ValidationResult result = await _validationService.ValidateJWTAsync(jwt);

        return result switch
        {
            { IsValid: true } => (true, result, null),

            { IsNotExpired: false } => (false, result, new UnauthorizedObjectResult(
                new NoContentResponse { State = State.JWT_Expired })),

            { IsSignatureValid: false } => (false, result, new UnauthorizedObjectResult(
                new NoContentResponse { State = State.JWT_SignatureNotValidated })),

            _ => (false, result, new UnauthorizedObjectResult(
                new NoContentResponse { State = State.JWT_Broken })),
        };
    }


    /// <summary>
    /// Gets user using id, encoded in given jwt.
    /// </summary>
    /// <param name="jwt"></param>
    /// <response code="200">Returns <code>GetResponse&lt;User&gt;</code> with successful state.</response>
    /// <response code="401">If jwt is not validated.</response>
    [HttpGet("getUser")]
    public async Task<IActionResult> GetUser([FromHeader] string jwt)
    {
        var (isValid, validationResult, response) = await CheckJWT(jwt);
        if (!isValid)
        {
            return response;
        }
        long userId = validationResult.JWT.Value.AccountId;
        return await _userService.GetByIdAsync(userId);
    }


    /// <summary>
    /// Gets user's tasks using id, encoded in given jwt.
    /// </summary>
    /// <param name="jwt"></param>
    /// <response code="200">Returns <code>GetResponse&lt;List&lt;ToDoTask&gt;&gt;</code> with successful state.</response>
    /// <response code="401">If jwt is not validated.</response>
    [HttpGet("getTasks")]
    public async Task<IActionResult> GetTasks([FromHeader] string jwt)
    {
        var (isValid, validationResult, response) = await CheckJWT(jwt);
        if (!isValid)
        {
            return response;
        }
        long userId = validationResult.JWT.Value.AccountId;
        return await _userService.GetTasksAsync(userId);
    }


    /// <summary>
    /// Creates new task using user's id in jwt.
    /// </summary>
    /// <param name="jwt"></param>
    /// <param name="request"></param>
    /// <response code="200">Returns <code>PostResponse</code> with added task id.</response>
    /// <response code="400">If task title or description not validated.</response>
    /// <response code="401">If jwt is not validated.</response>
    [HttpPost("createTask")]
    public async Task<IActionResult> AddTask(
        [FromHeader] string jwt, [FromBody] CreateTaskRequest request)
    {
        var (isValid, validationResult, response) = await CheckJWT(jwt);
        if (!isValid)
        {
            return response;
        }
        long userId = validationResult.JWT.Value.AccountId;
        return await _userService.AddTaskAsync(userId, request.Title, request.Description, request.IsDone);
    }


    /// <summary>
    /// Deletes task with given id.
    /// </summary>
    /// <param name="jwt"></param>
    /// <param name="taskId"></param>
    /// <response code="200">Returns <code>DeleteResponse</code>.</response>
    /// <response code="401">If jwt is not validated.</response>
    /// <response code="404">If task with such id not found.</response>
    [HttpDelete("deleteTask/{taskId}")]
    public async Task<IActionResult> DeleteTask(
        [FromHeader] string jwt, [FromRoute] long taskId)
    {
        var (isValid, validationResult, response) = await CheckJWT(jwt);
        if (!isValid)
        {
            return response;
        }
        long userId = validationResult.JWT.Value.AccountId;
        return await _userService.DeleteTaskAsync(userId, taskId);
    }


    /// <summary>
    /// Change task's properties as is done status, title and description.
    /// </summary>
    /// <param name="jwt"></param>
    /// <param name="taskId"></param>
    /// <param name="request"></param>
    /// <response code="200">Returns <code>PutResponse</code>.</response>
    /// <response code="400">If no changes were commited.</response>
    /// <response code="401">If jwt is not validated.</response>
    [HttpPut("changeTask/{taskId}")]
    public async Task<IActionResult> ChangeTask(
        [FromHeader] string jwt,
        [FromRoute] long taskId,
        [FromBody] ChangeTaskRequest request)
    {
        var (isValid, validationResult, response) = await CheckJWT(jwt);
        if (!isValid)
        {
            return response;
        }
        long userId = validationResult.JWT.Value.AccountId;
        return await _userService.ChangeTaskAsync
            (userId, taskId, request.NewIsDone, request.NewTitle, request.NewDescription,request.NewGroupsId);
    }


    /// <summary>
    /// Gets user's groups using id, encoded in given jwt.
    /// </summary>
    /// <param name="jwt"></param>
    /// <response code="200">Returns <code>GetResponse&lt;List&lt;TaskGroup&gt;&gt;</code> with successful state.</response>
    /// <response code="401">If jwt is not validated.</response>
    [HttpGet("getGroups")]
    public async Task<IActionResult> GetGroups(
        [FromHeader] string jwt)
    {
        var (isValid, validationResult, response) = await CheckJWT(jwt);
        if (!isValid)
        {
            return response;
        }
        long userId = validationResult.JWT.Value.AccountId;
        return await _userService.GetGroupsAsync(userId);
    }


    /// <summary>
    /// Creates new group using user's id in jwt.
    /// </summary>
    /// <param name="jwt"></param>
    /// <param name="request"></param>
    /// <response code="200">Returns <code>PostResponse</code> with added group id.</response>
    /// <response code="400">If group title not validated.</response>
    /// <response code="401">If jwt is not validated.</response>
    [HttpPost("createGroup")]
    public async Task<IActionResult> AddGroup(
        [FromHeader] string jwt, [FromBody] CreateGroupRequest request)
    {
        var (isValid, validationResult, response) = await CheckJWT(jwt);
        if (!isValid)
        {
            return response;
        }
        long userId = validationResult.JWT.Value.AccountId;
        return await _userService.AddGroupAsync(userId, request.Title);
    }


    /// <summary>
    /// Deletes group with given id.
    /// </summary>
    /// <param name="jwt"></param>
    /// <param name="groupId"></param>
    /// <response code="200">Returns <code>DeleteResponse</code>.</response>
    /// <response code="401">If jwt is not validated.</response>
    /// <response code="404">If group with such id not found.</response>
    [HttpDelete("deleteGroup/{groupId}")]
    public async Task<IActionResult> DeleteGroup(
        [FromHeader] string jwt, [FromRoute] long groupId)
    {
        var (isValid, validationResult, response) = await CheckJWT(jwt);
        if (!isValid)
        {
            return response;
        }
        long userId = validationResult.JWT.Value.AccountId;
        return await _userService.DeleteGroupAsync(userId, groupId);
    }


    /// <summary>
    /// Change groups's title.
    /// </summary>
    /// <param name="jwt"></param>
    /// <param name="groupId"></param>
    /// <param name="request"></param>
    /// <response code="200">Returns <code>PutResponse</code>.</response>
    /// <response code="400">If no changes were commited.</response>
    /// <response code="401">If jwt is not validated.</response>
    [HttpPut("changeGroup/{groupId}")]
    public async Task<IActionResult> ChangeGroup(
        [FromHeader] string jwt,
        [FromRoute]long groupId,
        [FromBody] ChangeGroupRequest request)
    {
        var (isValid, validationResult, response) = await CheckJWT(jwt);
        if (!isValid)
        {
            return response;
        }
        long userId = validationResult.JWT.Value.AccountId;
        return await _userService.ChangeGroupAsync(userId, groupId, request.NewGroupTitle, request.NewTasksId);
    }


    /// <summary>
    /// Add task to groups with given ids.
    /// </summary>
    /// <param name="jwt"></param>
    /// <param name="taskId"></param>
    /// <param name="request"></param>
    /// <response code="200">Returns <code>PutResponse</code>.</response>
    /// <response code="400">If groups or task don't belong to a given user or don't exist.</response>
    /// <response code="401">If jwt is not validated.</response>
    [HttpPut("addGroupsToTask/{taskId}")]
    public async Task<IActionResult> AddGroupsToTask(
        [FromHeader] string jwt,
        [FromRoute]long taskId,
        [FromBody] ModifyRelatedEntitiesRequest request)
    {
        var (isValid, validationResult, response) = await CheckJWT(jwt);
        if (!isValid)
        {
            return response;
        }
        long userId = validationResult.JWT.Value.AccountId;
        return await _userService.AddGroupsToTaskAsync(userId, taskId, request.DependentEntitiesId);
    }


    /// <summary>
    /// Cleans tasks grouplist and adds new groups by ids.
    /// </summary>
    /// <param name="jwt"></param>
    /// <param name="taskId"></param>
    /// <param name="request"></param>
    /// <response code="200">Returns <code>PutResponse</code>.</response>
    /// <response code="400">If groups or task don't belong to a given user or don't exist.</response>
    /// <response code="401">If jwt is not validated.</response>
    [HttpPut("refreshGroupsOfTask/{taskId}")]
    public async Task<IActionResult> RefreshGroupsInTask(
        [FromHeader] string jwt,
        [FromRoute]long taskId,
        [FromBody] ModifyRelatedEntitiesRequest request)
    {
        var (isValid, validationResult, response) = await CheckJWT(jwt);
        if (!isValid)
        {
            return response;
        }
        long userId = validationResult.JWT.Value.AccountId;
        return await _userService.RefreshGroupsOfTaskAsync(userId, taskId, request.DependentEntitiesId);
    }


    /// <summary>
    /// Add group to tasks with given ids.
    /// </summary>
    /// <param name="jwt"></param>
    /// <param name="groupId"></param>
    /// <param name="request"></param>
    /// <response code="200">Returns <code>PutResponse</code>.</response>
    /// <response code="400">If tasks or group don't belong to a given user or don't exist.</response>
    /// <response code="401">If jwt is not validated.</response>
    [HttpPut("addTasksToGroup/{groupId}")]
    public async Task<IActionResult> AddTasksToGroup(
        [FromHeader] string jwt,
        [FromRoute]long groupId,
        [FromBody] ModifyRelatedEntitiesRequest request)
    {
        var (isValid, validationResult, response) = await CheckJWT(jwt);
        if (!isValid)
        {
            return response;
        }
        long userId = validationResult.JWT.Value.AccountId;
        return await _userService.AddTasksToGroupAsync(userId, groupId, request.DependentEntitiesId);
    }


    /// <summary>
    /// Cleans groups tasklist and adds new tasks by ids.
    /// </summary>
    /// <param name="jwt"></param>
    /// <param name="groupId"></param>
    /// <param name="request"></param>
    /// <response code="200">Returns <code>PutResponse</code>.</response>
    /// <response code="400">If tasks or group don't belong to a given user or don't exist.</response>
    /// <response code="401">If jwt is not validated.</response>
    [HttpPut("refreshTasksOfGroup/{groupId}")]
    public async Task<IActionResult> RefreshTasksInGroup(
        [FromHeader] string jwt,
        [FromRoute]long groupId,
        [FromBody] ModifyRelatedEntitiesRequest request)
    {
        var (isValid, validationResult, response) = await CheckJWT(jwt);
        if (!isValid)
        {
            return response;
        }
        long userId = validationResult.JWT.Value.AccountId;
        return await _userService.RefreshTasksOfGroupAsync(userId, groupId, request.DependentEntitiesId);
    }


    /// <summary>
    /// Deletes selected tasks in group.
    /// </summary>
    /// <param name="jwt"></param>
    /// <param name="groupId"></param>
    /// <param name="request"></param>
    /// <response code="200">Returns <code>PutResponse</code>.</response>
    /// <response code="400">If tasks or group don't belong to a given user or don't exist.</response>
    /// <response code="401">If jwt is not validated.</response>
    [HttpPut("deleteTasksOfGroup/{groupId}")]
    public async Task<IActionResult> DeleteTasksOfGroup(
        [FromHeader] string jwt,
        [FromRoute]long groupId,
        [FromBody] ModifyRelatedEntitiesRequest request)
    {
        var (isValid, validationResult, response) = await CheckJWT(jwt);
        if (!isValid)
        {
            return response;
        }
        long userId = validationResult.JWT.Value.AccountId;
        return await _userService.DeleteTasksOfGroupAsync(userId, groupId, request.DependentEntitiesId);
    }


    /// <summary>
    /// Creates new group and adds existing tasks with given ids to it.
    /// </summary>
    /// <param name="jwt"></param>
    /// <param name="request"></param>
    /// <response code="200">Returns <code>PutResponse</code>.</response>
    /// <response code="400">If group title not validated.</response>
    /// <response code="401">If jwt is not validated.</response>
    [HttpPost("createGroupAndAddTasks")]
    public async Task<IActionResult> CreateGroupAndAddTasks(
        [FromHeader] string jwt, [FromBody] CreateGroupAndAddTasksRequest request)
    {
        var (isValid, validationResult, response) = await CheckJWT(jwt);
        if (!isValid)
        {
            return response;
        }
        long userId = validationResult.JWT.Value.AccountId;
        return await _userService.CreateGroupAndAddTasksAsync(userId, request.GroupTitle, request.TasksId);
    }


    /// <summary>
    /// Creates new task and adds existing groups with given ids to it.
    /// </summary>
    /// <param name="jwt"></param>
    /// <param name="request"></param>
    /// <response code="200">Returns <code>PutResponse</code>.</response>
    /// <response code="400">If task title or description not validated.</response>
    /// <response code="401">If jwt is not validated.</response>
    [HttpPost("createTaskAndAddGroups")]
    public async Task<IActionResult> CreateTaskAndAddGroups(
        [FromHeader] string jwt, [FromBody] CreateTaskAndAddGroupRequest request)
    {
        var (isValid, validationResult, response) = await CheckJWT(jwt);
        if (!isValid)
        {
            return response;
        }
        long userId = validationResult.JWT.Value.AccountId;
        return await _userService.CreateTaskAndAddGroupsAsync
            (userId, request.TaskTitle, request.TaskDescription, request.IsDone, request.GroupsId);
    }
}
