using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TESTTEST.Response
{
    public record struct JsonWebToken(string Value, long AccountId, string AccountName, DateTimeOffset IssuedAt, DateTimeOffset ExpiresAt);

}
