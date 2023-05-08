using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TESTTEST.Response
{
    public class AuthenticationResponse
    {
        public JsonWebToken? JWT { get; set; }
        public RefreshToken RT { get; set; }
        public AuthState State { get; set; }
    }
}
