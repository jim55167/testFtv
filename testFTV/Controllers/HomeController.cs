using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using testFTV.Models;
using testFTV.Services;

namespace testFTV.Controllers
{
  public class HomeController(HomeService service) : Controller
  {
    private readonly HomeService _service = service;

    public async Task<IActionResult> Index()
    {
      var realtime = await _service.LoadRealtime();

      var vm = new HomeIndex
      {
        TextBasedGuide = await _service.LoadTextBasedGuide(),
        HotNewsMarqueeList = await _service.LoadHotNewsList(),
        FocusNewsList = await _service.LoadFocusNewsList(),
        HotNewsList = await _service.LoadHotNewList(),
        CarouselImage = await _service.LoadCarouselImage(),
        AnchorList = await _service.LoadAnchorList(),
        ShortVideos = await _service.LoadShortVideos(),
        ProjNews = await _service.LoadProjNews(),
        HomeVideo = await _service.LoadHomeVideo(),
        ProgList = await _service.LoadProgList(),
        FTVLiveImage = await _service.LoadFTVLiveImage(),
        HotLive = await _service.LoadHotLive(),
        NewsVideo = await _service.LoadNewsVideo(),
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
