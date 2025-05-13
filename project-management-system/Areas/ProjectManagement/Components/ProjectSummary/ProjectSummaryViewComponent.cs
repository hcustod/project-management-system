using COMP2139_ICE.Areas.ProjectManagement.Models;
using COMP2139_ICE.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace COMP2139_ICE.Areas.ProjectManagement.Components.ProjectSummary;

public class ProjectSummaryViewComponent : ViewComponent
{
    private readonly ApplicationDbContext _context;
    
    public ProjectSummaryViewComponent(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IViewComponentResult> InvokeAsync(int projectId)
    {
        
        var project = await _context.Projects
            .Include(p => p.Tasks)
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.ProjectId == projectId);

        if (project == null)
        {
            return Content("Project Not Content");
        }

        return View(project);
    }
}