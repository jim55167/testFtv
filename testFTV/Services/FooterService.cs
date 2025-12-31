using Newtonsoft.Json.Linq;
using System.Diagnostics.Metrics;
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

    private Task<JObject?> GetApiJsonAsync(string relativePath, IDictionary<string, string>? query = null)
    {
      return _httpPost.GetApiJsonAsync(_apiDomain, relativePath, query);
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

    public async Task<FooterModel?> LoadIdleInfoAsync()
    {

      var json = await GetApiJsonAsync("API/ConfigInfo.aspx");
      Console.WriteLine("===AAAA===:"+ json);
      if (json == null)
      {
        return null;
      }

      var status = json["Status"]?.ToString();
      if (!string.Equals(status, "Success", StringComparison.OrdinalIgnoreCase))
      {
        return null;
      }

      
      var item = GetItems(json).FirstOrDefault();
      if (item == null) 
      {
        return null;
      };

      var model = new FooterModel
      {
        IdleContent = FirstNonEmpty(item, "Content"),
        IdleTime = FirstNonEmpty(item, "TimeMine"),
      };
      
      return string.IsNullOrWhiteSpace(model.IdleTime) ? null : model;

    }
  }
}