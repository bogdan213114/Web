using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Web_Api.Models;

public class TaskGroup : BaseModel
{
    [JsonIgnore]
    public long? UserId { get; set; }
    [JsonIgnore]
    public User User { get; set; }

    [Required]
    public string Title { get; set; }

    public List<ToDoTask> Tasks { get; set; }
    public DateTime CreationDate { get; set; }
}
