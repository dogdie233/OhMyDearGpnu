namespace OhMyDearGpnu.Api.StuAff;

public static class GpnuClientStuAffExtension
{
    public static StuAffContext GetStuAffContext(this GpnuClient client)
    {
        return client.serviceContainer.Locate<StuAffContext>();
    }
}