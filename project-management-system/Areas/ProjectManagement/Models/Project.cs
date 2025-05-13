using System.ComponentModel.DataAnnotations;

namespace COMP2139_ICE.Areas.ProjectManagement.Models;

public class Project
{
    public int ProjectId { get; set; }
    
    [Display(Name = "Project Name")]
    [Required]
    [StringLength(100, ErrorMessage = "Project name cannot be longer than 100 characters.")]
    public required string Name { get; set; }
    
    [Display(Name = "Project Description")]
    [DataType(DataType.MultilineText)]
    [StringLength(500, ErrorMessage = "Project Description cannot be longer than 500 characters.")]
    public string? Description { get; set; }
    
    [Display(Name = "Project Start Date")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
    public DateTime StartDate { get; set; }
    
    [Display(Name = "Project End Date")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
    public DateTime EndDate { get; set; }
    
    [Display(Name = "Project Status")]
    public string? Status { get; set; }

    // One to many
    public List<ProjectTask>? Tasks { get; set; } = new();
}