namespace testFTV.Models
{
  public class HomeIndex
  {
    public IEnumerable<NewsItem> TextBasedGuide { get; set; } = [];
    public IEnumerable<NewsItem> HotNewsMarqueeList { get; set; } = [];
    public IEnumerable<NewsItem> FocusNewsList { get; set; } = [];
    public IEnumerable<NewsItem> HotNewsList { get; set; } = [];
    public IEnumerable<NewsItem> ShortVideos { get; set; } = [];
    public IEnumerable<ProjNewsGroup> ProjNews { get; set; } = [];
    public IEnumerable<HomeVideoGroup> HomeVideo { get; set; } = [];

    public SectionModel D1 { get; set; } = new();
    public SectionModel D2 { get; set; } = new();
    public SectionModel D3 { get; set; } = new();

    public IEnumerable<ImageCarouselItem> CarouselImages { get; set; } = [];
    public IEnumerable<AnchorItem> AnchorList { get; set; } = [];

  }

  public class NewsItem
  {
    public string Title { get; set; } = string.Empty;
    public string ShortTitle { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public string TimeText { get; set; } = string.Empty;
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

  public class ImageCarouselItem
  {
    public string ImageUrl { get; set; } = string.Empty;
    public string LinkUrl { get; set; } = string.Empty;
  }

  public class AnchorItem
  {
    public string Name { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
  }

}
