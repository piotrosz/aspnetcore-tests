using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;

namespace CustomTelemetryWebApi;

internal sealed class MyCustomTelemetryInitializer : ITelemetryInitializer
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public MyCustomTelemetryInitializer(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public void Initialize(ITelemetry telemetry)
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext == null)
        {
            return;
        }

        lock (httpContext)
        {
            if (telemetry?.Context?.GlobalProperties == null)
            {
                return;
            }

            if (httpContext.Items.TryGetValue("InstanceId", out object? instanceId))
            {

                telemetry.Context.GlobalProperties["InstanceId"] = instanceId?.ToString();
            }

            if (httpContext.Items.TryGetValue("InstanceName", out object? instanceName))
            {
                telemetry.Context.GlobalProperties["InstanceName"] = instanceName?.ToString();
            }
        }
    }
}