using EFCore.Dominio;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EFCore.Repo { 
    public class ApplicationDBContext : DbContext
    {
        /*public ApplicationDBContext()
        {
            
        }*/

        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options) {}

        public DbSet<Processo> Processos { get; set; }
        public DbSet<Movimentacao> Movimentacoes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Processo>().HasMany(c => c.Movimentacao).WithOne(e => e.Processo);
        }
    }
}
