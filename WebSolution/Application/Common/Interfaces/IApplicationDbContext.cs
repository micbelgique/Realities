using Domain.Entities;
using Domain.Entities.Communities;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        public DbSet<Community> Communities { get; set; }
        public DbSet<Anchor> Anchors { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Pannel> Pannels { get; set; }
        public DbSet<Interaction> Interactions { get; set; }
        public DbSet<Admission> Admissions { get; set; }
        public DbSet<Placement> Placements { get; set; }
        public DbSet<Report> Reports { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
