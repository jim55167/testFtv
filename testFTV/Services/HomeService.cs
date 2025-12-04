using Newtonsoft.Json.Linq;
using testFTV.Models.Home;

namespace testFTV.Services
{
  public class HomeService
  {
    private readonly HttpClient _http;
    private readonly F_httpPost _httpPost;
    private readonly P_APIInfo _apiInfo;

    public HomeService(
        HttpClient httpClient,
        F_httpPost httpPost,
        P_APIInfo apiInfo)
    {
      _http = httpClient;
      _httpPost = httpPost;
      _apiInfo = apiInfo;
    }

    private async Task<JObject?> GetJson(string url)
    {
      var res = await _http.GetStringAsync(url);
      if (string.IsNullOrEmpty(res))
        return null;

      return JObject.Parse(res);
    }

    public Task<IEnumerable<NewsItem>> LoadTextBasedGuide()
    {
      var items = new List<NewsItem>
      {
        new() { Title = "即時焦點 1", Url = "/news/guide-1" },
        new() { Title = "即時焦點 2", Url = "/news/guide-2" },
        new() { Title = "即時焦點 3", Url = "/news/guide-3" },
      };

      return Task.FromResult<IEnumerable<NewsItem>>(items);
    }

    public async Task<IEnumerable<NewsItem>> LoadHotNewsList()
    {
      try
      {   
      string apiDomain = _apiInfo.UrlInfo(15);
      string requestUrl = $"{apiDomain}API/HotNewsList";

      string response = await _httpPost.Get_HttpStatus(
          requestUrl,
          "application/json",
          string.Empty,
          "GET",
          "200");

      if (string.IsNullOrWhiteSpace(response))
      {
        return Enumerable.Empty<NewsItem>();
      }

      var json = JObject.Parse(response);
      var data = json["Data"] ?? json["data"] ?? json["Result"];

      if (data == null || data.Type != JTokenType.Array)
      {
        return Enumerable.Empty<NewsItem>();
      }

      var items = new List<NewsItem>();

      foreach (var entry in data)
      {
        string title = GetString(entry, "fsTitle", "fsTITLE", "title");
        string url = GetString(entry, "fsUrl", "fsURL", "url", "linkUrl", "link");

        if (!string.IsNullOrWhiteSpace(title))
        {
          items.Add(new NewsItem
          {
            Title = title,
            Url = string.IsNullOrWhiteSpace(url) ? "/" : url
          });
        }
      }

      return items;
    }
      catch
      {
        return Enumerable.Empty<NewsItem>();
      }
}

private static string GetString(JToken token, params string[] propertyNames)
{
  foreach (var name in propertyNames)
  {
    var value = token?[name];
    if (value != null && value.Type != JTokenType.Null)
    {
      var str = value.ToString();
      if (!string.IsNullOrWhiteSpace(str))
      {
        return str;
      }
    }
  }

  return string.Empty;
}

    public Task<IEnumerable<NewsItem>> LoadFocusNewsList()
    {
      var items = new List<NewsItem>
      {
        new() { Title = "焦點新聞 1", Url = "/news/focus-1" },
        new() { Title = "焦點新聞 2", Url = "/news/focus-2" },
        new() { Title = "焦點新聞 3", Url = "/news/focus-3" },
      };

      return Task.FromResult<IEnumerable<NewsItem>>(items);
    }

    public Task<IEnumerable<NewsItem>> LoadHotNewList()
    {
      var items = new List<NewsItem>
      {
        new() { Title = "熱門報導 1", Url = "/news/hotnew-1" },
        new() { Title = "熱門報導 2", Url = "/news/hotnew-2" },
        new() { Title = "熱門報導 3", Url = "/news/hotnew-3" },
      };

      return Task.FromResult<IEnumerable<NewsItem>>(items);
    }