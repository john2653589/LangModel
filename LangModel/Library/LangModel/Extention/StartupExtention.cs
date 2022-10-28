namespace Rugal.Web.Lang
{
    /// <summary>
    /// LangModel v1.0.0
    /// From Rugal Tu
    /// </summary>
    public static class StartupExtention
    {
        public static IServiceCollection AddLangModel(this IServiceCollection Services, Action<LangModel> OnCreated = null)
        {
            Services
                .AddHttpContextAccessor()
                .AddScoped((Provider) =>
                {
                    var HttpContext = Provider
                        .GetService<IHttpContextAccessor>()
                        .HttpContext;

                    var RouteValue = HttpContext.Request.RouteValues
                        .ToDictionary(Item => Item.Key, Item => Item.Value.ToString());

                    var Cookie = HttpContext.Request.Cookies
                        .ToDictionary(Item => Item.Key, Item => Item.Value);

                    var UrlPath = HttpContext.Request.Path;

                    var LangModel = new LangModel()
                        .SetUrlPath(UrlPath)
                        .SetRoute(RouteValue)
                        .SetLangFromCookie(Cookie);

                    OnCreated?.Invoke(LangModel);
                    return LangModel;
                });

            return Services;
        }


    }
}
