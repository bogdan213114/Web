namespace Web_Api.Models.DataModels;

public enum State : int
{
    DataAdded = 11,
    DataEdited = 12,
    DataUpdated = 13,
    DataDeleted = 14,

    EntryWithGivenIdNotFound = 21,
    EntryWithGivenIdNotFoundOrNothingToChange = 22,
    TitleNotValidated = 23,
    DescriptionNotValidated = 24,
    TitleAndDescriptionNotValidated=25,
    UnexpectedNullValue=26,

    JWT_IsMissing = 31,
    JWT_SignatureNotValidated = 32,
    JWT_Expired = 33,
    JWT_Broken = 34
}
