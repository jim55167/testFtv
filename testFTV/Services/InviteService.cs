using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using testFTV.Models.Invite;

namespace testFTV.Services
{
  public class InviteService
  {
    private readonly IHttpContextAccessor _context;
    private readonly P_APIInfo _apiInfo;
    private readonly F_httpPost _httpPost;

    public InviteService(
        IHttpContextAccessor accessor,
        P_APIInfo apiInfo,
        F_httpPost httpPost)
    {
      _context = accessor;
      _apiInfo = apiInfo;
      _httpPost = httpPost;
    }

    public async Task<InviteViewModel?> Load_GetRecommend()
    {
      try
      {
        string apiDomain = _apiInfo.UrlInfo(14);
        string sendUrl = apiDomain + "FTVNewsUserCodeList.aspx";

        string bearerToken = GetBearerToken("FTVGuid", "^Bearer\\s(.+)$");

        if (string.IsNullOrEmpty(bearerToken))
        {
          return new InviteViewModel
          {
            FsRecommendCode = "",
            FnCodeUse = ""
          };
        }

        string resultString = await _httpPost.get_RemoteDataHttpStatus(
            sendUrl,
            "application/json",
            bearerToken,
            string.Empty,
            "POST",
            "200");

        string decryption = F_httpPost.AesDecrypt(resultString, "", "");
        var json = JValue.Parse(decryption);

        string status = json["Status"]?.ToString() ?? string.Empty;

        if (!string.Equals(status, "Success", StringComparison.OrdinalIgnoreCase))
        {
          // 若 API 回傳非 success → 回傳空資料即可
          return new InviteViewModel();
        }

        return new InviteViewModel
        {
          FsRecommendCode = json["fsRecommendCode"]?.ToString() ?? "",
          FnCodeUse = json["fnCodeUse"]?.ToString() ?? ""
        };
      }
      catch
      {
        return new InviteViewModel();
      }
    }


    // === 原 GetBearerToken ===
    public string GetBearerToken(string cookieName, string pattern)
    {
      try
      {
        var req = _context.HttpContext?.Request;
        if (req == null) return "";

        if (req.Cookies.TryGetValue(cookieName, out var cookie))
        {
          var m = Regex.Match(cookie, pattern);
          if (m.Success)
            return m.Groups[1].Value;
        }
      }
      catch { }

      return "";
    }
  }
}
