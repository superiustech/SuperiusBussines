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
        public DbSet<CWRevendedor> Revendedor { get; set; }
        public DbSet<CWRevendedorTipo> RevendedorTipo { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region PRODUTO 

            modelBuilder.Entity<CWProduto>().HasOne(p => p.UnidadeMedida).WithMany().HasForeignKey(p => p.nCdUnidadeMedida).IsRequired();

            modelBuilder.Entity<CWProdutoImagem>().HasOne<CWProduto>().WithMany().HasForeignKey(p => p.nCdProduto).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CWProdutoOpcaoVariacao>().HasKey(x => new { x.nCdProduto, x.nCdVariacaoOpcao, x.nCdVariacao });

            modelBuilder.Entity<CWProdutoOpcaoVariacao>().HasOne<CWProduto>().WithMany().HasForeignKey(p => p.nCdProduto).IsRequired(false).OnDelete(DeleteBehavior.Cascade); ;          
            modelBuilder.Entity<CWProdutoOpcaoVariacao>().HasOne<CWVariacaoOpcao>().WithMany().HasForeignKey(p => p.nCdVariacaoOpcao).IsRequired();
            modelBuilder.Entity<CWProdutoOpcaoVariacao>().HasOne<CWVariacao>().WithMany().HasForeignKey(p => p.nCdVariacao).IsRequired();

            modelBuilder.Entity<CWProduto>().ToTable("PRODUTO");
            modelBuilder.Entity<CWVariacao>().ToTable("VARIACAO");
            modelBuilder.Entity<CWVariacaoOpcao>().ToTable("VARIACAO_OPCAO");
            modelBuilder.Entity<CWUnidadeMedida>().ToTable("UNIDADE_MEDIDA");
            modelBuilder.Entity<CWProdutoImagem>().ToTable("PRODUTO_IMAGEM");



            modelBuilder.Entity<CWVariacao>().HasMany(v => v.VariacaoOpcoes)
                .WithMany(vo => vo.Variacoes)
                .UsingEntity<Dictionary<string, object>>("VARIACAO_OPCAO_VARIACAO",
                    j => j.HasOne<CWVariacaoOpcao>().WithMany().HasForeignKey("nCdVariacaoOpcao"),
                    j => j.HasOne<CWVariacao>().WithMany().HasForeignKey("nCdVariacao"));


            #endregion

            #region ESTOQUE 
            
            modelBuilder.Entity<CWEstoque>().ToTable("ESTOQUE");
            modelBuilder.Entity<CWEstoqueProduto>().ToTable("ESTOQUE_PRODUTO").HasKey(p => new { p.nCdEstoque, p.nCdProduto });
            modelBuilder.Entity<CWEstoqueProduto>().HasOne(ep => ep.Estoque).WithMany(e => e.Produtos).HasForeignKey(ep => ep.nCdEstoque);

            #endregion

            #region REVENDEDOR 

            modelBuilder.Entity<CWRevendedor>().HasKey(r => r.nCdRevendedor);
            modelBuilder.Entity<CWRevendedorTipo>().HasKey(t => t.nCdTipoRevendedor);

            modelBuilder.Entity<CWRevendedor>().HasOne(r => r.Estoque).WithMany().HasForeignKey(r => r.nCdEstoque).OnDelete(DeleteBehavior.SetNull);
            modelBuilder.Entity<CWRevendedor>().HasOne(r => r.Tipo).WithMany().HasForeignKey(r => r.nCdTipoRevendedor).OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<CWRevendedor>().ToTable("REVENDEDOR");
            modelBuilder.Entity<CWRevendedorTipo>().ToTable("REVENDEDOR_TIPO");

            #endregion
        }
    }
}
