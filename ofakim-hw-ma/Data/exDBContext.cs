using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ofakim_hw_ma.Data
{
    public class exDBContext : DbContext
    {
        public exDBContext(DbContextOptions<exDBContext> options) : base(options)
        {
        }

        public DbSet<ExConvertEntity> exConvertEntities { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ExConvertEntity>()
            .HasIndex(u => u.ExCorrencyName)
            .IsUnique();
        }
    }
}
