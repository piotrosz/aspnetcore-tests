using Microsoft.FeatureManagement.FeatureFilters;

namespace TestAppConfig.FeatureFlags
{
    public sealed class TestTargetingContextAccessor : ITargetingContextAccessor
    {
        private const string TargetingContextLookup = "TestTargetingContextAccessor.TargetingContext";
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TestTargetingContextAccessor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public ValueTask<TargetingContext> GetContextAsync()
        {
            HttpContext httpContext = _httpContextAccessor.HttpContext;

            if (httpContext == null)
            {
                throw new Exception("HttpContext is null. Cannot get feature flags TargetingContext");
            }

            if (httpContext.Items.TryGetValue(TargetingContextLookup, out object value))
            {
                return new ValueTask<TargetingContext>((TargetingContext)value);
            }
            var groups = new List<string>();

            //if (httpContext.User.Identity.Name != null)
            //{
            //    groups.Add(httpContext.User.Identity.Name.Split("@")[1]);
            //}

            groups.Add("contoso.com");


            var targetingContext = new TargetingContext
            {
                //UserId = httpContext.User.Identity.Name,
                UserId = "test@contoso.com",
                Groups = groups
            };
            httpContext.Items[TargetingContextLookup] = targetingContext;
            return new ValueTask<TargetingContext>(targetingContext);
        }
    }
}
