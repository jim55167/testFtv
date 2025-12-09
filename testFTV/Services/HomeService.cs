using System.Net;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using testFTV.Models;

namespace testFTV.Services
{
  public class HomeService
  {
    private readonly F_httpPost _httpPost;
    private readonly string _apiDomainV2;

    public HomeService(
        F_httpPost httpPost,
        P_APIInfo apiInfo)
    {
      _httpPost = httpPost;
      _apiDomainV2 = apiInfo.UrlInfo(15);
    }

    private async Task<JObject?> GetApiJsonAsync(string relativePath, IDictionary<string, string>? query = null)
    {
      var queryString = query == null || !query.Any()
          ? string.Empty
          : $"?{BuildQueryString(query)}";

      var requestUrl = $"{_apiDomainV2}{relativePath}{queryString}";
      var response = await _httpPost.SendWithHeaders(
          requestUrl,
          "application/x-www-form-urlencoded;charset=UTF-8",
          string.Empty,
          "GET",
          new Dictionary<string, string>
          {
            ["TokenKey"] = "1QAZ0OKM2WSX9IJN3EDC8UHBftv8859",
            ["Ocp-Apim-Subscription-Key"] = "6943273d194a4d6e9852d021645ebb69",
          },
          "200");

      if (string.IsNullOrWhiteSpace(response) || response.StartsWith("HTTP StatusCode"))
      {
        return null;
      }

      try
      {
        return JObject.Parse(response);
      }
      catch
      {
        return null;
      }
    }

    private static string BuildQueryString(IDictionary<string, string> query)
    {
      return string.Join("&", query.Select(kv => $"{WebUtility.UrlEncode(kv.Key)}={WebUtility.UrlEncode(kv.Value)}"));
    }

    private static IEnumerable<JToken> GetItems(JObject json)
    {
      return json["ITEM"] is JArray arr ? arr : Enumerable.Empty<JToken>();
    }

    private static string FirstNonEmpty(JToken token, params string[] propertyNames)
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

    private async Task<IEnumerable<JToken>> LoadHotNewsRawAsync(int type, int sp)
    {
      var json = await GetApiJsonAsync(
          "API/FtvGetHotNewsWeb.aspx",
          new Dictionary<string, string>
          {
            ["Type"] = type.ToString(),
            ["Sp"] = sp.ToString(),
          });

      if (json == null || !string.Equals(json["Status"]?.ToString(), "Success", StringComparison.OrdinalIgnoreCase))
      {
        return Enumerable.Empty<JToken>();
      }

      return GetItems(json);
    }

    public async Task<IEnumerable<NewsItem>> LoadTextBasedGuide()
    {
      var json = await GetApiJsonAsync(
          "API/ArticleMenuImage.aspx",
          new Dictionary<string, string> { ["kind"] = "y" });

      if (json == null || !string.Equals(json["Status"]?.ToString(), "Success", StringComparison.OrdinalIgnoreCase))
      {
        return Enumerable.Empty<NewsItem>();
      }

      return GetItems(json)
          .Select(item => new NewsItem
          {
            Title = FirstNonEmpty(item, "Title", "title"),
            Url = FirstNonEmpty(item, "links", "link", "url"),
            ImageUrl = FirstNonEmpty(item, "image", "Image"),
          })
          .Where(i => !string.IsNullOrWhiteSpace(i.Title));
    }

    public async Task<IEnumerable<NewsItem>> LoadHotNewsList()
    {
      var json = await GetApiJsonAsync("API/HotNews.aspx");
      if (json == null || !string.Equals(json["Status"]?.ToString(), "Success", StringComparison.OrdinalIgnoreCase))
      {
        return Enumerable.Empty<NewsItem>();
      }

      return GetItems(json)
          .Select(item => new NewsItem
          {
            Title = FirstNonEmpty(item, "Title", "TitleShort", "fsTitle"),
            ShortTitle = FirstNonEmpty(item, "TitleShort"),
            Url = FirstNonEmpty(item, "links", "link", "url"),
            TimeText = FirstNonEmpty(item, "CreateDate"),
          })
          .Where(i => !string.IsNullOrWhiteSpace(i.Title));
    }

    public async Task<IEnumerable<NewsItem>> LoadFocusNewsList()
    {
      var items = await LoadHotNewsRawAsync(1, 15);

      return items
          .Select(item => new NewsItem
          {
            Title = FirstNonEmpty(item, "Title", "TitleShort"),
            ShortTitle = FirstNonEmpty(item, "TitleShort", "Title"),
            Url = $"/news/detail/{FirstNonEmpty(item, "ID")}",
            ImageUrl = FirstNonEmpty(item, "Image"),
          })
          .Where(i => !string.IsNullOrWhiteSpace(i.Title));
    }

    public async Task<IEnumerable<NewsItem>> LoadHotNewList()
    {
      var items = await LoadHotNewsRawAsync(1, 25);

      return items
          .Skip(15)
          .Take(10)
          .Select(item => new NewsItem
          {
            Title = FirstNonEmpty(item, "TitleShort", "Title"),
            ShortTitle = FirstNonEmpty(item, "TitleShort", "Title"),
            Url = $"/news/detail/{FirstNonEmpty(item, "ID")}",
          })
          .Where(i => !string.IsNullOrWhiteSpace(i.Title));
    }

    public async Task<(SectionModel D1, SectionModel D2, SectionModel D3)> LoadRealtime()
    {
      var defaultSection = new SectionModel { SectionTitle = "即時新聞" };

      var json = await GetApiJsonAsync(
          "API/realtime_List_Home.aspx",
          new Dictionary<string, string>
          {
            ["PageSize"] = "15",
            ["Page"] = "1",
          });

      if (json == null || !string.Equals(json["Status"]?.ToString(), "Success", StringComparison.OrdinalIgnoreCase))
      {
        return (defaultSection, defaultSection, defaultSection);
      }

      var allItems = GetItems(json)
          .Select(item => new NewsItem
          {
            Title = FirstNonEmpty(item, "TitleShort", "Title"),
            ShortTitle = FirstNonEmpty(item, "TitleShort", "Title"),
            Url = $"/news/detail/{FirstNonEmpty(item, "ID")}",
            ImageUrl = FirstNonEmpty(item, "Image"),
            TimeText = FirstNonEmpty(item, "CreateDate"),
          })
          .Where(i => !string.IsNullOrWhiteSpace(i.Title))
          .ToList();

      var d1Items = new List<NewsItem>();
      var d2Items = new List<NewsItem>();
      var d3Items = new List<NewsItem>();

      if (allItems.Count > 0) d1Items.Add(allItems[0]);
      if (allItems.Count > 1) d2Items.Add(allItems[1]);
      if (allItems.Count > 2) d3Items.Add(allItems[2]);

      for (int i = 3; i < allItems.Count; i++)
      {
        if (i >= 3 && i <= 6)
        {
          d1Items.Add(allItems[i]);
        }
        else if (i >= 7 && i <= 10)
        {
          d2Items.Add(allItems[i]);
        }
        else
        {
          d3Items.Add(allItems[i]);
        }
      }

      var d1 = new SectionModel
      {
        SectionTitle = d1Items.FirstOrDefault()?.Title ?? "即時新聞",
        Items = d1Items,
      };

      var d2 = new SectionModel
      {
        SectionTitle = d2Items.FirstOrDefault()?.Title ?? "即時新聞",
        Items = d2Items,
      };

      var d3 = new SectionModel
      {
        SectionTitle = d3Items.FirstOrDefault()?.Title ?? "即時新聞",
        Items = d3Items,
      };

      return (d1, d2, d3);
    }

    public async Task<IEnumerable<ImageCarouselItem>> LoadCarouselImages()
    {
      var json = await GetApiJsonAsync("API/CarouselImage.aspx");
      if (json == null || !string.Equals(json["Status"]?.ToString(), "Success", StringComparison.OrdinalIgnoreCase))
      {
        return Enumerable.Empty<ImageCarouselItem>();
      }

      return GetItems(json)
          .Select(item => new ImageCarouselItem
          {
            ImageUrl = FirstNonEmpty(item, "image", "Image"),
            LinkUrl = FirstNonEmpty(item, "links", "link", "url"),
          })
          .Where(i => !string.IsNullOrWhiteSpace(i.ImageUrl));
    }

    public async Task<IEnumerable<AnchorItem>> LoadAnchorList()
    {
      var json = await GetApiJsonAsync("API/AnchorList.aspx");
      if (json == null || !string.Equals(json["Status"]?.ToString(), "Success", StringComparison.OrdinalIgnoreCase))
      {
        return Enumerable.Empty<AnchorItem>();
      }

      return GetItems(json)
          .Select(item => new AnchorItem
          {
            Name = FirstNonEmpty(item, "Title", "Name"),
            ImageUrl = FirstNonEmpty(item, "Image", "image"),
            Url = $"/anchor/detail/{FirstNonEmpty(item, "ID")}",
          })
          .Where(a => !string.IsNullOrWhiteSpace(a.Name));
    }
  }
}