namespace PizzaStar.Data.Helpers
{
    public static class UrlExtensions
    {
        public static string PathAndQuery(this HttpRequest request)
        {
            return request.QueryString.HasValue ? $"{request.Path}{request.QueryString}" : request.Path.ToString();
        }
    }
}
