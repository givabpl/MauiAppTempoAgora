using Microsoft.EntityFrameworkCore;
using MauiAppTempoAgora.Models;

namespace MauiAppTempoAgora.Data
{
    public class TempoDbContext : DbContext
    {
        public TempoDbContext(DbContextOptions<TempoDbContext> options)
            : base(options)
        {
        }

        public DbSet<Tempo> Previsoes { get; set; }
    }
}
