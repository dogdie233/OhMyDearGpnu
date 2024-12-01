namespace OhMyDearGpnu.Api.AcaAff;

public static class GpnuClientAcaAffExtension
{
    public static AcaAffContext GetAcaAffContext(this GpnuClient client)
    {
        return client.serviceContainer.Locate<AcaAffContext>();
    }
}