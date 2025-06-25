namespace OhMyDearGpnu.Api.TeachEval;

public static class GpnuClientTeachEvalExtension
{
    public static TeachEvalContext GetTeachEvalContext(this GpnuClient client)
    {
        return client.serviceContainer.Locate<TeachEvalContext>();
    }
}