using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;

namespace testFTV.Services
{
  public class F_httpPost
  {
    private readonly HttpClient _http;
    private const string SubscriptionKey = "6943273d194a4d6e9852d021645ebb69";
    private const string TokenKey = "1QAZ0OKM2WSX9IJN3EDC8UHBftv8859";

    public F_httpPost(HttpClient httpClient)
    {
      _http = httpClient;
    }

    // === AES 加密 ===
    public static string AesEncrypt(string str, string key = null, string iv = null)
    {
      key = "Mnb$VcxZ@AsdFghJkl*TryUopQwe#691";
      iv = "QazMlpXswOknTyuh";

      if (string.IsNullOrEmpty(str)) return null;

      byte[] input = Encoding.UTF8.GetBytes(str);

      using Aes aes = Aes.Create();
      aes.Key = Encoding.UTF8.GetBytes(key);
      aes.IV = Encoding.UTF8.GetBytes(iv);
      aes.Mode = CipherMode.CBC;
      aes.Padding = PaddingMode.PKCS7;

      var enc = aes.CreateEncryptor();
      byte[] result = enc.TransformFinalBlock(input, 0, input.Length);

      return Convert.ToBase64String(result);
    }

    // === AES 解密 ===
    public static string AesDecrypt(string str, string key = null, string iv = null)
    {
      key = "Mnb$VcxZ@AsdFghJkl*TryUopQwe#691";
      iv = "QazMlpXswOknTyuh";

      if (string.IsNullOrEmpty(str)) return null;

      byte[] data = Convert.FromBase64String(str);

      using Aes aes = Aes.Create();
      aes.Key = Encoding.UTF8.GetBytes(key);
      aes.IV = Encoding.UTF8.GetBytes(iv);
      aes.Mode = CipherMode.CBC;
      aes.Padding = PaddingMode.PKCS7;

      var dec = aes.CreateDecryptor();
      byte[] result = dec.TransformFinalBlock(data, 0, data.Length);

      return Encoding.UTF8.GetString(result);
    }

    // === HttpPost (原 get_RemoteDataHttpStatus) ===
    public async Task<string> get_RemoteDataHttpStatus(
        string url,
        string contentType,
        string authorization,
        string postParams,
        string httpMethod,
        string expectedCode = "200")
    {
      var msg = BuildRequest(url, contentType, authorization, postParams, httpMethod);
      msg.Headers.Add("Ocp-Apim-Subscription-Key", SubscriptionKey);

      var response = await _http.SendAsync(msg);
      string result = await response.Content.ReadAsStringAsync();

      if (((int)response.StatusCode).ToString() != expectedCode)
      {
        return ((int)response.StatusCode).ToString();
      }

      return result;
    }

    // === HttpUploadFile ===
    public async Task<string> HttpUploadFile(
        string url,
        string filePath,
        string fileParameterName,
        string contentType,
        string authorization,
        string httpMethod)
    {
      var form = new MultipartFormDataContent();
      var fileBytes = await File.ReadAllBytesAsync(filePath);
      var fileContent = new ByteArrayContent(fileBytes);
      fileContent.Headers.ContentType = new MediaTypeHeaderValue(contentType);

      form.Add(fileContent, fileParameterName, Path.GetFileName(filePath));

      var msg = new HttpRequestMessage(new HttpMethod(httpMethod), url);
      msg.Headers.Add("Authorization", authorization);
      msg.Content = form;

      var res = await _http.SendAsync(msg);
      return await res.Content.ReadAsStringAsync();
    }

    // === Get_HttpStatus ===
    public async Task<string> Get_HttpStatus(
        string url,
        string contentType,
        string postParams,
        string httpMethod,
        string expectedCode = "200")
    {
      var msg = BuildRequest(url, contentType, null, postParams, httpMethod);
      msg.Headers.Add("TokenKey", TokenKey);

      var res = await _http.SendAsync(msg);
      string content = await res.Content.ReadAsStringAsync();

      if (((int)res.StatusCode).ToString() != expectedCode)
      {
        return $"HTTP StatusCode 不符合規則，預期: {expectedCode}，實際: {(int)res.StatusCode}";
      }

      return content;
    }

    public async Task<string> SendWithHeaders(
      string url,
      string contentType,
      string postParams,
      string httpMethod,
      IDictionary<string, string> headers,
      string expectedCode = "200")
    {
      var msg = BuildRequest(url, contentType, null, postParams, httpMethod);

      foreach (var header in headers)
      {
        msg.Headers.Remove(header.Key);
        msg.Headers.Add(header.Key, header.Value);
      }

      var res = await _http.SendAsync(msg);
      string content = await res.Content.ReadAsStringAsync();

      if (((int)res.StatusCode).ToString() != expectedCode)
      {
        return $"HTTP StatusCode 不符合規則，預期: {expectedCode}，實際: {(int)res.StatusCode}";
      }

      return content;
    }

    public async Task<JObject?> GetApiJsonAsync(
    string baseUrl,
    string relativePath,
    IDictionary<string, string>? query = null,
    string httpMethod = "GET",
    string contentType = "application/x-www-form-urlencoded;charset=UTF-8",
    string postParams = "",
    string expectedCode = "200")
    {
      // 組 QueryString
      var queryString = query == null || !query.Any()
          ? string.Empty
          : "?" + string.Join("&", query.Select(kv =>
              $"{WebUtility.UrlEncode(kv.Key)}={WebUtility.UrlEncode(kv.Value)}"));

      // 組完整 URL（避免重複或缺少斜線）
      var url = $"{baseUrl.TrimEnd('/')}/{relativePath.TrimStart('/')}{queryString}";

      // 統一帶上 TokenKey + SubscriptionKey
      var headers = new Dictionary<string, string>
      {
        ["TokenKey"] = TokenKey,
        ["Ocp-Apim-Subscription-Key"] = SubscriptionKey,
      };

      var response = await SendWithHeaders(
          url,
          contentType,
          postParams,
          httpMethod,
          headers,
          expectedCode);

      if (string.IsNullOrWhiteSpace(response) || response.StartsWith("HTTP StatusCode"))
      {
        return null;
      }

      try
      {
        return JObject.Parse(response);
      }
      catch (Exception ex)
      {
        // 之後可以改成你的正式 logging
        Console.WriteLine("JSON parse error: " + ex.Message);
        Console.WriteLine("Response head: " + response.Substring(0, Math.Min(response.Length, 200)));
        return null;
      }
    };


    private static HttpRequestMessage BuildRequest(
        string url,
        string contentType,
        string authorization,
        string postParams,
        string httpMethod)
    {
      var msg = new HttpRequestMessage(new HttpMethod(httpMethod), url);

      msg.Headers.Add("User-Agent", "Mozilla/5.0");
      msg.Headers.Add("Accept", "*/*");

      if (!string.IsNullOrEmpty(authorization))
      {
        msg.Headers.Add("Authorization", authorization);
      }

      if (httpMethod == "POST")
      {
        msg.Content = new StringContent(postParams ?? "", Encoding.UTF8, contentType);
      }

      return msg;
    }
  }
}
