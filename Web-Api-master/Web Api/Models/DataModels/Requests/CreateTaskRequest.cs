namespace Web_Api.Models.DataModels.Requests;

public record struct CreateTaskRequest(string Title, string Description, bool IsDone);
