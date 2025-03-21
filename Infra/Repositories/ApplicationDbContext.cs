using Microsoft.EntityFrameworkCore;
using Domain.Entities; // Substitua com seu namespace de entidades

namespace Infra
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<CWUsuario> Usuario { get; set; }
        public DbSet<CWProduto> Produto { get; set; }
        public DbSet<CWProdutoImagem> ProdutoImagem { get; set; }
        public DbSet<CWVariacao> Variacao { get; set; }
        public DbSet<CWVariacaoOpcao> VariacaoOpcao { get; set; }
        public DbSet<CWProdutoOpcaoVariacaoBase> ProdutoOpcaoVariacao { get; set; }
        public DbSet<CWUnidadeMedida> UnidadeMedida { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CWVariacao>().HasKey(v => v.nCdVariacao);
            modelBuilder.Entity<CWVariacaoOpcao>().HasKey(vo => vo.nCdVariacaoOpcao);
            modelBuilder.Entity<CWProduto>().HasKey(vo => vo.nCdProduto);
            modelBuilder.Entity<CWProdutoImagem>().HasKey(vo => vo.nCdImagem);
            modelBuilder.Entity<CWUnidadeMedida>().HasKey(vo => vo.nCdUnidadeMedida);
            modelBuilder.Entity<CWProdutoOpcaoVariacaoBase>().HasKey(vo => new { vo.nCdProduto, vo.nCdVariacaoOpcao, vo.nCdVariacao });

            modelBuilder.Entity<CWVariacao>().ToTable("VARIACAO");  
            modelBuilder.Entity<CWVariacaoOpcao>().ToTable("VARIACAO_OPCAO");  
            modelBuilder.Entity<CWProduto>().ToTable("PRODUTO");  
            modelBuilder.Entity<CWProdutoImagem>().ToTable("PRODUTO_IMAGEM");  
            modelBuilder.Entity<CWProdutoOpcaoVariacaoBase>().ToTable("PRODUTO_OPCAO_VARIACAO");  
            modelBuilder.Entity<CWUnidadeMedida>().ToTable("UNIDADE_MEDIDA");  

            modelBuilder.Entity<CWVariacao>().HasMany(v => v.VariacaoOpcoes)
                .WithMany(vo => vo.Variacoes)
                .UsingEntity<Dictionary<string, object>>(
                    "VARIACAO_OPCAO_VARIACAO",
                    j => j.HasOne<CWVariacaoOpcao>().WithMany().HasForeignKey("nCdVariacaoOpcao"),
                    j => j.HasOne<CWVariacao>().WithMany().HasForeignKey("nCdVariacao")
                );

        }
    }
}
