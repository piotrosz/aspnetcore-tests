using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.FeatureManagement;
using Microsoft.FeatureManagement.Mvc;

namespace TestAppConfig.Pages;

[FeatureGate("Beta")]
public class BetaModel : PageModel
{
    public string AllFeaturesMessage { get; set; }

    public bool IsBetaFeature { get; set; }

    public async Task OnGet([FromServices]IFeatureManager featureManager)
    {
        var featureNames = featureManager.GetFeatureNamesAsync();

        var featureNameList = new List<string>();
        await foreach (var featureName in featureNames)
        {
            featureNameList.Add(featureName);
        }

        AllFeaturesMessage = string.Join(",", featureNameList);

        IsBetaFeature = await featureManager.IsEnabledAsync("Beta");
    }
}