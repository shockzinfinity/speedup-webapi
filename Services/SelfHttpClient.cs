namespace speedupApi.Services
{
  public class SelfHttpClient : ISelfHttpClient
  {
    private readonly HttpClient _client;

    public SelfHttpClient(HttpClient client, IHttpContextAccessor httpContextAccessor)
    {
      string baseAddress = string.Format("{0}://{1}/api/", httpContextAccessor.HttpContext.Request.Scheme, httpContextAccessor.HttpContext.Request.Host);
      _client = client ?? throw new ArgumentNullException(nameof(client));
      _client.BaseAddress = new Uri(baseAddress);
    }

    public async Task PostIdAsync(string apiRoute, string id)
    {
      try
      {
        var result = await _client.PostAsync(string.Format("{0}/{1}", apiRoute, id), null).ConfigureAwait(false);
      }
      catch (Exception ex)
      {
        // ignore error
      }
    }
  }
}
