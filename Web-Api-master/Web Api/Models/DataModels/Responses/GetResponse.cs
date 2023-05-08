namespace Web_Api.Models.DataModels;

public struct GetResponse<T> : IDataResponse
{
    public State State { get; set; }
    public T Data { get; set; }


    public GetResponse(T data)
    {
        Data = data;
        State = State.DataAdded;
    }
}
