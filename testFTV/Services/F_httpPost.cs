using System.Net;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;

namespace testFTV.Services
{
  public class F_httpPost
  {
    private readonly HttpClient _http;

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
      var msg = new HttpRequestMessage(new HttpMethod(httpMethod), url);

      msg.Headers.Add("User-Agent", "Mozilla/5.0");
      msg.Headers.Add("Accept", "*/*");
      msg.Headers.Add("Accept-Encoding", "gzip,deflate");

      if (!string.IsNullOrEmpty(authorization))
      {
        msg.Headers.Add("Authorization", authorization);
      }

      msg.Headers.Add("Ocp-Apim-Subscription-Key", "6943273d194a4d6e9852d021645ebb69");

      if (httpMethod == "POST")
      {
        msg.Content = new StringContent(postParams ?? "", Encoding.UTF8, contentType);
      }

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
      var msg = new HttpRequestMessage(new HttpMethod(httpMethod), url);

      msg.Headers.Add("User-Agent", "Mozilla/5.0");
      msg.Headers.Add("Accept", "*/*");
      msg.Headers.Add("Accept-Encoding", "gzip,deflate");
      msg.Headers.Add("TokenKey", "1QAZ0OKM2WSX9IJN3EDC8UHBftv8859");

      if (httpMethod == "POST")
      {
        msg.Content = new StringContent(postParams ?? "", Encoding.UTF8, contentType);
      }

      var res = await _http.SendAsync(msg);
      string content = await res.Content.ReadAsStringAsync();

      if (((int)res.StatusCode).ToString() != expectedCode)
      {
        return $"HTTP StatusCode 不符合規則，預期: {expectedCode}，實際: {(int)res.StatusCode}";
      }

      return content;
    }
  }
}
