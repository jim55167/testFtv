using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using testFTV.Models;
using testFTV.Services;

namespace testFTV.Controllers
{
  [Route("tag")]
  public class TagListController : Controller
  {
    private readonly TagListService _service;

    public TagListController(TagListService service)
    {
      _service = service;
    }

    [HttpGet("{id}")]
    [HttpGet("{id}/{page:int}")]
    public async Task<IActionResult> Index(string id, int page = 1)
    {
      var vm = await _service.LoadFtvGetNewsWeb(id, page);

      if (vm == null)
      {
        return Redirect("/404");
      }

      ViewData["Title"] = vm.PageTitle;

      ViewData["MetaTitleCsv"] = vm.MetaTitleCsv;

      return View("~/Views/Tag/Index.cshtml", vm);
    }
  }
}
