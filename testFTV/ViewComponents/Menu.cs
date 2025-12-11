using Microsoft.AspNetCore.Mvc;
using testFTV.Services;

namespace testFTV.ViewComponents
{
  public class Menu : ViewComponent
  {
    private readonly MenuService _menuService;

    public Menu(MenuService menuService)
    {
      _menuService = menuService;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
      var model = await _menuService.LoadAsync();
      return View(model);
    }
  }
}