namespace OhMyDearGpnu.Api;

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