namespace testFTV.Models.Home
{
  public class HomeIndexViewModel
  {
    public IEnumerable<NewsItem> TextBasedGuide { get; set; }
    public IEnumerable<NewsItem> HotNewsList { get; set; }
    public IEnumerable<NewsItem> FocusNewsList { get; set; }
    public IEnumerable<NewsItem> HotNewList { get; set; }

    public SectionModel D1 { get; set; }
    public SectionModel D2 { get; set; }
    public SectionModel D3 { get; set; }

    public IEnumerable<ImageCarouselItem> CarouselImages { get; set; }
    public IEnumerable<AnchorItem> AnchorList { get; set; }
  }

  public class NewsItem
  {
    public string Title { get; set; }
    public string Url { get; set; }
  }

  public class SectionModel
  {
    public string SectionTitle { get; set; }
    public IEnumerable<NewsItem> Items { get; set; }
  }

  public class ImageCarouselItem
  {
    public string ImageUrl { get; set; }
    public string LinkUrl { get; set; }
  }

  public class AnchorItem
  {
    public string Name { get; set; }
    public string ImageUrl { get; set; }
    public string Url { get; set; }
  }

}
