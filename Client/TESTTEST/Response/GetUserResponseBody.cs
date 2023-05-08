using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TESTTEST.MVVM.Model;

namespace TESTTEST.Response
{
    public class GetUserResponseBody
    {
      public  UserDataGettingState State { get; set; }

      public  User data { get; set; }
    }
}
