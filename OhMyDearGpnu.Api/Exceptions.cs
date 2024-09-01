namespace OhMyDearGpnu.Api;

public class CasNotLoggedInException : Exception
{
    public CasNotLoggedInException() : base("CAS not logged in.")
    {
    }
}