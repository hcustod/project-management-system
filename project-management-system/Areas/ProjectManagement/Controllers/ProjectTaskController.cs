using Microsoft.AspNetCore.Mvc;
using COMP2139_ICE.Data;
using COMP2139_ICE.Areas.ProjectManagement.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace COMP2139_ICE.Areas.ProjectManagement.Controllers;

[Area("ProjectManagement")]
[Route("[area]/[controller]/[action]")]
public class ProjectTaskController : Controller
{

    private readonly ApplicationDbContext _context;

    public ProjectTaskController(ApplicationDbContext context)
    {
        _context = context;
    }
    
    
    [HttpGet("Index/{projectId:int}")]
    public async Task<IActionResult> Index(int projectId)
    {

        var tasks = await _context
            .ProjectTasks
            .Where(t => t.ProjectId == projectId)
            .ToListAsync();

        ViewBag.ProjectId = projectId;
        ViewBag.SearchPerformed = false;
        
        return View(tasks);
    }

    [HttpGet("Details/{id:int}")]
    public async Task<IActionResult> Details(int id)
    {
        var task = await _context
            .ProjectTasks
            .Include(t => t.Project)
            .FirstOrDefaultAsync(t => t.ProjectTaskId == id);

        if (task == null)
        {
            return NotFound();
        }
        
        return View(task);
    }

 
    [HttpGet("Create/{projectId:int}")]
    public async Task<IActionResult> Create(int projectId)
    {
        var project = await _context.Projects.FindAsync(projectId);
        if (project == null)
        {
            return NotFound();
        }

        var task = new ProjectTask()
        {
            ProjectId = projectId,
            Title = "",
            Description = ""
        };

        return View(task);
    }

    [HttpPost("Create/{projectId:int}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("ProjectTaskId", "Title", "Description", "ProjectId")]ProjectTask task)
    {
        if (ModelState.IsValid)
        {
            await _context.ProjectTasks.AddAsync(task);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", new { projectId = task.ProjectId });
        }

        return View(task);
    }

    
    [HttpGet("Edit/{id:int}")]
    public async Task<IActionResult> Edit(int id)
    {
        var task = await _context
            .ProjectTasks
            .Include(t => t.Project)
            .FirstOrDefaultAsync(t => t.ProjectTaskId == id);

        if (task == null)
        {
            return NotFound();
        }

        return View(task);
    }
    
    [HttpPost("Edit/{id:int}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("ProjectTaskId","Title", "Description", "ProjectId")] ProjectTask task)
    {
        if (id != task.ProjectTaskId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            _context.ProjectTasks.Update(task);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", new { projectId = task.ProjectId });
        }

        return View(task);
    }
    
    [HttpGet("Delete/{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var task = await _context
            .ProjectTasks
            .Include(t => t.Project)
            .FirstOrDefaultAsync(t => t.ProjectTaskId == id);

        if (task == null)
        {
            return NotFound();
        }

        return View(task);
    }

    
    [HttpPost("Delete/{id:int}"), ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var task = await _context.ProjectTasks.FindAsync(id);
        if (task != null)
        {
            int projectId = task.ProjectId;
            
            _context.ProjectTasks.Remove(task);
            await _context.SaveChangesAsync();
            
            return RedirectToAction("Index", new { projectId });
        }
        
        return View(task);
    }

    
    [HttpGet("Search/{projectId?}")]
    public async Task<IActionResult> Search(int? projectId, string searchString)
    {
        var tasksQuery = _context.ProjectTasks.AsQueryable();

        bool searchPerformed = !string.IsNullOrWhiteSpace(searchString);

        if (projectId.HasValue)
        {
            tasksQuery = tasksQuery.Where(t => t.ProjectId == projectId);
        }

        if (searchPerformed)
        {
            searchString = searchString.ToLower();

            tasksQuery = tasksQuery.Where(p => p.Title.ToLower().Contains(searchString) ||
                                                     p.Description.ToLower().Contains(searchString));
        }

        var tasks = await tasksQuery.ToListAsync();

        ViewBag.ProjectId = projectId;
        ViewBag.SearchPerformed = searchPerformed;
        ViewData["SearchString"] = searchString;

        return View("Index", tasks);
    }

}