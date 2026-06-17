using Microsoft.AspNetCore.Mvc;

namespace UserManagementSystem.Main.Controllers;

public class HomeController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }
}