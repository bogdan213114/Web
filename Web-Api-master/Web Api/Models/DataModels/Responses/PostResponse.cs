namespace Web_Api.Models.DataModels;

public struct PostResponse : IDataResponse
{
    public State State { get; set; }
    public long? EntryId { get; set; }
}
