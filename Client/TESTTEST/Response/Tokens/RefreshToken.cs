using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TESTTEST.MVVM.Model;

namespace TESTTEST.Response
{
   
    public class RefreshToken : BaseModel
    {
    
        public string Value { get; set; }
        public bool IsActive { get; set; }
        public DateTime ExpiresAt { get; set; }
        public DateTime IssuedAt { get; set; }  

        public bool IsExpired()
        {
            return ExpiresAt.CompareTo(DateTime.Now) <= 0;
        }
      
    }
}
