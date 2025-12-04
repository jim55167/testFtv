using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using testFTV.Models;
using testFTV.Models.Home;
using testFTV.Services;

namespace testFTV.Controllers
{
  public class HomeController : Controller
  {
    private readonly HomeService _service;

    public HomeController(HomeService service)
    {
      _service = service;
    }

    public async Task<IActionResult> Index()
    {
      var realtime = await _service.LoadRealtime();

      var vm = new HomeIndexViewModel
      {
        TextBasedGuide = await _service.LoadTextBasedGuide(),
        HotNewsList = await _service.LoadHotNewsList(),
        FocusNewsList = await _service.LoadFocusNewsList(),
        HotNewList = await _service.LoadHotNewList(),
        CarouselImages = await _service.LoadCarouselImages(),
        AnchorList = await _service.LoadAnchorList(),
        D1 = realtime.D1,
        D2 = realtime.D2,
        D3 = realtime.D3,
      };

      return View(vm);
    }

    public IActionResult Privacy()
    {
      return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
      return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
  }
}
