using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Web_Api.Models.AuthenticationModels;

[Index("Name", IsUnique = true)]
public class Account : BaseModel
{
    [Required]
    public string Name { get; set; }
    [Required]
    public string PasswdHash { get; set; }
    public List<RefreshToken> RefreshTokens { get; set; }
    public User User { get; set; }
}
