using GayQuyTuThien.DataContext.Entity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace GayQuyTuThien.DataContext
{
    public class DataDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public DataDbContext(DbContextOptions<DataDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            foreach (var relationship in builder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
            base.OnModelCreating(builder);
        }
        //public virtual DbSet<Menu> Menus { get; set; }
        public virtual DbSet<Function> Functions { get; set; }

		public virtual DbSet<Picture> Pictures { get; set; }

		public virtual DbSet<SubmitForm> SubmitForms { get; set; }

        public virtual DbSet<HtmlContent> HtmlContent { get; set; }
    }
}
