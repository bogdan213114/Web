using Microsoft.EntityFrameworkCore;
using System.Linq;
using Web_Api.Models;

namespace Web_Api.Services;

public class UserRepository : BaseRepository<User>
{
    public UserRepository(WebApiContext context)
        : base(context) { }
    public async Task<User> GetWithDataAsync(long userId)
    {
        return await _dbSet
            .Include(u => u.Tasks)
            .ThenInclude(t => t.Groups)
            .Include(u => u.Groups)
            .ThenInclude(g => g.Tasks)
            .FirstOrDefaultAsync(u => u.Id == userId);
    }
    public async Task<IEnumerable<ToDoTask>> GetTasksAsync(long userId)
    {
        User user = await _dbSet
            .Include(u => u.Tasks)
            .ThenInclude(t => t.Groups)
            .FirstOrDefaultAsync(u => u.Id == userId);
        if (user is null)
        {
            return null;
        }
        return user.Tasks;
    }
    public async Task<ToDoTask> AddTaskAsync(long userId, string taskTitle, string taskDescription, bool taskIsDone)
    {
        if (!await _dbSet.AnyAsync(u => u.Id == userId))
        {
            return null;
        }
        ToDoTask task = new()
        {
            UserId = userId,
            Title = taskTitle,
            Description = taskDescription,
            IsDone = taskIsDone,
            CreationDate = DateTime.Now
        };
        var result = await _db.Set<ToDoTask>().AddAsync(task);
        return result.Entity;
    }
    public async Task<bool> DeleteTaskAsync(long userId, long taskId)
    {
        ToDoTask task = await _db.Set<ToDoTask>().FirstOrDefaultAsync(t => t.Id == taskId && t.UserId == userId);
        if (task is null)
        {
            return false;
        }
        _db.Set<ToDoTask>().Remove(task);
        return true;
    }
    public async Task<bool> ChangeTaskAsync(long userId, long taskId, bool? newIsDoneStatus, string newTitle, string newDescription, long[] newGroupsId)
    {
        if (newIsDoneStatus is null && newTitle is null && newDescription is null && newGroupsId is null)
        {
            return false;
        }
        ToDoTask task = await _db.Set<ToDoTask>().FirstOrDefaultAsync(t => t.Id == taskId && t.UserId == userId);
        if (task is null)
        {
            return false;
        }
        if (newGroupsId is not null)
        {
            if (!await RefreshGroupsOfTaskAsync(userId, taskId, newGroupsId))
            {
                return false;
            }
        }
        if (newIsDoneStatus.HasValue)
        {
            task.IsDone = newIsDoneStatus.Value;
        }
        if (newTitle is not null)
        {
            task.Title = newTitle;
        }
        if (newDescription is not null)
        {
            task.Description = newDescription;
        }
        return true;
    }

    public async Task<IEnumerable<TaskGroup>> GetGroupsAsync(long userId)
    {
        User user = await _dbSet
            .Include(u => u.Groups)
            .ThenInclude(g => g.Tasks)
            .FirstOrDefaultAsync(u => u.Id == userId);
        if (user is null)
        {
            return null;
        }
        return user.Groups;
    }
    public async Task<TaskGroup> AddGroupAsync(long userId, string groupName)
    {
        if (!await _dbSet.AnyAsync(u => u.Id == userId))
        {
            return null;
        }
        TaskGroup group = new()
        {
            UserId = userId,
            Title = groupName,
            CreationDate = DateTime.Now
        };
        var result = await _db.Set<TaskGroup>().AddAsync(group);
        return result.Entity;
    }
    public async Task<bool> DeleteGroupAsync(long userId, long groupId)
    {
        TaskGroup group = await _db.Set<TaskGroup>().FirstOrDefaultAsync(g => g.Id == groupId && g.UserId == userId);
        if (group is null)
        {
            return false;
        }
        _db.Set<TaskGroup>().Remove(group);
        return true;
    }
    public async Task<bool> ChangeGroupAsync(long userId, long groupId, string newTitle, long[] newTasksId)
    {
        if (newTitle is null && newTasksId is null)
        {
            return false;
        }
        TaskGroup group = await _db.Set<TaskGroup>().FirstOrDefaultAsync(g => g.Id == groupId && g.UserId == userId);
        if (group is null)
        {
            return false;
        }
        if (newTasksId is not null)
        {
            if (!await RefreshTasksOfGroupAsync(userId, groupId, newTasksId))
            {
                return false;
            }
        }
        if (newTitle is not null)
        {
            group.Title = newTitle;
        }

        return true;
    }

    public async Task<bool> AddGroupsToTaskAsync(long userId, long taskId, IEnumerable<long> groupsId)
    {
        ToDoTask task = await _db.Set<ToDoTask>()
            .Include(t => t.Groups)
            .Include(t => t.User)
            .ThenInclude(u => u.Groups)
            .FirstOrDefaultAsync(t => t.Id == taskId && t.UserId == userId);
        if (task?.User is null)
        {
            return false;
        }
        var confirmedGroups = await Task.Run(() =>
          {
              return from g in task.User.Groups
                     from givenGroupId in groupsId
                     where g.Id == givenGroupId
                     select g;
          });
        if (confirmedGroups.Count() != groupsId.Count())
        {
            return false;
        }
        task.Groups.AddRange(confirmedGroups.Except(task.Groups));
        return true;
    }
    public async Task<bool> RefreshGroupsOfTaskAsync(long userId, long taskId, IEnumerable<long> groupsId)
    {
        ToDoTask task = await _db.Set<ToDoTask>()
            .Include(t => t.Groups)
            .Include(t => t.User)
            .ThenInclude(u => u.Groups)
            .FirstOrDefaultAsync(t => t.Id == taskId && t.UserId == userId);
        if (task?.User is null)
        {
            return false;
        }
        var confirmedGroups = await Task.Run(() =>
        {
            return from g in task.User.Groups
                   from givenGroupId in groupsId
                   where g.Id == givenGroupId
                   select g;
        });
        if (confirmedGroups.Count() != groupsId.Count())
        {
            return false;
        }
        task.Groups = confirmedGroups.ToList();
        return true;
    }
    public async Task<bool> AddTasksToGroupAsync(long userId, long groupId, IEnumerable<long> tasksId)
    {
        TaskGroup taskGroup = await _db.Set<TaskGroup>()
            .Include(g => g.Tasks)
            .Include(g => g.User)
            .ThenInclude(u => u.Tasks)
            .FirstOrDefaultAsync(g => g.UserId == userId && g.Id == groupId);
        if (taskGroup?.User == null)
        {
            return false;
        }
        var confirmedTasks = await Task.Run(() =>
        {
            return from t in taskGroup.User.Tasks
                   from givenTaskId in tasksId
                   where t.Id == givenTaskId
                   select t;
        });
        if (confirmedTasks.Count() != tasksId.Count())
        {
            return false;
        }
        taskGroup.Tasks.AddRange(confirmedTasks.Except(taskGroup.Tasks));
        return true;
    }
    public async Task<bool> RefreshTasksOfGroupAsync(long userId, long groupId, IEnumerable<long> tasksId)
    {
        TaskGroup taskGroup = await _db.Set<TaskGroup>()
            .Include(g => g.Tasks)
            .Include(g => g.User)
            .ThenInclude(u => u.Tasks)
            .FirstOrDefaultAsync(g => g.UserId == userId && g.Id == groupId);
        if (taskGroup?.User == null)
        {
            return false;
        }
        var confirmedTasks = await Task.Run(() =>
        {
            return from t in taskGroup.User.Tasks
                   from givenTaskId in tasksId
                   where t.Id == givenTaskId
                   select t;
        });
        if (confirmedTasks.Count() != tasksId.Count())
        {
            return false;
        }
        taskGroup.Tasks = confirmedTasks.ToList();
        return true;
    }
    public async Task<bool> DeleteTasksOfGroupAsync(long userId, long groupId, IEnumerable<long> tasksId)
    {
        TaskGroup taskGroup = await _db.Set<TaskGroup>()
            .Include(g => g.Tasks)
            .Include(g => g.User)
            .ThenInclude(u => u.Tasks)
            .FirstOrDefaultAsync(g => g.UserId == userId && g.Id == groupId);
        if (taskGroup?.User == null)
        {
            return false;
        }
        var confirmedTasks = await Task.Run(() =>
        {
            return from t in taskGroup.User.Tasks
                   from givenTaskId in tasksId
                   where t.Id == givenTaskId
                   select t;
        });
        if (confirmedTasks.Count() != tasksId.Count())
        {
            return false;
        }
        taskGroup.Tasks = taskGroup.Tasks.Except(confirmedTasks).ToList();
        return true;
    }
    public async Task<TaskGroup> CreateGroupAndAddTasksAsync(long userId, string groupTitle, IEnumerable<long> tasksId)
    {
        User user = await _dbSet.Include(u => u.Tasks).FirstOrDefaultAsync(u => u.Id == userId);
        if (user is null)
        {
            return null;
        }

        var confirmedTasks = await Task.Run(() =>
        {
            return from t in user.Tasks
                   from givenTaskId in tasksId
                   where t.Id == givenTaskId
                   select t;
        });
        if (confirmedTasks.Count() != tasksId.Count())
        {
            return null;
        }
        var result = await _db.Set<TaskGroup>().AddAsync(new TaskGroup
        {
            User = user,
            Tasks = confirmedTasks.ToList(),
            Title = groupTitle,
            CreationDate = DateTime.Now
        });
        return result.Entity;
    }
    public async Task<ToDoTask> CreateTaskAndAddGroupsAsync(long userId, string taskTitle, string taskDescription, bool isDone, IEnumerable<long> groupsId)
    {
        User user = await _dbSet.Include(u => u.Groups).FirstOrDefaultAsync(u => u.Id == userId);
        if (user is null)
        {
            return null;
        }

        var confirmedGroups = await Task.Run(() =>
        {
            return from g in user.Groups
                   from givenGroupId in groupsId
                   where g.Id == givenGroupId
                   select g;
        });
        if (confirmedGroups.Count() != groupsId.Count())
        {
            return null;
        }
        var result = await _db.Set<ToDoTask>().AddAsync(new ToDoTask
        {
            User = user,
            Groups = confirmedGroups.ToList(),
            Title = taskTitle,
            Description = taskDescription,
            IsDone = isDone,
            CreationDate = DateTime.Now
        });
        return result.Entity;
    }
}
