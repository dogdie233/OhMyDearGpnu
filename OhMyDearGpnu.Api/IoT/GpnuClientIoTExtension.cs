namespace OhMyDearGpnu.Api.IoT;

public static class GpnuClientIoTExtension
{
    public static IoTContext GetIoTContext(this GpnuClient client)
    {
        return client.serviceContainer.Locate<IoTContext>();
    }
}