namespace Web_Api.Models.AuthenticationModels;

public record struct JsonWebToken(string Value, long AccountId, string AccountName, DateTimeOffset IssuedAt, DateTimeOffset ExpiresAt);
