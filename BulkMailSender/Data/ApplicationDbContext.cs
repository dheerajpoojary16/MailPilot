using Microsoft.EntityFrameworkCore;
using BulkMailSender.Models;

namespace BulkMailSender.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options
        ) : base(options)
        {
        }

        public DbSet<EmailTemplate> EmailTemplates { get; set; }
    }
}