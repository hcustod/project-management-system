using COMP2139_ICE.Models;
using COMP2139_ICE.Areas.ProjectManagement.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace COMP2139_ICE.Data;

public class ApplicationDbContext : IdentityDbContext
{

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
    
    public DbSet<Project> Projects { get; set; }
    
    public DbSet<ProjectTask> ProjectTasks { get; set; }
    
    public DbSet<ProjectComment> ProjectComments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Ensure Identity config and table are created
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Project>()
            .HasMany(p => p.Tasks)
            .WithOne(t => t.Project)
            .HasForeignKey(t => t.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Project>().HasData(
            new Project { ProjectId = 1, Name = "Assignment 1", Description = "COMP2139 - Assignment 1" },
            new Project { ProjectId = 2, Name = "Assignment 2", Description = "COMP2139 - Assignment 2" }
        );
    }

}