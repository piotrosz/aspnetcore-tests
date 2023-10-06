using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;

    private TelemetryClient _telemetryClient;

    private static HttpClient _httpClient = new HttpClient();

    public Worker(ILogger<Worker> logger, TelemetryClient telemetryClient)
    {
        _logger = logger;
        _telemetryClient = telemetryClient;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

            using (_telemetryClient.StartOperation<RequestTelemetry>("callbing-operation"))
            {
                _logger.LogWarning("A sample warning message. By default, logs with severity Warning or higher is captured by Application Insights");
                _logger.LogInformation("Calling bing.com");
                var res = await _httpClient.GetAsync("https://bing.com");
                _logger.LogInformation("Calling bing completed with status: {statusCode}", res.StatusCode);
                _telemetryClient.TrackEvent("Bing call event completed");
            }

            await Task.Delay(1000, stoppingToken);
        }
    }
}