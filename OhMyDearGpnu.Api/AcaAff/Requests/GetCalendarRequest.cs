using OhMyDearGpnu.Api.Common;
using OhMyDearGpnu.Api.Modules;

namespace OhMyDearGpnu.Api.AcaAff.Requests;

[Request(PayloadTypeEnum.None)]
public partial class GetCalendarRequest : BaseRequest<Calendar>
{
    public override Uri Url => new(Hosts.acaAff, "jwglxt/xtgl/index_cxAreaFive.html");
    public override HttpMethod HttpMethod => HttpMethod.Get;

    public override async ValueTask<Calendar> CreateDataResponseAsync(SimpleServiceContainer serviceContainer, HttpResponseMessage responseMessage)
    {
        var page = await PageCache.CreateFromResponseAsync(serviceContainer.Locate<GpnuClient>(), responseMessage, TimeSpan.FromDays(1), responseMessage.RequestMessage!);
        serviceContainer.Locate<PageCacheManager>().AddCache(page);

        var thead = page.Document!.QuerySelector("thead");
        var tbody = page.Document!.QuerySelector("tbody");
        if (thead == null || tbody == null || thead.ChildElementCount != 3)
            throw new UnexpectedResponseException("Valid calendar table not found");

        var titleHead = thead.Children[0];
        var monthHead = thead.Children[1];
        var weekHead = thead.Children[2];

        var weekPosition = weekHead.Children.TakeWhile(child => child.TextContent.Trim() != "1").Count();

        var month = 0;
        var span = 0;
        foreach (var child in monthHead.Children)
        {
            span += int.Parse(child.GetAttribute("colspan") ?? "1");
            if (span <= weekPosition)
                continue;
            month = int.Parse(child.TextContent.Trim()[0..^1]); // 去掉月字
            break;
        }

        var day = 0;
        foreach (var tr in tbody.Children)
        {
            var tds = tr.QuerySelectorAll("td");
            if (tds.Length < weekPosition + 2)
                continue;

            var content = tds[weekPosition].TextContent.Trim();
            if (content.Length == 0)
                continue;

            day = int.Parse(content);
        }

        if (!int.TryParse(titleHead.TextContent.Trim()[0..4], out var year))
            throw new UnexpectedResponseException("Cannot parse year");
        return new Calendar(new DateTime(year, month, day));
    }
}