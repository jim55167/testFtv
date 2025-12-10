using Microsoft.AspNetCore.Mvc;

namespace testFTV.ViewComponents
{
  public class Header : ViewComponent
  {
    public IViewComponentResult Invoke()
    {
      return View();
    }
  }
}
