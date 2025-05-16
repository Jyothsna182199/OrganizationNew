using Microsoft.EntityFrameworkCore;

namespace OrganizationForm.Models.cs
{
    public class OrgFormDbContext:DbContext
    {
        public OrgFormDbContext(DbContextOptions<OrgFormDbContext> options) : base(options) 
        {
            
        }

        public DbSet<OrgForm> OrgForm1 { get; set; }
    }
}
