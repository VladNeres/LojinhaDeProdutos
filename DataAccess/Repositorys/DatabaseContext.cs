using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositorys
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> opt) : base(opt)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<SubCategoria>()
                .HasOne(subCategoria => subCategoria.Categoria)
                .WithMany(categoria => categoria.SubCategoria)
                .HasForeignKey(subCategoria => subCategoria.CategoriaId);

            builder.Entity<Produto>()
                .HasOne(produto => produto.Subcategoria)
                .WithMany(subCategoria => subCategoria.Produtos)
                .HasForeignKey(produto => produto.SubCategoriaId);

          
        }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<SubCategoria> SubCategorias { get; set; }
        public DbSet<Produto> Produtos { get; set; }
    }
}
