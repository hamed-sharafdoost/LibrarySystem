using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorPage.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
            _logger.LogDebug(1, "Nlog injected into IndexModel");
        }

        public void OnGet()
        {
            _logger.LogInformation("Hello,this is the index");
        }
    }
}