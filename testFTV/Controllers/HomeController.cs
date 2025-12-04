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
      var vm = new HomeIndexViewModel
      {
        HotNewsListHtml = await _service.LoadHotNewsList(),
        FocusNewsListHtml = await _service.LoadFocusNewsList(),
        HotNewListHtml = await _service.LoadHotNewList(),
        ProjNewsHtml = await _service.LoadProjNews(),
        ProgListHtml = await _service.LoadProgList(),
        AnchorListHtml = await _service.LoadAnchorList(),
        HotLiveHtml = await _service.LoadHotLive(),
        HomeVideoHtml = await _service.LoadHomeVideo(),
        NewsVideoHtml = await _service.LoadNewsVideo(),
        TextBasedGuideHtml = await _service.LoadTextBasedGuide(),
        CarouselImageHtml = await _service.LoadCarouselImage(),
        FTVLiveImageHtml = await _service.LoadFTVLiveImage(),
        ShortVideoHtml = await _service.LoadShortVideos(),
      };

      var realtime = await _service.LoadRealtime();
      vm.D1_St = realtime.D1_St;
      vm.D2_St = realtime.D2_St;
      vm.D3_St = realtime.D3_St;
      vm.D1_List = realtime.D1_List;
      vm.D2_List = realtime.D2_List;
      vm.D3_List = realtime.D3_List;

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
