using System.Diagnostics.CodeAnalysis;

namespace OhMyDearGpnu.Api.Cas;

public class CasLoginResult
{
    private CasLoginResult(bool isSucceeded)
    {
        IsSucceeded = isSucceeded;
    }

    [MemberNotNullWhen(true, nameof(Ticket), nameof(Tgt))]
    [MemberNotNullWhen(false, nameof(ErrorMessage))]
    public bool IsSucceeded { get; init; }

    public string? Ticket { get; set; }
    public string? Tgt { get; set; }
    public string? ErrorMessage { get; set; }

    public static CasLoginResult CreateSuccess(string ticket, string tgt)
    {
        return new CasLoginResult(true)
        {
            Ticket = ticket,
            Tgt = tgt
        };
    }

    public static CasLoginResult CreateFail(string message)
    {
        return new CasLoginResult(false)
        {
            ErrorMessage = message
        };
    }
}