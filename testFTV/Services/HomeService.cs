using Newtonsoft.Json.Linq;

namespace testFTV.Services
{
  public class HomeService
  {
    private readonly HttpClient _http;

    private readonly string API_DOMAIN_V2;

    public HomeService(HttpClient httpClient)
    {
      _http = httpClient;
    }

    private async Task<JObject?> GetJson(string url)
    {
      var res = await _http.GetStringAsync(url);
      if (string.IsNullOrEmpty(res))
        return null;

      return JObject.Parse(res);
    }
  }
