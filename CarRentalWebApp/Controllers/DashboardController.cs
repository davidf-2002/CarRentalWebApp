using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

public class Dashboard : Controller
{

    // GET: /Dashboard
    public async Task<IActionResult> Index()
    {
        return View();
    }
}