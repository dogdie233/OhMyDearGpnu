namespace OhMyDearGpnu.Api.Cas;

public class CasNotLoggedInException : Exception
{
    public CasNotLoggedInException() : base("CAS not logged in.")
    {
    }
}

public class CasTgtInvalidException : Exception
{
    public CasTgtInvalidException() : base("CAS TGT invalid.")
    {
    }

    public CasTgtInvalidException(string message) : base(message)
    {
    }
}

public class CasLoginFailException(CasLoginFailException.LoginFailReasonType reason, string message) : Exception(message)
{
    public enum LoginFailReasonType
    {
        Unknown,
        CodeFalse,
        PassError,
        NoUser
    }

    public LoginFailReasonType Reason { get; init; } = reason;
}