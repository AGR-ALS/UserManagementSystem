using Microsoft.AspNetCore.Mvc;

namespace UserManagementSystem.Main.Controllers;

public class ErrorController : Controller
{
    public ActionResult Blocked()
    {
        return View("Blocked");
    }
}