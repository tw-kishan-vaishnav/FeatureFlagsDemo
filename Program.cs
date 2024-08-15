using Microsoft.FeatureManagement;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddLogging(configure => configure.AddConsole());


builder.Configuration.AddAzureAppConfiguration(options =>
{
    options.Connect(builder.Configuration.GetConnectionString("AppConfig"));
    options.UseFeatureFlags();
});
// Add Azure App Configuration middleware to the container of services.
builder.Services.AddAzureAppConfiguration();
// Add feature management to the container of services.
builder.Services.AddFeatureManagement();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/feature-flag/Feature1", async (IFeatureManager featureManager) =>
{
    var feature1 = await featureManager.IsEnabledAsync("Feature1");

    return new
    {
        FeatureFlag = $"Feature 1 is '{feature1}'"
    };
})
.WithName("FeatureFlagDemo")
.WithOpenApi();

app.Run();