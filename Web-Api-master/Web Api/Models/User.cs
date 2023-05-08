using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using Web_Api.Models.AuthenticationModels;

namespace Web_Api.Models;

public class User : BaseModel
{
    [Required]
    public string Name { get; set; }
    public List<ToDoTask> Tasks { get; set; }
    public List<TaskGroup> Groups { get; set; }

    [JsonIgnore]
    public Account Account { get; set; }

    [JsonIgnore]
    public long AccountId { get; set; }

    public DateTime RegisteredAt { get; set; }
}
