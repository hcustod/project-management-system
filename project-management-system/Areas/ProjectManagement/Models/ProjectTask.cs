using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace COMP2139_ICE.Areas.ProjectManagement.Models;

public class ProjectTask
{
    [Key]
    public int ProjectTaskId { get; set; }
    
    [Display(Name = "Task Title")]
    [Required]
    [StringLength(100, ErrorMessage = "Task Title cannot be longer than 100 characters.")]
    public required string Title { get; set; }
    
    [Display(Name = "Task Description")]
    [Required]
    [DataType(DataType.MultilineText)]
    [StringLength(500, ErrorMessage = "Task Description cannot be longer than 500 characters.")]
    public required string Description { get; set; }
    
    [Display(Name = "Parent Project Id")]
    [ForeignKey("Project")]
    public int ProjectId { get; set; }
    
    // Navigation Property
    // Allows for easy access to the related Project entity from the Task entity
    public Project? Project { get; set; }
}