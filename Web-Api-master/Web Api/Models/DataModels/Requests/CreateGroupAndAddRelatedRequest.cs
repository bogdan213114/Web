namespace Web_Api.Models.DataModels.Requests
{
    public record struct CreateGroupAndAddTasksRequest(string GroupTitle, long[] TasksId);
}
