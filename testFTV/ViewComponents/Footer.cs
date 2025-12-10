using Microsoft.AspNetCore.Mvc;
using testFTV.Models;
using testFTV.Services;

namespace testFTV.ViewComponents
{
  public class Footer : ViewComponent
  {
    private readonly Services.FooterService _footerService;

    public Footer(Services.FooterService footerService)
    {
      _footerService = footerService;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
      Models.FooterModel model = await _footerService.LoadIdleInfoAsync();
      return View(model);
    }
  }
}