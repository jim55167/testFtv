namespace testFTV.Services
{
  public class Now_date
  {
    public string Now_st()
    {
      var tz = TimeZoneInfo.FindSystemTimeZoneById("Taipei Standard Time");
      var now = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, tz);
      return now.ToString("yyyy/MM/dd HH:mm:ss");
    }
  }
}
