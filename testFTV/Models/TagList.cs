namespace testFTV.Models
{
  public class TagList
  {
    public string TagId { get; set; } = string.Empty;
    public string TagTitle { get; set; } = string.Empty;
    public IReadOnlyList<NewsWebN> FtvGetNewsWeb { get; set; } = [];
    public IEnumerable<HashTags> HashTagsInfo { get; set; } = [];

    public int Page { get; set; } = 1;
    public int PageTotal { get; set; } = 0;

    public string PageTitle { get; set; } = string.Empty;

    // VB: postlist_obj -> JSON-LD
    public string NewsListJsonLd { get; set; } = string.Empty;

    // VB: Meta_TitleSt 前 6 筆標題串
    public string MetaTitleCsv { get; set; } = string.Empty;

    public Pagination SetPageLink { get; set; } = new();

  }

  public class NewsWebN
  {
    public string ID { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public string CreateDate { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
  }

  public class HashTags
  {
    public string Title { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string CssContent { get; set; } = string.Empty;
  }

  public class Pagination
  {
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public IReadOnlyList<int> Pages { get; set; } = [];

    public bool IsFirst => CurrentPage <= 1;
    public bool IsLast => TotalPages <= 0 || CurrentPage >= TotalPages;
  }
}
