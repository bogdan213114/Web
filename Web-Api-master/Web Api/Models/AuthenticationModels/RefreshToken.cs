using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web_Api.Models.AuthenticationModels
{
    [Index("Value", IsUnique = true)]
    public class RefreshToken : BaseModel
    {
        [Required]
        public string Value { get; set; }
        [JsonIgnore]
        public bool IsActive { get; set; }
        [JsonIgnore]
        public Account Account { get; set; }

        public long AccountId { get; set; }
        public DateTime ExpiresAt { get; set; }
        public DateTime IssuedAt { get; set; }
            
        [JsonIgnore]
        public long? ReplacedById { get; set; }

        [ForeignKey("ReplacedById"),JsonIgnore]
        public RefreshToken ReplacedBy { get; set; }

        public bool IsExpired()
        {
            return ExpiresAt.CompareTo(DateTime.Now) <= 0;
        }
        public void ReplaceBy(RefreshToken newRefreshToken)
        {
            ReplacedBy = newRefreshToken;
            IsActive = false;
        }
    }
}
