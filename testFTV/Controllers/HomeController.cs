using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using testFTV.Models;
using testFTV.Services;

namespace testFTV.Controllers
{
  public class HomeController : Controller
  {
    private readonly Services.HomeService _service;

    public HomeController(Services.HomeService service)
    {
      _service = service;
    }

    public async Task<IActionResult> Index()
    {
      var realtime = await _service.LoadRealtime();

      var vm = new HomeIndex
      {
        TextBasedGuide = await _service.LoadTextBasedGuide(),
        HotNewsList = await _service.LoadHotNewsList(),
        FocusNewsList = await _service.LoadFocusNewsList(),
        HotNewList = await _service.LoadHotNewList(),
        CarouselImages = await _service.LoadCarouselImages(),
        AnchorList = await _service.LoadAnchorList(),
        ShortVideos = await _service.LoadShortVideos(),
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
      return View(new Error { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
  }
}
