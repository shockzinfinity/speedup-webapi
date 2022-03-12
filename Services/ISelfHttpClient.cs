namespace speedupApi.Services
{
  public interface ISelfHttpClient
  {
    Task PostIdAsync(string apiRoute, string id);
  }
}
