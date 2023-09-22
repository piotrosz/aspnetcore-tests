using Microsoft.Extensions.Configuration;
using Microsoft.FeatureManagement;
using Microsoft.FeatureManagement.FeatureFilters;
using TestAppConfig;
using TestAppConfig.FeatureFlags;
using Azure.Identity;

var builder = WebApplication.CreateBuilder(args);

//var keyVaultEndpoint = new Uri(Environment.GetEnvironmentVariable("VaultUri"));
//builder.Configuration.AddAzureKeyVault(keyVaultEndpoint, new DefaultAzureCredential());

// Add Azure App Config
string connectionString = builder.Configuration.GetConnectionString("AppConfig");

builder.Configuration.AddAzureAppConfiguration(options =>
{
    options.Connect(connectionString)
        // Load all keys that start with `TestApp:` and have no label
        .Select("TestApp:*")
        // Configure to reload configuration if the registered sentinel key is modified
        .ConfigureRefresh(refreshOptions =>
            refreshOptions.Register("TestApp:Settings:Sentinel", refreshAll: true))
        .UseFeatureFlags(featureFlagOptions => { featureFlagOptions.CacheExpirationInterval = TimeSpan.FromMinutes(1); });
});

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddHttpContextAccessor();

builder.Services.AddFeatureManagement()
    .AddFeatureFilter<TargetingFilter>();

builder.Services.AddSingleton<ITargetingContextAccessor, TestTargetingContextAccessor>();

// Bind configuration "TestApp:Settings" section to the Settings object
builder.Services.Configure<Settings>(builder.Configuration.GetSection("TestApp:Settings"));

// Add Azure App Configuration middleware to the container of services.
builder.Services.AddAzureAppConfiguration();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Use Azure App Configuration middleware for dynamic configuration refresh.
app.UseAzureAppConfiguration();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
