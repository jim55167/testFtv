using Microsoft.AspNetCore.Mvc;
using testFTV.Models.Footer;
using testFTV.Services;

namespace testFTV.ViewComponents
{
  public class FooterViewComponent : ViewComponent
  {
    private readonly FooterService _footerService;

    public FooterViewComponent(FooterService footerService)
    {
      _footerService = footerService;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
      FooterViewModel model = await _footerService.LoadIdleInfoAsync();
      return View(model);
    }
  }
}