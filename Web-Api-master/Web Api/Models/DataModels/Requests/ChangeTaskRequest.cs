namespace Web_Api.Models.DataModels.Requests;

public record struct ChangeTaskRequest(string NewTitle, string NewDescription, bool? NewIsDone, long[] NewGroupsId);
