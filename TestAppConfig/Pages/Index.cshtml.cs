using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;

namespace TestAppConfig.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    public Settings Settings { get; }

    public IndexModel(
        ILogger<IndexModel> logger,
        IOptionsSnapshot<Settings> options)
    {
        Settings = options.Value;
        _logger = logger;
    }

    public void OnGet()
    {

    }
}
