using Microsoft.EntityFrameworkCore;
using WebApi.Template.Model.Domain;

namespace WebApi.Template.DataAccess
{
    public partial class TemplateDbContext : DbContext
    {
        public TemplateDbContext(DbContextOptions<TemplateDbContext> options) : base(options)
        {
        }

        public virtual DbSet<NameAddresses> NameAddresses { get; set; }
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NameAddresses>(DbConfiguration.Configure_NameAddresses);

        }
    }
}
