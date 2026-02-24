namespace Domain.Constants;

public static class Messages
{
    public const string NotFound = "Requested Account could not be found";
    public const string InsufficientBalance = "Requested amount must be less or equal to current balance";
    public const string NoNegativeAmount = "Requested amount must be positive";
    public const string InvalidName = "Requested name must follow the patter of <First> <Middle> <Last>";
    public const string InvalidSearchType = "Requested search type must be balance or name";
}