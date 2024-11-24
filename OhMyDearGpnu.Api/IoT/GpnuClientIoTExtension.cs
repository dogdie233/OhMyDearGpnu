using OhMyDearGpnu.Api.IoT;

namespace OhMyDearGpnu.Api.Common;

public static class GpnuClientIoTExtension
{
    public static IoTContext GetIoTContext(this GpnuClient client)
    {
        return client.serviceContainer.Locate<IoTContext>();
    }
}