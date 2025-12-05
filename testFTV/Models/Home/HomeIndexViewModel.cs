namespace testFTV.Models.Home
{
  public class HomeIndexViewModel
  {
    public IEnumerable<NewsItem> TextBasedGuide { get; set; } = Enumerable.Empty<NewsItem>();
    public IEnumerable<NewsItem> HotNewsList { get; set; } = Enumerable.Empty<NewsItem>();
    public IEnumerable<NewsItem> FocusNewsList { get; set; } = Enumerable.Empty<NewsItem>();
    public IEnumerable<NewsItem> HotNewList { get; set; } = Enumerable.Empty<NewsItem>();

    public SectionModel D1 { get; set; } = new();
    public SectionModel D2 { get; set; } = new();
    public SectionModel D3 { get; set; } = new();

    public IEnumerable<ImageCarouselItem> CarouselImages { get; set; } = Enumerable.Empty<ImageCarouselItem>();
    public IEnumerable<AnchorItem> AnchorList { get; set; } = Enumerable.Empty<AnchorItem>();
  }

  public class NewsItem
  {
    public string Title { get; set; } = string.Empty;
    public string? ShortTitle { get; set; }
    public string Url { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public string? TimeText { get; set; }
  }

  public class SectionModel
  {
    public string SectionTitle { get; set; } = string.Empty;
    public IEnumerable<NewsItem> Items { get; set; } = Enumerable.Empty<NewsItem>();
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
