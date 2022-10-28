namespace Rugal.Web.Lang
{
    /// <summary>
    /// LangModel v1.0.0
    /// From Rugal Tu
    /// </summary>
    public partial class LangModel
    {
        public event Action<LangModel> OnCreated;
        public string Language { get; set; }
        public string UrlPath { get; set; }
        public bool IsRandomLoad { get; set; } = true;
        public bool IsHasLanguage { get; set; } = false;
        public bool IsHasPath => !string.IsNullOrWhiteSpace(UrlPath);
        public string DefaultLanguage { get; set; } = "en-us";
        public string BaseJsFileName { get; set; } = "Lang";
        public string LangKey { get; set; } = ".AspNetCore.Culture";
        public List<string> RouteKey { get; set; }
        public Dictionary<string, string> UrlRotue { get; set; } = new Dictionary<string, string> { };
        public int RouteSkipCount { get; set; } = 0;
        public int RouteTakeCount { get; set; } = 2;
        public RouteDirectionType RouteDirection { get; set; } = RouteDirectionType.FromLeft;

        public virtual string GetLangJsPath(string BaseJsPath = null)
        {
            var GetUrlRoute = UrlRotue.Select(Item => Item.Value).ToList();
            if (RouteKey is not null)
            {
                GetUrlRoute = new List<string> { };
                RouteKey.ForEach((Item) =>
                {
                    if (UrlRotue.TryGetValue(Item, out var FindRoute))
                        GetUrlRoute.Add(FindRoute);
                });
            }

            var RoutePath = string.Join(@"/", RouteDirection switch
            {
                RouteDirectionType.FromLeft => GetUrlRoute.Skip(RouteSkipCount).Take(RouteTakeCount),
                RouteDirectionType.FromRight => GetUrlRoute.TakeLast(RouteTakeCount).Skip(RouteSkipCount)
            });

            var JoinPath = new List<string>
            {
                RoutePath,
                GetLangJsFileName()
            };

            return CreateJsFile(BaseJsPath, JoinPath);
        }
        public virtual string GetSharedJsPath(string BaseJsPath = null)
        {
            var JoinPath = new List<string>
            {
                GetLangJsFileName()
            };

            return CreateJsFile(BaseJsPath, JoinPath);
        }

        public virtual LangModel SetRouteCount(int _RouteSkipCount = 0, int _RouteTakeCount = 0)
        {
            if (_RouteSkipCount > 0)
                RouteSkipCount = _RouteSkipCount;

            if (_RouteTakeCount > 0)
                RouteTakeCount = _RouteTakeCount;

            return this;
        }
        public virtual LangModel SetRouteKey(params string[] Params)
        {
            RouteKey = Params.ToList();
            return this;
        }
        public virtual LangModel SetRoute(Dictionary<string, string> _UrlRotue)
        {
            UrlRotue = _UrlRotue;
            return this;
        }
        public virtual LangModel SetLangFromCookie(Dictionary<string, string> CookieDic)
        {
            if (CookieDic.TryGetValue(LangKey, out var SetLang))
            {
                IsHasLanguage = true;
                Language = SetLang.ToLower();
            }
            return this;
        }
        public virtual LangModel SetIsRandomLoad(bool _IsRandomLoad)
        {
            IsRandomLoad = _IsRandomLoad;
            return this;
        }
        public virtual LangModel SetUrlPath(string _UrlPath)
        {
            UrlPath = _UrlPath.ToLower().TrimStart('/').TrimEnd('/');
            return this;
        }
        public virtual LangModel SetLanguage(string _Language)
        {
            Language = _Language;
            return this;
        }
        internal virtual string GetLangJsFileName()
        {
            var SetLanguage = IsHasLanguage ? Language : DefaultLanguage;
            var JsFile = $"{BaseJsFileName}.{SetLanguage}.js";

            if (IsRandomLoad)
                JsFile += $"?id={Guid.NewGuid()}";

            return JsFile;
        }
        internal virtual string CreateJsFile(string BaseJsPath, List<string> JoinPath)
        {
            if (BaseJsPath is not null)
                JoinPath.Insert(0, BaseJsPath.TrimEnd('/'));

            var FullJsFile = string.Join("/", JoinPath);
            return FullJsFile;
        }
    }
    public enum RouteDirectionType
    {
        FromLeft,
        FromRight,
    }
}