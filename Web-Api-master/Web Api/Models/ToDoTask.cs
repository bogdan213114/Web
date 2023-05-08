using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Web_Api.Models;

public class ToDoTask : BaseModel
{
    [JsonIgnore]
    public long? UserId { get; set; }
    [JsonIgnore]
    public User User { get; set; }
    [Required]
    public string Title { get; set; }
    [Required]
    public string Description { get; set; }
    public bool IsDone { get; set; }
    public List<TaskGroup> Groups { get; set; }
    public DateTime CreationDate { get; set; }
}
