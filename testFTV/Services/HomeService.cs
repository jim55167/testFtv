using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using testFTV.Models;

namespace testFTV.Services
{
  public class HomeService
  {
    private readonly F_httpPost _httpPost;
    private readonly string _apiDomainV2;

    public HomeService( F_httpPost httpPost, P_APIInfo apiInfo)
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

    private async Task<IEnumerable<JToken>> LoadHotNewsRawAsync(int type, int sp)
    {
      var json = await GetApiJsonAsync(
          "API/FtvGetHotNewsWeb.aspx",
          new Dictionary<string, string>
          {
            ["Type"] = type.ToString(),
            ["Sp"] = sp.ToString(),
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

      return GetItems(json);
    }

    public async Task<IEnumerable<NewsItem>> LoadTextBasedGuide()
    {
      var json = await GetApiJsonAsync(
          "API/ArticleMenuImage.aspx",
          new Dictionary<string, string>
          {
            ["kind"] = "y"
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
          .Select(item => new NewsItem
          {
            Title = FirstNonEmpty(item, "title"),
            Links = FirstNonEmpty(item, "links"),
          })
          .Where(i => !string.IsNullOrWhiteSpace(i.Title));
    }

    public async Task<IEnumerable<NewsItem>> LoadHotNewsList()
    {
      var json = await GetApiJsonAsync("API/HotNews.aspx");

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
        .Select(item =>
        {
          var cate = FirstNonEmpty(item, "Cate");   
          var createDateRaw = FirstNonEmpty(item, "CreateDate");

          var timeHHmm = "";

          if (!string.IsNullOrWhiteSpace(createDateRaw) &&
                DateTime.TryParse(
                    createDateRaw,
                    CultureInfo.GetCultureInfo("zh-TW"),
                    DateTimeStyles.None,
                    out var dt))
          {
            timeHHmm = dt.ToString("HH:mm", CultureInfo.InvariantCulture);
          }

          if (cate == "2") 
          {
            timeHHmm = "";
          }
          
          return new NewsItem
          {
            Cate = cate,
            Title = FirstNonEmpty(item, "Title"),
            ShortTitle = FirstNonEmpty(item, "TitleShort"),
            Links = FirstNonEmpty(item, "links"),
            CreateDate = createDateRaw,
            CreateTime = timeHHmm
          };
        })
        .Where(i => !string.IsNullOrWhiteSpace(i.Title));
    }

    public async Task<IEnumerable<NewsItem>> LoadFocusNewsList()
    {
      var items = await LoadHotNewsRawAsync(1, 15);

      return items
          .Select(item => new NewsItem
          {
            ID = FirstNonEmpty(item, "ID"),
            Title = FirstNonEmpty(item, "Title"),
            ShortTitle = FirstNonEmpty(item, "TitleShort"),
            ImageUrl = FirstNonEmpty(item, "Image"),
          })
          .Where(i => !string.IsNullOrWhiteSpace(i.ID));
    }

    public async Task<IEnumerable<NewsItem>> LoadHotNewList()
    {
      var items = await LoadHotNewsRawAsync(1, 25);

      return items
          .Skip(15)
          .Take(10)
          .Select(item => new NewsItem
          {
            ID = FirstNonEmpty(item, "ID"),
            Title = FirstNonEmpty(item, "Title"),
            ShortTitle = FirstNonEmpty(item, "TitleShort"),
          })
          .Where(i => !string.IsNullOrWhiteSpace(i.ID));
    }

    public async Task<(SectionModel D1, SectionModel D2, SectionModel D3)> LoadRealtime()
    {

      var emptySection = new SectionModel
      {
        Items = []
      };

      var json = await GetApiJsonAsync(
        "API/realtime_List_Home.aspx",
        new Dictionary<string, string>
        {
          ["PageSize"] = "15",
          ["Page"] = "1",
        });

      if (json == null)
      {
        return (emptySection, emptySection, emptySection);
      }

      var status = json["Status"]?.ToString();

      if (!string.Equals(status, "Success", StringComparison.OrdinalIgnoreCase))
      {
        return (emptySection, emptySection, emptySection);
      }

      var allItems = GetItems(json)
        .Select(item => new NewsItem
        {
          ID = FirstNonEmpty(item, "ID"),
          Title = FirstNonEmpty(item, "Title"),
          ShortTitle = FirstNonEmpty(item, "TitleShort"),
          ImageUrl = FirstNonEmpty(item, "Image"),
          CreateDate = FirstNonEmpty(item, "CreateDate"),
          Links = $"/news/detail/{FirstNonEmpty(item, "ID")}"
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

      var d1Top = d1Items.FirstOrDefault();
      var d2Top = d2Items.FirstOrDefault();
      var d3Top = d3Items.FirstOrDefault();

      var d1 = new SectionModel
      {
        SectionShortTitle = d1Top?.Title,
        SectionUrl = d1Top?.Links,
        SectionImageUrl = d1Top?.ImageUrl,
        Items = d1Items,
      };

      var d2 = new SectionModel
      {
        SectionShortTitle = d2Top?.Title,
        SectionUrl = d2Top?.Links,
        SectionImageUrl = d2Top?.ImageUrl,
        Items = d2Items,
      };

      var d3 = new SectionModel
      {
        SectionShortTitle = d3Top?.Title,
        SectionUrl = d3Top?.Links,
        SectionImageUrl = d3Top?.ImageUrl,
        Items = d3Items,
      };

      return (d1, d2, d3);
    }

    public async Task<IEnumerable<CarouselImage>> LoadCarouselImage()
    {
      var json = await GetApiJsonAsync("API/CarouselImage.aspx");

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
          .Select(item => new CarouselImage
          {
            Title = FirstNonEmpty(item, "Title"),
            ImageUrl = FirstNonEmpty(item, "image"),
            Links = FirstNonEmpty(item, "links"),
          })
          .Where(i => !string.IsNullOrWhiteSpace(i.Links));
    }

    public async Task<IEnumerable<AnchorItem>> LoadAnchorList()
    {
      var json = await GetApiJsonAsync("API/AnchorList.aspx");

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
          .Select(item => new AnchorItem
          {
            ID = FirstNonEmpty(item, "ID"),
            Title = FirstNonEmpty(item, "Title"),
            ImageUrl = FirstNonEmpty(item, "Image"),
            JobTitle = FirstNonEmpty(item, "JobTitle"),
          })
          .Where(a => !string.IsNullOrWhiteSpace(a.Title));
    }

    public async Task<IEnumerable<NewsItem>> LoadShortVideos()
    {
      var json = await GetApiJsonAsync(
        "API/FTVYTShortVideoList.aspx",
        new Dictionary<string, string>
        {
          ["Sp"] = "7",
          ["Page"] = "1",
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
          .Select(item => new NewsItem
          {
            Title = FirstNonEmpty(item, "Title", "Name"),
            ImageUrl = FirstNonEmpty(item, "Image", "image"),
            Links = $"/shortvideo/detail/{FirstNonEmpty(item, "VideoId")}",
          })
          .Where(a => !string.IsNullOrWhiteSpace(a.Links));
    }

    public async Task<IReadOnlyList<ProjNewsGroup>> LoadProjNews()
    {
      var json = await GetApiJsonAsync(
          "API/HomeFeatureNew.aspx",
          new Dictionary<string, string>
          {
            ["PageSize"] = "99",
            ["Page"] = "1",
          });

      if (json == null) { return []; }

      var status = json["Status"]?.ToString();

      if (!string.Equals(status, "Success", StringComparison.OrdinalIgnoreCase))
      {
        return [];
      }
      const string defaultImg =
          "https://cdn.ftvnews.com.tw/manasystem/FileData/NewsImg/b28ba1dc-ec15-4d06-94ce-2f3753d3abd6.jpg";

      var items = json["ITEM"]?.Children() ?? Enumerable.Empty<JToken>();

      var result = items.Select(item =>
      {
        var group = new ProjNewsGroup
        {
          ProjSN = FirstNonEmpty(item, "ProjSN"),
          ProjTitle = FirstNonEmpty(item, "ProjTitle"),
          ProjContent = FirstNonEmpty(item, "ProjContent"),
          ProjImageUrl = FirstNonEmpty(item, "ProjImage"),
        };

        var itembs = item["ITEMB"]?.Children() ?? Enumerable.Empty<JToken>();

        group.Items = [.. itembs.Select(b =>
        {
          var img = FirstNonEmpty(b, "Image");
          if (string.IsNullOrWhiteSpace(img)) img = defaultImg;

          return new ProjNewsItem
          {
            ID = FirstNonEmpty(b, "ID"),
            Title = FirstNonEmpty(b, "Title"),
            Content = FirstNonEmpty(b, "Content"),
            ImageUrl = img,
            ThumbUrl = FirstNonEmpty(b, "Thumb"),
            CreateDate = FirstNonEmpty(b, "CreateDate"),
          };
        })
        .Where(n => !string.IsNullOrWhiteSpace(n.ID) && !string.IsNullOrWhiteSpace(n.Title))];

        return group;
      })
      .Where(g => !string.IsNullOrWhiteSpace(g.ProjSN) && !string.IsNullOrWhiteSpace(g.ProjTitle))
      .ToList();

      return result;
    }

    public async Task<IReadOnlyList<HomeVideoGroup>> LoadHomeVideo()
    {
      var json = await GetApiJsonAsync("API/HomeVideoNew.aspx");

      if (json == null) { return []; }

      var status = json["Status"]?.ToString();

      if (!string.Equals(status, "Success", StringComparison.OrdinalIgnoreCase))
      {
        return [];
      }

      var items = json["ITEM"]?.Children() ?? Enumerable.Empty<JToken>();

      var result = items.Select(item =>
      {
        var group = new HomeVideoGroup
        {
          TitleCate = FirstNonEmpty(item, "M_TitleCate"),
          LinkCate = FirstNonEmpty(item, "M_LinkCate"),
          Title = FirstNonEmpty(item, "M_Title"),
          Img = FirstNonEmpty(item, "M_Img"),
          VideoId = FirstNonEmpty(item, "M_videoId"),
          Link = FirstNonEmpty(item, "M_Link"),
        };

        var itembs = item["ITEMB"]?.Children() ?? Enumerable.Empty<JToken>();

        group.Items = [.. itembs.Select(b =>
        {
          return new HomeVideoItem
          {
            ID = FirstNonEmpty(b, "ID"),
            Title = FirstNonEmpty(b, "Title"),
            ImageUrl = FirstNonEmpty(b, "Image"),
            CreateDate = FirstNonEmpty(b, "CreateDate"),
          };
        })
        .Where(n => !string.IsNullOrWhiteSpace(n.ID) && !string.IsNullOrWhiteSpace(n.Title))];

        return group;
      })
      .Where(g => !string.IsNullOrWhiteSpace(g.VideoId) && !string.IsNullOrWhiteSpace(g.TitleCate))
      .ToList();

      return result;
    }

    public async Task<IEnumerable<NewsItem>> LoadProgList()
    {
      var json = await GetApiJsonAsync("API/Prog.aspx");

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
          .Select(item => new NewsItem
          {
            Title = FirstNonEmpty(item, "Title"),
            ImageUrl = FirstNonEmpty(item, "Image"),
            Links = FirstNonEmpty(item, "Link"),
            Content = FirstNonEmpty(item, "Content")
          })
          .Where(a => !string.IsNullOrWhiteSpace(a.Title));
    }

    public async Task<IEnumerable<LiveImage>> LoadFTVLiveImage()
    {
      var json = await GetApiJsonAsync("API/FTVLiveImage.aspx");

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
          .Select(item => new LiveImage
          {
            ImageUrl = FirstNonEmpty(item, "image"),
          })
          .Where(a => !string.IsNullOrWhiteSpace(a.ImageUrl));
    }

    public async Task<IEnumerable<HotLive>> LoadHotLive()
    {
      var json = await GetApiJsonAsync("API/HotLive.aspx");

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
          .Select(item => new HotLive
          {
            ID = FirstNonEmpty(item, "ID"),
            Title = FirstNonEmpty(item, "Title"),
            ImageUrl = FirstNonEmpty(item, "Image"),
          })
          .Where(a => !string.IsNullOrWhiteSpace(a.ID));
    }

    public async Task<IEnumerable<NewsVideo>> LoadNewsVideo()
    {
      var json = await GetApiJsonAsync("API/FtvGetHotVNewOrd.aspx");

      if (json == null)
      {
        return [];
      }

      var status = json["Status"]?.ToString();

      if (!string.Equals(status, "Success", StringComparison.OrdinalIgnoreCase))
      {
        return [];
      }

      return [.. GetItems(json)
          .Select(item => new NewsVideo
          {
            ID = FirstNonEmpty(item, "ID"),
            Title = FirstNonEmpty(item, "Title"),
            VideoId = FirstNonEmpty(item, "Video"),
            Platform = FirstNonEmpty(item, "VPlatform"),
            CDNID = FirstNonEmpty(item, "FtvNewsCDNID"),
            Thumb = FirstNonEmpty(item, "Thumb"),
            ImageUrl = FirstNonEmpty(item, "Image"),
            YTImage = FirstNonEmpty(item, "YTImage")
          })
          .Where(a => !string.IsNullOrWhiteSpace(a.ID))];
    }
  }
}