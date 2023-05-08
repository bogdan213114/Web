using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TESTTEST.Response
{
    public enum UserDataGettingState
    {
        DataAdded = 11,
        DataEdited = 12,
        DataUpdated = 13,
        DataDeleted = 14,

        EntryWithGivenIdNotFound = 21,
        EntryWithGivenIdNotFoundOrNothingToChange = 22,
        TitleNotValidated = 23,
        DescriptionNotValidated = 24,

        JWT_IsMissing = 31,
        JWT_SignatureNotValidated = 32,
        JWT_Expired = 33,
        JWT_Broken = 34
    }
}
