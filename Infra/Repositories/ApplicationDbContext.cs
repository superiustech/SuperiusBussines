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
        public DbSet<CWProdutoOpcaoVariacao> ProdutoOpcaoVariacao { get; set; }
        public DbSet<CWUnidadeMedida> UnidadeMedida { get; set; }
        public DbSet<CWEstoque> Estoque { get; set; }
        public DbSet<CWEstoqueProduto> EstoqueProduto { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CWVariacao>().HasKey(v => v.nCdVariacao);
            modelBuilder.Entity<CWVariacaoOpcao>().HasKey(vo => vo.nCdVariacaoOpcao);
            modelBuilder.Entity<CWProduto>().HasKey(vo => vo.nCdProduto);
            modelBuilder.Entity<CWEstoque>().HasKey(vo => vo.nCdEstoque);
            modelBuilder.Entity<CWProdutoImagem>().HasKey(vo => vo.nCdImagem);
            modelBuilder.Entity<CWUnidadeMedida>().HasKey(vo => vo.nCdUnidadeMedida);

            modelBuilder.Entity<CWVariacao>().ToTable("VARIACAO");  
            modelBuilder.Entity<CWVariacaoOpcao>().ToTable("VARIACAO_OPCAO");  
            modelBuilder.Entity<CWEstoque>().ToTable("ESTOQUE");  
            modelBuilder.Entity<CWProduto>().ToTable("PRODUTO");  
            modelBuilder.Entity<CWProdutoImagem>().ToTable("PRODUTO_IMAGEM");
            modelBuilder.Entity<CWUnidadeMedida>().ToTable("UNIDADE_MEDIDA");

            modelBuilder.Entity<CWProdutoOpcaoVariacao>().ToTable("PRODUTO_OPCAO_VARIACAO").HasKey(p => new { p.nCdProduto, p.nCdVariacaoOpcao, p.nCdVariacao });
            modelBuilder.Entity<CWEstoqueProduto>().ToTable("ESTOQUE_PRODUTO").HasKey(p => new { p.nCdEstoque, p.nCdProduto});

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
