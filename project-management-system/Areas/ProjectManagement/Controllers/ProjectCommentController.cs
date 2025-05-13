using COMP2139_ICE.Areas.ProjectManagement.Models;
using COMP2139_ICE.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace COMP2139_ICE.Areas.ProjectManagement.Controllers;

[Area("ProjectManagement")]
[Route("ProjectManagement/[controller]/[action]")]
public class ProjectCommentController : Controller
{
    private readonly ApplicationDbContext _context;

    public ProjectCommentController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> GetComments(int projectId)
    {
        var comments = await _context.ProjectComments
            .Where(c => c.ProjectId == projectId)
            .OrderByDescending(c => c.DatePosted)
            .ToListAsync();

        return Json(comments);
    }

    public async Task<IActionResult> AddComment([FromBody] ProjectComment comment)
    {
        if (ModelState.IsValid)
        {
            comment.DatePosted = DateTime.Now;

            await _context.ProjectComments.AddAsync(comment);

            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Comment Added Successfully." });
        }

        var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
        return Json(new { success = false, message = "Invalid comment data.", errors = errors });
    }
    
}