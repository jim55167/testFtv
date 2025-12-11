using Newtonsoft.Json.Linq;
using System.Text;
using testFTV.Models;

namespace testFTV.Services
{
  public class MenuService
  {
    private readonly F_httpPost _httpPost;
    private readonly string _apiDomain;
    private readonly string _apiDomainV2;

    public MenuService(F_httpPost httpPost, P_APIInfo apiInfo)
    {
      _httpPost = httpPost;
      _apiDomain = apiInfo.UrlInfo(12);
      _apiDomainV2 = apiInfo.UrlInfo(15);
    }

    public async Task<MenuModel> LoadAsync()
    {
      var model = new MenuModel
      {
        ArticleMenuHtml = await LoadArticleMenuAsync(),
        SearchKeyConfigHtml = await LoadSearchKeyConfigAsync(),
        CommunityHtml = await LoadCommunityAsync()
      };

      return model;
    }

    private async Task<string> LoadArticleMenuAsync()
    {
      var sendUrl = $"{_apiDomainV2}API/ArticleMenuListHtml.aspx";
      var resultString = await _httpPost.Get_HttpStatus(
          sendUrl,
          "application/x-www-form-urlencoded; charset=UTF-8",
          string.Empty,
          "GET",
          "200");

      if (string.IsNullOrWhiteSpace(resultString) || resultString.Equals("empty", StringComparison.OrdinalIgnoreCase))
      {
        return string.Empty;
      }

      return resultString;
    }

    private async Task<string> LoadSearchKeyConfigAsync()
    {
      var sendUrl = $"{_apiDomain}API/SearchKeyConfig.aspx";
      var resultString = await _httpPost.get_RemoteDataHttpStatus(
          sendUrl,
          "application/x-www-form-urlencoded; charset=UTF-8",
          string.Empty,
          string.Empty,
          "GET",
          "200");

      if (string.IsNullOrWhiteSpace(resultString))
      {
        return string.Empty;
      }

      try
      {
        var json = JObject.Parse(resultString);
        if (!string.Equals(json["Status"]?.ToString(), "Success", StringComparison.OrdinalIgnoreCase))
        {
          return string.Empty;
        }

        var items = json["ITEM"] as JArray;
        if (items == null || items.Count == 0)
        {
          return string.Empty;
        }

        var builder = new StringBuilder();
        foreach (var item in items)
        {
          var name = item?["name"]?.ToString() ?? string.Empty;
          if (string.IsNullOrWhiteSpace(name))
          {
            continue;
          }

          var encoded = name.Replace("&", "＆").Replace("+", "＋");
          builder.Append($"<li><a href='/search/{encoded}'>{name}</a></li>");
        }

        return builder.ToString();
      }
      catch
      {
        return string.Empty;
      }
    }

    private async Task<string> LoadCommunityAsync()
    {
      var sendUrl = $"{_apiDomain}API/Community.aspx";
      var resultString = await _httpPost.get_RemoteDataHttpStatus(
          sendUrl,
          "application/x-www-form-urlencoded; charset=UTF-8",
          string.Empty,
          string.Empty,
          "GET",
          "200");

      if (string.IsNullOrWhiteSpace(resultString))
      {
        return string.Empty;
      }

      try
      {
        var json = JObject.Parse(resultString);
        if (!string.Equals(json["Status"]?.ToString(), "Success", StringComparison.OrdinalIgnoreCase))
        {
          return string.Empty;
        }

        var items = json["ITEM"] as JArray;
        if (items == null || items.Count == 0)
        {
          return string.Empty;
        }

        var builder = new StringBuilder();
        foreach (var item in items)
        {
          var title = item?["Title"]?.ToString() ?? string.Empty;
          var url = item?["Url"]?.ToString();
          var image = item?["Image"]?.ToString() ?? string.Empty;

          if (string.IsNullOrWhiteSpace(url))
          {
            url = "#";
          }

          builder.Append("<li class='brand brand-s'>");
          builder.Append($"   <a href='{url}' target='_blank'>");
          builder.Append($"     <img src='{image}' alt='{title}' />");
          builder.Append("   </a> ");
          builder.Append("</li>");
        }

        return builder.ToString();
      }
      catch
      {
        return string.Empty;
      }
    }
  }
}