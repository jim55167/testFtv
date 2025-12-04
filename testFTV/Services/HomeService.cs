using Newtonsoft.Json.Linq;
using testFTV.Models.Home;

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

    public Task<IEnumerable<NewsItem>> LoadHotNewsList()
    {
      var items = new List<NewsItem>
      {
        new() { Title = "熱門新聞 1", Url = "/news/hot-1" },
        new() { Title = "熱門新聞 2", Url = "/news/hot-2" },
        new() { Title = "熱門新聞 3", Url = "/news/hot-3" },
      };

      return Task.FromResult<IEnumerable<NewsItem>>(items);
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

    public Task<IEnumerable<ImageCarouselItem>> LoadCarouselImages()
    {
      var items = new List<ImageCarouselItem>
      {
        new() { ImageUrl = "/images/carousel-1.jpg", LinkUrl = "/news/carousel-1" },
        new() { ImageUrl = "/images/carousel-2.jpg", LinkUrl = "/news/carousel-2" },
        new() { ImageUrl = "/images/carousel-3.jpg", LinkUrl = "/news/carousel-3" },
      };

      return Task.FromResult<IEnumerable<ImageCarouselItem>>(items);
    }

    public Task<IEnumerable<AnchorItem>> LoadAnchorList()
    {
      var items = new List<AnchorItem>
      {
        new() { Name = "主播甲", Url = "/anchor/a", ImageUrl = "/images/anchor-a.jpg" },
        new() { Name = "主播乙", Url = "/anchor/b", ImageUrl = "/images/anchor-b.jpg" },
        new() { Name = "主播丙", Url = "/anchor/c", ImageUrl = "/images/anchor-c.jpg" },
      };

      return Task.FromResult<IEnumerable<AnchorItem>>(items);
    }

    public Task<(SectionModel D1, SectionModel D2, SectionModel D3)> LoadRealtime()
    {
      var d1 = new SectionModel
      {
        SectionTitle = "即時 - 1",
        Items = new List<NewsItem>
        {
          new() { Title = "即時 1-1", Url = "/news/realtime-1-1" },
          new() { Title = "即時 1-2", Url = "/news/realtime-1-2" },
        }
      };

      var d2 = new SectionModel
      {
        SectionTitle = "即時 - 2",
        Items = new List<NewsItem>
        {
          new() { Title = "即時 2-1", Url = "/news/realtime-2-1" },
          new() { Title = "即時 2-2", Url = "/news/realtime-2-2" },
        }
      };

      var d3 = new SectionModel
      {
        SectionTitle = "即時 - 3",
        Items = new List<NewsItem>
        {
          new() { Title = "即時 3-1", Url = "/news/realtime-3-1" },
          new() { Title = "即時 3-2", Url = "/news/realtime-3-2" },
        }
      };

      return Task.FromResult((d1, d2, d3));
    }
  }
}