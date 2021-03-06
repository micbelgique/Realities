using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Entities.Communities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Persistence
{
    public class ApplicationDbContext : IdentityDbContext<User, Role, string>, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options){
            this.Database.EnsureCreated();
        }

        public DbSet<Community> Communities { get; set; }
        public DbSet<Pannel> Pannels { get; set; }
        public DbSet<Anchor> Anchors { get; set; }
        public DbSet<Interaction> Interactions { get; set; }
        public DbSet<Admission> Admissions { get; set; }
        public DbSet<Placement> Placements { get; set; }
        public DbSet<Report> Reports { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            var result = await base.SaveChangesAsync(cancellationToken);

            //await DispatchEvents();

            return result;
        }
    }
}
