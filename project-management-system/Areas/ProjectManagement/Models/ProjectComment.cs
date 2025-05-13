using System.ComponentModel.DataAnnotations;

namespace COMP2139_ICE.Areas.ProjectManagement.Models;

public class ProjectComment
{
    [Key]
    public int ProjectCommentId { get; set; }
    
    [Display(Name="Project Message")]
    [StringLength(500, ErrorMessage = "Project Name cannot be longer than 500 characters.")]
    public string? Content { get; set; }
    
    [Display(Name="Date Posted")]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
    [DataType(DataType.DateTime)]
    private DateTime _datePosted;
    public DateTime DatePosted
    {
        get => _datePosted;
        set => _datePosted = DateTime.SpecifyKind(value, DateTimeKind.Utc);
    }
    
    // Foreign key
    public int ProjectId { get; set; }

    // Navigation Property
    public Project? Project { get; set; }
    
}

