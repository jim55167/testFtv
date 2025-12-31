using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;
using testFTV.Models;

namespace testFTV.Services
{
  public class TagListService
  {
    private readonly F_httpPost _httpPost;
    private readonly string _apiDomainV2;

    public TagListService(F_httpPost httpPost, P_APIInfo apiInfo)
    {
      _httpPost = httpPost;
      _apiDomainV2 = apiInfo.UrlInfo(15);
    }

    private Task<JObject?> GetApiJsonAsync(string relativePath, IDictionary<string, string>? query = null)
    {
      return _httpPost.GetApiJsonAsync(_apiDomainV2, relativePath, query);
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

    private static Pagination SetPageLink(int page, int pageTotal, int maxViewPage = 5)
    {
      if (pageTotal <= 0) return new Pagination { CurrentPage = page, TotalPages = 0 };

      int half = maxViewPage / 2;

      int startPage;
      if (page <= half) startPage = 1;
      else if (page > pageTotal - half) startPage = Math.Max(1, pageTotal - maxViewPage + 1);
      else startPage = page - half;

      int endPage = Math.Min(pageTotal, startPage + maxViewPage - 1);

      var pages = Enumerable.Range(startPage, endPage - startPage + 1).ToList();

      return new Pagination
      {
        CurrentPage = page,
        TotalPages = pageTotal,
        Pages = pages
      };
    }

    private static string BuildItemListJsonLd(IReadOnlyList<NewsWebN> items)
    {
      if (items.Count == 0)
      {
        return string.Empty;
      }

      var arr = new JArray();
      for (int i = 0; i < items.Count; i++)
      {
        var n = items[i];
        arr.Add(new JObject
        {
          ["@type"] = "ListItem",
          ["position"] = i + 1,
          ["name"] = n.Title,
          ["url"] = $"/news/detail/{n.ID}",
          ["image"] = n.ImageUrl
        });
      }

      var obj = new JObject
      {
        ["@context"] = "https://schema.org",
        ["@type"] = "ItemList",
        ["itemListElement"] = arr
      };

      return JsonConvert.SerializeObject(obj, Formatting.Indented);
    }

    public async Task<TagList?> LoadFtvGetNewsWeb(string tagId, int page = 1, int sp = 30)
    {
      if (string.IsNullOrWhiteSpace(tagId))
      {
        return null;
      }

      if (page < 1)
      {
        page = 1;
      }

      var json = await GetApiJsonAsync(
          "API/FtvGetNewsWebN.aspx",
          new Dictionary<string, string>
          {
            ["Cate"] = tagId,
            ["Page"] = page.ToString(),
            ["Sp"] = sp.ToString()
          });

      if (json == null)
      {
        return null;
      }

      var status = json["Status"]?.ToString();
      if (!string.Equals(status, "Success", StringComparison.OrdinalIgnoreCase))
      {
        return null;
      }

      int pageTotal = 0;
      int.TryParse(json["PageTotal"]?.ToString(), out pageTotal);

      if (pageTotal > 0 && page > pageTotal)
      {
        page = pageTotal;
      }

      var items = GetItems(json)
        .Select(item => new NewsWebN
        {
          ID = FirstNonEmpty(item, "ID"),
          Title = FirstNonEmpty(item, "Title"),
          ImageUrl = FirstNonEmpty(item, "Image"),
          Content = FirstNonEmpty(item, "content"),
          CreateDate = FirstNonEmpty(item, "CreateDate"),
        })
        .Where(n => !string.IsNullOrWhiteSpace(n.ID))
        .ToList();

      if (items.Count == 0)
      {
        return null;
      }
      var metaTitleCsv = string.Join(",", items.Take(6).Select(x => x.Title));
      var pageTitle = $"{tagId} - 第{page}頁 - 相關新聞 - 民視新聞網";

      return new TagList
      {
        TagId = tagId,
        TagTitle = string.Empty,
        Page = page,
        PageTotal = pageTotal,
        PageTitle = pageTitle,
        MetaTitleCsv = metaTitleCsv,
        NewsListJsonLd = BuildItemListJsonLd(items),
        FtvGetNewsWeb = items,
        SetPageLink = SetPageLink(page, pageTotal)
      };
    }

    public async Task<IEnumerable<HashTags>> LoadHashTagsInfo(string tagId)
    {
      var json = await GetApiJsonAsync(
          "API/API_HashTagsInfo.aspx",
          new Dictionary<string, string>
          {
            ["Cate"] = tagId
          });

      if (json == null)
      {
        return [];
      }

      var status = json["Status"]?.ToString();

      if (!string.Equals(status, "Success", StringComparison.OrdinalIgnoreCase))
      {
        return [];
      }
      return GetItems(json)
          .Select(item => new HashTags
          {
            Title = FirstNonEmpty(item, "Title"),
            ImageUrl = FirstNonEmpty(item, "Image"),
            Description = FirstNonEmpty(item, "description"),
            CssContent = FirstNonEmpty(item, "CssContent")
          })
          .Where(a => !string.IsNullOrWhiteSpace(a.Title))
          .ToList();

    }
  }
}
