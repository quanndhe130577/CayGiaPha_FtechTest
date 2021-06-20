using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CayGiaPhaTest
{
    public class GPDbContext : DbContext
    {
        public GPDbContext()
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server = localhost; Database = GiaPhaDbTest; Trusted_Connection = True;");
            }
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Parents> Parents { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Parents>()
                    .HasOne(x => x.Mother)
                    .WithMany(x => x.Mothers)
                    .HasForeignKey(x => x.MotherId)
                    .OnDelete(DeleteBehavior.ClientNoAction);

            modelBuilder.Entity<User>()
                    .HasOne(x => x.Parents)
                    .WithMany(x => x.Children)
                    .HasForeignKey(x => x.ParentsId)
                    .OnDelete(DeleteBehavior.ClientNoAction);

        }

    }
}
