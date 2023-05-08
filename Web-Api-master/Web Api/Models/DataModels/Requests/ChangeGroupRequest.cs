namespace Web_Api.Models.DataModels.Requests;

public record struct ChangeGroupRequest(string NewGroupTitle,long[]NewTasksId);
