namespace Web_Api.Models.DataModels.Requests
{
    public record struct CreateTaskAndAddGroupRequest(string TaskTitle, string TaskDescription, bool IsDone, long[] GroupsId);
}
