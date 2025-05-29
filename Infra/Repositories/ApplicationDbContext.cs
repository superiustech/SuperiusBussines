using Microsoft.EntityFrameworkCore;
using Domain.Entities; 

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
        public DbSet<CWEstoqueProdutoHistorico> EstoqueProdutoHistorico { get; set; }
        public DbSet<CWRevendedor> Revendedor { get; set; }
        public DbSet<CWRevendedorTipo> RevendedorTipo { get; set; }
        public DbSet<CWFuncionalidade> Funcionalidade { get; set; }
        public DbSet<CWFuncionalidadePermissao> FuncionalidadePermissao { get; set; }
        public DbSet<CWPermissao> Permissao { get; set; }
        public DbSet<CWPermissaoPerfil> PermissaoPerfil { get; set; }
        public DbSet<CWPerfil> Perfil { get; set; }
        public DbSet<CWPerfilUsuario> PerfilUsuario { get; set; }
        public DbSet<CWRevendedorUsuario> RevendedorUsuario { get; set; }

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

            modelBuilder.Entity<CWEstoqueProdutoHistorico>(entity =>
            {
                entity.HasKey(e => e.nCdEstoqueProdutoHistorico);
                entity.ToTable("ESTOQUE_PRODUTO_HISTORICO");

                entity.Property(e => e.nCdEstoqueProdutoHistorico).ValueGeneratedOnAdd(); 
                entity.Property(e => e.nCdEstoque).IsRequired();
                entity.Property(e => e.nCdEstoqueDestino).IsRequired();
                entity.Property(e => e.nCdProduto).IsRequired();
                entity.Property(e => e.tDtMovimentacao).IsRequired();
                entity.Property(e => e.tDtMovimentacao).HasDefaultValueSql("CURRENT_TIMESTAMP");
                entity.Property(e => e.dQtMovimentada).IsRequired();
                entity.Property(e => e.nTipoMovimentacao).IsRequired().HasConversion<int>();
                entity.Property(e => e.sDsObservacao).HasMaxLength(500); 

                entity.HasOne<CWEstoqueProduto>()
                .WithMany(p => p.Historicos)
                .HasForeignKey(e => new { e.nCdEstoque, e.nCdProduto })
                .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Produto)
               .WithMany()
               .HasForeignKey(e => e.nCdProduto)
               .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.EstoqueOrigem)
                .WithMany()
                .HasForeignKey(e => e.nCdEstoque)
                .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.EstoqueDestino)
                .WithMany()
                .HasForeignKey(e => e.nCdEstoqueDestino)
                .OnDelete(DeleteBehavior.Restrict);
            });

            #endregion

            #region REVENDEDOR 

            modelBuilder.Entity<CWRevendedor>().HasKey(r => r.nCdRevendedor);
            modelBuilder.Entity<CWRevendedorTipo>().HasKey(t => t.nCdTipoRevendedor);

            modelBuilder.Entity<CWRevendedor>().HasOne(r => r.Estoque).WithMany().HasForeignKey(r => r.nCdEstoque).OnDelete(DeleteBehavior.SetNull);
            modelBuilder.Entity<CWRevendedor>().HasOne(r => r.Tipo).WithMany().HasForeignKey(r => r.nCdTipoRevendedor).OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<CWRevendedor>().ToTable("REVENDEDOR");
            modelBuilder.Entity<CWRevendedorTipo>().ToTable("REVENDEDOR_TIPO");

            #endregion

            #region FUNCIONALIDAE - PERMISSAO - PERFIL

            #region FUNCIONALIDADE 
            modelBuilder.Entity<CWFuncionalidade>(entity =>
            {
                entity.ToTable("FUNCIONALIDADE");
                entity.HasKey(e => e.nCdFuncionalidade);
                entity.Property(e => e.nCdFuncionalidade).ValueGeneratedOnAdd();
                entity.Property(e => e.sNmFuncionalidade).IsRequired();
            });

            modelBuilder.Entity<CWFuncionalidadePermissao>(entity =>
            {
                entity.ToTable("FUNCIONALIDADE_PERMISSAO");
                entity.HasKey(e => new { e.nCdFuncionalidade, e.nCdPermissao });
                entity.HasOne(e => e.Funcionalidade).WithMany().HasForeignKey(e => e.nCdFuncionalidade);
                entity.HasOne(e => e.Permissao).WithMany().HasForeignKey(e => e.nCdPermissao);
            });
            #endregion

            #region PERMISSAO

            modelBuilder.Entity<CWPermissao>(entity =>
            {
                entity.ToTable("PERMISSAO");
                entity.HasKey(e => e.nCdPermissao);
                entity.Property(e => e.nCdPermissao).ValueGeneratedOnAdd();
                entity.Property(e => e.sNmPermissao).IsRequired();
            });

            modelBuilder.Entity<CWPermissaoPerfil>(entity =>
            {
                entity.ToTable("PERMISSAO_PERFIL");
                entity.HasKey(e => new {e.nCdPermissao , e.nCdPerfil});
                entity.HasOne(e => e.Permissao).WithMany().HasForeignKey(e => e.nCdPermissao);
                entity.HasOne(e => e.Perfil).WithMany().HasForeignKey(e => e.nCdPerfil);

            });
            #endregion

            #region PERFIL

            modelBuilder.Entity<CWPerfil>(entity =>
            {
                entity.ToTable("PERFIL");
                entity.HasKey(e => e.nCdPerfil);
                entity.Property(e => e.nCdPerfil).ValueGeneratedOnAdd();
                entity.Property(e => e.sNmPerfil).IsRequired();
            });

            modelBuilder.Entity<CWPerfilUsuario>(entity =>
            {
                entity.ToTable("PERFIL_USUARIO");
                entity.HasKey(e => new { e.nCdPerfil, e.sCdUsuario });
                entity.HasOne(e => e.Perfil).WithMany().HasForeignKey(e => e.nCdPerfil);
                entity.HasOne(e => e.Usuario).WithMany().HasForeignKey(e => e.sCdUsuario);
            });
            #endregion

            #endregion

            #region USUARIO

            modelBuilder.Entity<CWUsuario>(entity =>
            {
                entity.ToTable("USUARIO");
                entity.HasKey(e => e.sCdUsuario);
                entity.Property(e => e.sCdUsuario).HasColumnName("sCdUsuario").IsRequired();
                entity.Property(e => e.sNmUsuario).HasColumnName("sNmUsuario").IsRequired();
                entity.Property(e => e.sSenha).HasColumnName("sSenha").IsRequired();
                entity.Property(e => e.sEmail).HasColumnName("sEmail");
                entity.HasMany(e => e.Perfis).WithOne(e => e.Usuario).HasForeignKey(e => e.sCdUsuario);
            });
            #endregion

            #region REVENDEDOR (EMPRESA) - USUARIO

            modelBuilder.Entity<CWRevendedorUsuario>().ToTable("REVENDEDOR_USUARIO").HasKey(p => new { p.nCdRevendedor, p.sCdUsuario });
            modelBuilder.Entity<CWRevendedorUsuario>().HasOne(ep => ep.Revendedor).WithMany(e => e.Usuarios).HasForeignKey(ep => ep.nCdRevendedor);

            #endregion
        }
    }
}
