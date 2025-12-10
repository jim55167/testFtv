using Newtonsoft.Json.Linq;
using testFTV.Models;

namespace testFTV.Services
{
  public class FooterService
  {
    private readonly F_httpPost _httpPost;
    private readonly string _apiDomain;

    public FooterService(F_httpPost httpPost, P_APIInfo apiInfo)
    {
      _httpPost = httpPost;
      _apiDomain = apiInfo.UrlInfo(12);
    }

    public async Task<FooterModel> LoadIdleInfoAsync()
    {
      var model = new FooterModel();
      var requestUrl = $"{_apiDomain}API/ConfigInfo.aspx";

      var resultString = await _httpPost.get_RemoteDataHttpStatus(
          requestUrl,
          "application/x-www-form-urlencoded;charset=UTF-8",
          string.Empty,
          string.Empty,
          "GET",
          "200");

      if (string.IsNullOrWhiteSpace(resultString))
      {
        return model;
      }

      try
      {
        var json = JObject.Parse(resultString);
        if (!string.Equals(json["Status"]?.ToString(), "Success", StringComparison.OrdinalIgnoreCase))
        {
          return model;
        }

        var items = json["ITEM"] as JArray;
        if (items == null || items.Count == 0)
        {
          return model;
        }

        var item = items.LastOrDefault();
        if (item == null)
        {
          return model;
        }

        var content = item["Content"]?.ToString() ?? string.Empty;
        content = content.Replace("\r", string.Empty).Replace("\n", "<br/>");

        model.IdleContent = content;
        model.IdleTime = item["TimeMine"]?.ToString() ?? string.Empty;
      }
      catch
      {
        return model;
      }

      return model;
    }
  }
}