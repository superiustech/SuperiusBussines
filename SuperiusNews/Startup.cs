using Business.Services;
using Business.Middlewares;
using Business.Uteis;
using Domain.Interfaces;
using Domain.Uteis;
using Infra;
using Infra.Repositories;
using Infra.Repositories.Persistencia;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddDistributedMemoryCache();

        services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromMinutes(30);
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
        });

        services.AddSpaStaticFiles(configuration =>
        {
            configuration.RootPath = "ClientApp/build";
        });

        // JWT
        var jwtSettings = Configuration.GetSection("Jwt");
        var key = Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]);

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };
        });

        services.Configure<JwtSettings>(Configuration.GetSection(JwtSettings.SectionName));

        // Autorizações globais
        services.AddControllers(options =>
        {
            var policy = new AuthorizationPolicyBuilder()
                             .RequireAuthenticatedUser()
                             .Build();
            options.Filters.Add(new AuthorizeFilter(policy));
            options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
        });

        // Swagger só no dev
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
            c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Insira o token JWT no formato: Bearer {token}",
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                    },
                    Array.Empty<string>()
                }
            });
        });

        // Contexto Master
        services.AddDbContext<ApplicationDbContextMaster>(options =>
            options.UseNpgsql(Configuration.GetConnectionString("ConnectionMaster")));

        // Contexto por Tenant
        services.AddScoped<ApplicationDbContext>(provider =>
        {
            var tenantProvider = provider.GetRequiredService<ITenantProvider>();
            var tenantBase = tenantProvider.ConsultarTenantBase();
            var factory = provider.GetRequiredService<IRuntimeDbContextFactory<ApplicationDbContext>>();
            return factory.CreateDbContext(tenantBase);
        });

        #region Injeção de Dependência

        services.AddScoped<IRuntimeDbContextFactory<ApplicationDbContext>, RuntimeDbContextFactory>();

        services.AddScoped<ITenantProvider, TenantProvider>();
        services.AddScoped<IJwtTokenService, JwtTokenService>();

        services.AddScoped<IAutenticacao, AutenticacaoService>();
        services.AddScoped<IProduto, ProdutoService>();
        services.AddScoped<IEstoque, EstoqueService>();
        services.AddScoped<IUsuario, UsuarioService>();
        services.AddScoped<IRevendedor, RevendedorService>();
        services.AddScoped<IFuncionalidade, FuncionalidadeService>();
        services.AddScoped<IPermissao, PermissaoService>();
        services.AddScoped<IPerfil, PerfilService>();

        services.AddScoped<IProdutoRepository, ProdutoRepository>();
        services.AddScoped<IProdutoRepositorySQL, ProdutoRepositorySQL>();
        services.AddScoped<IEstoqueRepository, EstoqueRepository>();
        services.AddScoped<IUsuarioRepository, UsuarioRepository>();
        services.AddScoped<IRevendedorRepository, RevendedorRepository>();
        services.AddScoped<IAutenticacaoRepository, AutenticacaoRepository>();
        services.AddScoped<IFuncionalidadeRepository, FuncionalidadeRepository>();
        services.AddScoped<IPermissaoRepository, PermissaoRepository>();
        services.AddScoped<IPerfilRepository, PerfilRepository>();
        services.AddScoped<IEntidadeLeituraRepository, EntidadeLeituraRepository>();

        #endregion
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        // Middleware de erro e Swagger só no desenvolvimento
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
                c.RoutePrefix = "swagger";
            });
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseSpaStaticFiles();

        app.UseRouting();

        app.UseAuthentication();
        app.UseMiddleware<TenantMiddleware>();
        app.UseAuthorization();

        app.UseSession();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });

        app.UseSpa(spa =>
        {
            spa.Options.SourcePath = "ClientApp";
            if (env.IsDevelopment())
            {
                spa.UseReactDevelopmentServer(npmScript: "start");
            }
        });
    }
}
