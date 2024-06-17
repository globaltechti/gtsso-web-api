namespace Globaltech.GtSso.WebApiSample
{
    public class UserInfo
    {
        public IList<string> Claims { get; set; } = new List<string>();
        public IList<string> Scopes { get; set; } = new List<string>();
        public IList<string> Roles { get; set; } = new List<string>();
    }
}
