using Microsoft.AspNetCore.Mvc;
using Web_Api.Models;
using Web_Api.Models.DataModels;


namespace Web_Api.Services;

public class UserService : IUserService
{
    private readonly UserRepository _userRepository;
    private readonly ValidationService _validationService;

    public UserService(
        UserRepository userRepository,
        ValidationService validationService)
    {
        _userRepository = userRepository;
        _validationService = validationService;
    }

    public async Task<IActionResult> GetByIdAsync(long userId)
    {
        User user = await _userRepository.GetWithDataAsync(userId);
        if (user is null)
        {
            return new NotFoundObjectResult(new NoContentResponse { State = Models.DataModels.State.EntryWithGivenIdNotFound });
        }
        return new OkObjectResult(new GetResponse<User>(user));
    }

    public async Task<IActionResult> GetTasksAsync(long userId)
    {
        IEnumerable<ToDoTask> tasks = await _userRepository.GetTasksAsync(userId);
        if (tasks is null)
        {
            return new NotFoundObjectResult(new NoContentResponse { State = Models.DataModels.State.EntryWithGivenIdNotFound });
        }
        return new OkObjectResult(new GetResponse<IEnumerable<ToDoTask>>(tasks));
    }
    public async Task<IActionResult> AddTaskAsync(long userId, string taskTitle, string taskDescription, bool taskIsDone)
    {
        switch (_validationService.ValidateTaskData(taskTitle, taskDescription))
        {
            case { IsValid: true }:
                break;
            case { IsDescriptionValid: false, IsTitleValid: false }:
                return new BadRequestObjectResult(new PostResponse
                {
                    State = Models.DataModels.State.TitleAndDescriptionNotValidated,
                    EntryId = null
                });
            case { IsTitleValid: false }:
                return new BadRequestObjectResult(new PostResponse
                {
                    State = Models.DataModels.State.TitleNotValidated,
                    EntryId = null
                });
            case { IsDescriptionValid: false }:
                return new BadRequestObjectResult(new PostResponse
                {
                    State = Models.DataModels.State.DescriptionNotValidated,
                    EntryId = null
                });
        }
        ToDoTask task = await _userRepository.AddTaskAsync(userId, taskTitle, taskDescription, taskIsDone);
        await _userRepository.SaveAsync();
        if (task is null)
        {
            return new NotFoundObjectResult(new PostResponse
            {
                State = Models.DataModels.State.EntryWithGivenIdNotFound,
                EntryId = null
            });
        }
        return new OkObjectResult(new PostResponse
        {
            State = Models.DataModels.State.DataAdded,
            EntryId = task.Id
        });
    }
    public async Task<IActionResult> DeleteTaskAsync(long userId, long taskId)
    {
        bool isDeleted = await _userRepository.DeleteTaskAsync(userId, taskId);
        if (!isDeleted)
        {
            return new NotFoundObjectResult(new DeleteResponse
            {
                State = Models.DataModels.State.EntryWithGivenIdNotFound
            });
        }
        await _userRepository.SaveAsync();
        return new OkObjectResult(new DeleteResponse
        {
            State = Models.DataModels.State.DataDeleted
        });
    }
    public async Task<IActionResult> ChangeTaskAsync(long userId, long taskId, bool? newIsDoneStatus, string newTitle, string newDescription, long[] newGroupsId)
    {
        switch (_validationService.ValidateTaskData(newTitle, newDescription))
        {
            case { IsValid: true }:
                break;
            case { IsDescriptionValid: false, IsTitleValid: false }:
                return new BadRequestObjectResult(new PostResponse
                {
                    State = Models.DataModels.State.TitleAndDescriptionNotValidated,
                    EntryId = null
                });
            case { IsTitleValid: false }:
                return new BadRequestObjectResult(new PostResponse
                {
                    State = Models.DataModels.State.TitleNotValidated,
                    EntryId = null
                });
            case { IsDescriptionValid: false }:
                return new BadRequestObjectResult(new PostResponse
                {
                    State = Models.DataModels.State.DescriptionNotValidated,
                    EntryId = null
                });
        }
        bool isSuccessful = await _userRepository.ChangeTaskAsync(userId, taskId, newIsDoneStatus, newTitle, newDescription,newGroupsId);
        if (!isSuccessful)
        {
            return new BadRequestObjectResult(new PutResponse
            {
                State = Models.DataModels.State.EntryWithGivenIdNotFoundOrNothingToChange
            });
        }
        await _userRepository.SaveAsync();
        return new OkObjectResult(new PutResponse
        {
            State = Models.DataModels.State.DataEdited
        });
    }

    public async Task<IActionResult> GetGroupsAsync(long userId)
    {
        IEnumerable<TaskGroup> groups = await _userRepository.GetGroupsAsync(userId);
        if (groups is null)
        {
            return new NotFoundObjectResult(new NoContentResponse { State = Models.DataModels.State.EntryWithGivenIdNotFound });
        }
        return new OkObjectResult(new GetResponse<IEnumerable<TaskGroup>>(groups));
    }
    public async Task<IActionResult> AddGroupAsync(long userId, string groupTitle)
    {
        if (!_validationService.ValidateGroupData(groupTitle).IsValid)
        {
            return new BadRequestObjectResult(new PostResponse
            {
                State = Models.DataModels.State.TitleNotValidated,
                EntryId = null
            });
        }
        TaskGroup entry = await _userRepository.AddGroupAsync(userId, groupTitle);
        await _userRepository.SaveAsync();
        if (entry is null)
        {
            return new NotFoundObjectResult(new PostResponse
            {
                State = Models.DataModels.State.EntryWithGivenIdNotFound,
                EntryId = null
            });
        }
        return new OkObjectResult(new PostResponse
        {
            State = Models.DataModels.State.DataAdded,
            EntryId = entry.Id
        });
    }
    public async Task<IActionResult> DeleteGroupAsync(long userId, long groupId)
    {
        bool isDeleted = await _userRepository.DeleteGroupAsync(userId, groupId);
        if (!isDeleted)
        {
            return new NotFoundObjectResult(new DeleteResponse
            {
                State = Models.DataModels.State.EntryWithGivenIdNotFound
            });
        }
        await _userRepository.SaveAsync();
        return new OkObjectResult(new DeleteResponse
        {
            State = Models.DataModels.State.DataDeleted
        });
    }
    public async Task<IActionResult> ChangeGroupAsync(long userId, long groupId, string newTitle, long[] newTasksId)
    {
        if (newTitle is not null && !_validationService.ValidateGroupData(newTitle).IsValid)
        {
            return new BadRequestObjectResult(new PutResponse
            {
                State = Models.DataModels.State.TitleNotValidated
            });
        }
        bool isSuccessful = await _userRepository.ChangeGroupAsync(userId, groupId, newTitle, newTasksId);
        if (!isSuccessful)
        {
            return new BadRequestObjectResult(new PutResponse
            {
                State = Models.DataModels.State.EntryWithGivenIdNotFoundOrNothingToChange
            });
        }
        await _userRepository.SaveAsync();
        return new OkObjectResult(new PutResponse
        {
            State = Models.DataModels.State.DataEdited
        });
    }

    public async Task<IActionResult> AddGroupsToTaskAsync(long userId, long taskId, IEnumerable<long> groupsId)
    {
        if (groupsId is null)
        {
            return new BadRequestObjectResult(new PostResponse
            {
                State = Models.DataModels.State.UnexpectedNullValue,
                EntryId = null
            });
        }
        bool isSuccessful = await _userRepository.AddGroupsToTaskAsync(userId, taskId, groupsId);
        if (!isSuccessful)
        {
            return new BadRequestObjectResult(new PutResponse
            {
                State = Models.DataModels.State.EntryWithGivenIdNotFound
            });
        }
        await _userRepository.SaveAsync();
        return new OkObjectResult(new PutResponse
        {
            State = Models.DataModels.State.DataEdited
        });
    }
    public async Task<IActionResult> RefreshGroupsOfTaskAsync(long userId, long taskId, IEnumerable<long> groupsId)
    {
        if (groupsId is null)
        {
            return new BadRequestObjectResult(new PostResponse
            {
                State = Models.DataModels.State.UnexpectedNullValue,
                EntryId = null
            });
        }
        bool isSuccessful = await _userRepository.RefreshGroupsOfTaskAsync(userId, taskId, groupsId);
        if (!isSuccessful)
        {
            return new BadRequestObjectResult(new PutResponse
            {
                State = Models.DataModels.State.EntryWithGivenIdNotFound
            });
        }
        await _userRepository.SaveAsync();
        return new OkObjectResult(new PutResponse
        {
            State = Models.DataModels.State.DataEdited
        });
    }
    public async Task<IActionResult> AddTasksToGroupAsync(long userId, long groupId, IEnumerable<long> tasksId)
    {
        if (tasksId is null)
        {
            return new BadRequestObjectResult(new PostResponse
            {
                State = Models.DataModels.State.UnexpectedNullValue,
                EntryId = null
            });
        }
        bool isSuccessful = await _userRepository.AddTasksToGroupAsync(userId, groupId, tasksId);
        if (!isSuccessful)
        {
            return new BadRequestObjectResult(new PutResponse
            {
                State = Models.DataModels.State.EntryWithGivenIdNotFound
            });
        }
        await _userRepository.SaveAsync();
        return new OkObjectResult(new PutResponse
        {
            State = Models.DataModels.State.DataEdited
        });
    }
    public async Task<IActionResult> RefreshTasksOfGroupAsync(long userId, long groupId, IEnumerable<long> tasksId)
    {
        if (tasksId is null)
        {
            return new BadRequestObjectResult(new PostResponse
            {
                State = Models.DataModels.State.UnexpectedNullValue,
                EntryId = null
            });
        }
        bool isSuccessful = await _userRepository.RefreshTasksOfGroupAsync(userId, groupId, tasksId);
        if (!isSuccessful)
        {
            return new BadRequestObjectResult(new PutResponse
            {
                State = Models.DataModels.State.EntryWithGivenIdNotFound
            });
        }
        await _userRepository.SaveAsync();
        return new OkObjectResult(new PutResponse
        {
            State = Models.DataModels.State.DataEdited
        });
    }
    public async Task<IActionResult> DeleteTasksOfGroupAsync(long userId, long groupId, IEnumerable<long> tasksId)
    {
        if (tasksId is null)
        {
            return new BadRequestObjectResult(new PostResponse
            {
                State = Models.DataModels.State.UnexpectedNullValue,
                EntryId = null
            });
        }
        bool isSuccessful = await _userRepository.DeleteTasksOfGroupAsync(userId, groupId, tasksId);
        if (!isSuccessful)
        {
            return new BadRequestObjectResult(new PutResponse
            {
                State = Models.DataModels.State.EntryWithGivenIdNotFound
            });
        }
        await _userRepository.SaveAsync();
        return new OkObjectResult(new PutResponse
        {
            State = Models.DataModels.State.DataEdited
        });
    }
    public async Task<IActionResult> CreateGroupAndAddTasksAsync(long userId, string groupTitle, IEnumerable<long> tasksId)
    {
        if (tasksId is null)
        {
            return new BadRequestObjectResult(new PostResponse
            {
                State = Models.DataModels.State.UnexpectedNullValue,
                EntryId = null
            });
        }
        if (!_validationService.ValidateGroupData(groupTitle).IsValid)
        {
            return new BadRequestObjectResult(new PostResponse
            {
                State = Models.DataModels.State.TitleNotValidated,
                EntryId = null
            });
        }
        TaskGroup group = await _userRepository.CreateGroupAndAddTasksAsync(userId, groupTitle, tasksId);
        await _userRepository.SaveAsync();
        if (group is null)
        {
            return new BadRequestObjectResult(new PostResponse
            {
                State = Models.DataModels.State.EntryWithGivenIdNotFound,
                EntryId = null
            });
        }
        return new OkObjectResult(new PostResponse
        {
            State = Models.DataModels.State.DataAdded,
            EntryId = group.Id
        });
    }
    public async Task<IActionResult> CreateTaskAndAddGroupsAsync(
        long userId, string taskTitle, string taskDescription, bool isDone, IEnumerable<long> groupsId)
    {
        if (groupsId is null)
        {
            return new BadRequestObjectResult(new PostResponse
            {
                State = Models.DataModels.State.UnexpectedNullValue,
                EntryId = null
            });
        }
        switch (_validationService.ValidateTaskData(taskTitle, taskDescription))
        {
            case { IsValid: true }:
                break;
            case { IsDescriptionValid: false, IsTitleValid: false }:
                return new BadRequestObjectResult(new PostResponse
                {
                    State = Models.DataModels.State.TitleAndDescriptionNotValidated,
                    EntryId = null
                });
            case { IsTitleValid: false }:
                return new BadRequestObjectResult(new PostResponse
                {
                    State = Models.DataModels.State.TitleNotValidated,
                    EntryId = null
                });
            case { IsDescriptionValid: false }:
                return new BadRequestObjectResult(new PostResponse
                {
                    State = Models.DataModels.State.DescriptionNotValidated,
                    EntryId = null
                });
        }
        ToDoTask task = await _userRepository.CreateTaskAndAddGroupsAsync(userId, taskTitle, taskDescription, isDone, groupsId);
        await _userRepository.SaveAsync();
        if (task is null)
        {
            return new BadRequestObjectResult(new PostResponse
            {
                State = Models.DataModels.State.EntryWithGivenIdNotFound,
                EntryId = null
            });
        }
        return new OkObjectResult(new PostResponse
        {
            State = Models.DataModels.State.DataAdded,
            EntryId = task.Id
        });
    }
}
