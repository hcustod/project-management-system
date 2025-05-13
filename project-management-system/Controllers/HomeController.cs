using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using COMP2139_ICE.Models;
using COMP2139_ICE.Data;

namespace COMP2139_ICE.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _context;
    
    public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult Index()
    {
        // Has to match or be below what is being logged in app settings. 
        _logger.LogInformation("Accessed HomeController Index at {Time}", DateTime.Now);
        return View();
    }

    public IActionResult About()
    {
        _logger.LogInformation("Accessed HomeController About at {Time}", DateTime.Now);
        return View();
    }
    
    [HttpGet]
    public IActionResult GeneralSearch(string searchType, string searchString)
    {
        _logger.LogInformation("Accessed HomeController GeneralSearch at {Time}", DateTime.Now);
        
        if (string.IsNullOrWhiteSpace(searchType) || string.IsNullOrWhiteSpace(searchString))
        {
            return RedirectToAction(nameof(Index), "Home");
        }
        
        searchType = searchType.ToLower();
        if (searchType == "projects")
        {
            return RedirectToAction("Search", "Project", new { area = "ProjectManagement", searchString });
        }

        if (searchType == "tasks")
        {
            return RedirectToAction("Search", "ProjectTask", new { area = "ProjectManagement", searchString });
        }

        return RedirectToAction(nameof(Index), "Home");
    }
    
    
    /*
    // Un-needed? 
    [HttpGet]
    public IActionResult Search(string searchTerm)
    {
        var projects = _context.Projects.Where(p => p.Name.Contains(searchTerm) || p.Description.Contains(searchTerm)).ToList();
        var tasks = _context.ProjectTasks.Where(t => t.Title.Contains(searchTerm) || t.Description.Contains(searchTerm)).ToList();

        ViewBag.SearchTerm = searchTerm;
        
        return View(new SearchResultsViewModel { Projects = projects, Tasks = tasks});
    }
    */
    
    
    // Errors 
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        _logger.LogInformation("Accessed HomeController Error at {Time}", DateTime.Now);
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }


    [HttpGet]
    public IActionResult NotFoundPage(int statusCode)
    {
        _logger.LogInformation("NotFoundPage invoked at {Time}", DateTime.Now);
        if (statusCode == 404)
        {
            return View("NotFound");
        }

        return View("Error");
    }
}