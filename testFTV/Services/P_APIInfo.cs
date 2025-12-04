namespace testFTV.Services
{
  public class P_APIInfo
  {
    public static string btnName = "API更新";
    public static string CKName = "API更新";

    public string UrlInfo(int APICate)
    {
      string url_st = string.Empty;

      switch (APICate)
      {
        case 1:
          url_st = "https://app.4gtv.tv/Data/FtvNewsGetChannelList.ashx";
          break;
        case 2:
          url_st = "https://app.4gtv.tv/Data/FtvNewsGetChannelURL.ashx";
          break;
        case 3:
          url_st = "https://ftvstorage.blob.core.windows.net/img/";
          break;
        case 4:
          url_st = "https://newsimg.ftv.com.tw/img/";
          break;
        case 5:
          url_st = "https://www.ftvnews.com.tw/";
          break;
        case 6:
          url_st = "https://ftvback.azurewebsites.net/";
          break;
        case 7:
          url_st = "https://www.ftv.com.tw/NewNews/EngAppNews.ashx";
          break;
        case 8:
          url_st = "264991820642095";
          break;
        case 9:
          url_st = "https://ftvstorage.blob.core.windows.net/manasystem/";
          break;
        case 10:
          url_st = "https://newsimg.ftv.com.tw/manasystem/";
          break;
        case 11:
          url_st = "https://newsimg.ftv.com.tw/";
          break;
        case 12:
          url_st = "https://ftvapimanager.azure-api.net/";
          break;
        case 13:
          url_st = "https://newsimg.ftv.com.tw/manasystem/FileData/News/def.jpg";
          break;
        case 14:
          url_st = "https://ftvapi.ftvnews.com.tw/API/";
          break;
        case 15:
          url_st = "https://ftvapiv2.ftvnews.com.tw/";
          break;
      }

      return url_st;
    }
  }
}
