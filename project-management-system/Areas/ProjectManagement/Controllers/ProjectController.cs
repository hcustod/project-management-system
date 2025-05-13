using COMP2139_ICE.Data;
using Microsoft.AspNetCore.Mvc;
using COMP2139_ICE.Areas.ProjectManagement.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace COMP2139_ICE.Areas.ProjectManagement.Controllers;

[Area("ProjectManagement")]
[Route("[area]/[controller]/[action]")]
public class ProjectController : Controller
{
    
    private readonly ILogger<ProjectController> _logger;

    // Connection context needed
    private readonly ApplicationDbContext _context;

    public ProjectController(ApplicationDbContext context, ILogger<ProjectController> logger)
    {
        _logger = logger;
        _context = context;
    }
    
    [HttpGet("")]
    public async Task<IActionResult> Index()
    {
        _logger.LogInformation("Accessed ProjectController Index at {Time}", DateTime.Now);

        var projects = await _context.Projects.ToListAsync();
        return View(projects);
    }
    
    [HttpGet("Create")]
    // Create methods
    public IActionResult Create()
    {
        _logger.LogInformation("Accessed ProjectController Create at {Time}", DateTime.Now);
        
        return View();
    }
    
    
    [HttpPost("Create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Project project)
    {

        if (ModelState.IsValid)
        {
            // To ensure its utc before saves to DB. Else error occurs (?). 
            project.StartDate = DateTime.SpecifyKind(project.StartDate, DateTimeKind.Utc);
            project.EndDate = DateTime.SpecifyKind(project.EndDate, DateTimeKind.Utc);

            
            _context.Projects.Add(project);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        
        return View(project);
    }
    
    
    [HttpGet("Details/{id:int}")]
    public async Task<IActionResult> Details(int id)
    {
        _logger.LogInformation("Accessed ProjectController Details at {Time}", DateTime.Now);
        
        var project = await _context.Projects.FirstOrDefaultAsync(p => p.ProjectId == id);

        if (project == null)
        {
            _logger.LogWarning("Could not find Project with id of {id}", id);
            return NotFound();

        }

        return View(project);
    }
    
    // Edit methods 
    [HttpGet("Edit/{id:int}")]
    public async Task<IActionResult> Edit(int id)
    {
        var project = await _context.Projects.FindAsync(id);
        if (project == null)
        {
            return NotFound();
        }

        return View(project);
    }
    
    [HttpPost("Edit/{id:int}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("ProjectId", "Name", "Description", "Status", "StartDate", "EndDate")] Project project)
    {
        if (id != project.ProjectId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                // To ensure its utc before saves to DB. Else error occurs (?). 
                project.StartDate = DateTime.SpecifyKind(project.StartDate, DateTimeKind.Utc);
                project.EndDate = DateTime.SpecifyKind(project.EndDate, DateTimeKind.Utc);
                
                _context.Projects.Update(project);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ProjectExists(project.ProjectId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToAction();
        }

        return View(project);
    }

    private async Task<bool> ProjectExists(int id)
    {
        return await _context.Projects.AnyAsync(e => e.ProjectId == id);
    }
    
    
    [HttpGet("Delete/{id:int}")]
    // Delete methods
    public async Task<IActionResult> Delete(int id)
    {
        var project = await _context.Projects.FindAsync(id);
        if (project == null)
            return NotFound();

        return View(project);
    }


    [HttpPost("Delete/{id:int}"), ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var project = await _context.Projects.FindAsync(id);
        if (project != null)
        {
            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
            
            return RedirectToAction("Index");
        }

        return NotFound();
    }

    [HttpGet("Search/{searchString?}")]
    public async Task<IActionResult> Search(string? searchString)
    {
        var projectsQuery = _context.Projects.AsQueryable();

        bool searchPerformed = !string.IsNullOrWhiteSpace(searchString);
                     
        if (searchPerformed)
        {
            searchString = searchString.ToLower();

            projectsQuery = projectsQuery.Where(p => p.Name.ToLower().Contains(searchString) ||
                                                     p.Description.ToLower().Contains(searchString));
        }

        var projects = await projectsQuery.ToListAsync();

        ViewBag.SearchPerformed = searchPerformed;
        ViewData["SearchString"] = searchString;

        return View("Index", projects);
    }
    

}