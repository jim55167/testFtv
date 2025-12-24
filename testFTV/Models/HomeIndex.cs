namespace testFTV.Models
{
  public class HomeIndex
  {
    public IEnumerable<NewsItem> TextBasedGuide { get; set; } = [];
    public IEnumerable<NewsItem> HotNewsMarqueeList { get; set; } = [];
    public IEnumerable<NewsItem> FocusNewsList { get; set; } = [];
    public IEnumerable<NewsItem> HotNewsList { get; set; } = [];
    public IEnumerable<NewsItem> ShortVideos { get; set; } = [];
    public IEnumerable<NewsItem> ProgList { get; set; } = [];
    public IEnumerable<ProjNewsGroup> ProjNews { get; set; } = [];
    public IEnumerable<HomeVideoGroup> HomeVideo { get; set; } = [];
    public IEnumerable<LiveImage> FTVLiveImage { get; set; } = [];
    public IEnumerable<HotLive> HotLive { get; set; } = [];
    public IEnumerable<NewsVideo> NewsVideo { get; set; } = [];

    public SectionModel D1 { get; set; } = new();
    public SectionModel D2 { get; set; } = new();
    public SectionModel D3 { get; set; } = new();

    public IEnumerable<CarouselImage> CarouselImage { get; set; } = [];
    public IEnumerable<AnchorItem> AnchorList { get; set; } = [];

  }

  public class NewsItem
  {
    public string ID { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string ShortTitle { get; set; } = string.Empty;
    public string Links { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public string CreateDate { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string CreateTime { get; set; } = string.Empty;
    public string Cate { get; set; } = string.Empty;
  }

  public class ProjNewsGroup
  {
    public string ProjSN { get; set; } = string.Empty;
    public string ProjTitle { get; set; } = string.Empty;
    public string ProjContent { get; set; } = string.Empty;
    public string ProjImageUrl { get; set; } = string.Empty;

    public IReadOnlyList<ProjNewsItem> Items { get; set; } = [];
  }

  public class ProjNewsItem
  {
    public string ID { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public string ThumbUrl { get; set; } = string.Empty;
    public string CreateDate { get; set; } = string.Empty;
  }

  public class HomeVideoGroup
  {
    public string TitleCate { get; set; } = string.Empty;
    public string LinkCate { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Img { get; set; } = string.Empty;
    public string VideoId { get; set; } = string.Empty;
    public string Link { get; set; } = string.Empty;

    public IReadOnlyList<HomeVideoItem> Items { get; set; } = [];
  }

  public class HomeVideoItem
  {
    public string ID { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public string CreateDate { get; set; } = string.Empty;
  }

  public class SectionModel
  {
    public string SectionUrl { get; set; } = string.Empty;
    public string SectionImageUrl { get; set; } = string.Empty;
    public string SectionShortTitle { get; set; } = string.Empty;
    public List<NewsItem> Items { get; set; } = new();
  }

  public class CarouselImage
  {
    public string Title { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public string Links { get; set; } = string.Empty;
  }

  public class AnchorItem
  {
    public string ID { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string JobTitle { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
  }

  public class LiveImage
  {
    public string ImageUrl { get; set; } = string.Empty;
  }

  public class HotLive
  {
    public string ID { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
  }

  public class NewsVideo
  {
    public string ID { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string VideoId { get; set; } = string.Empty;
    public string Platform { get; set; } = string.Empty;
    public string CDNID { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public string YTImage { get; set; } = string.Empty;
    public string Thumb { get; set; } = string.Empty;
    public string CheckImageUrl =>
        !string.IsNullOrWhiteSpace(Thumb) ? Thumb :
        !string.IsNullOrWhiteSpace(ImageUrl) ? ImageUrl :
        (YTImage ?? string.Empty);
  }
}
