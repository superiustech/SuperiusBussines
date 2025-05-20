using Business.Services;
using Infra.Repositories;
using Domain.Interfaces;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.EntityFrameworkCore;
using Infra;
using Infra.Repositories.Persistencia;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Business.Middlewares;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Business.Uteis;
using Domain.Uteis;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllersWithViews();
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
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });

        services.AddScoped<ApplicationDbContext>(provider =>
        {
            var tenantProvider = provider.GetRequiredService<ITenantProvider>();
            var tenantId = tenantProvider.ConsultarTenantID();
            var tentantBase = tenantProvider.ConsultarTenantBase();
            //if (string.IsNullOrEmpty(tenantId)) throw new Exception("Tenant não identificado");
            var factory = provider.GetRequiredService<IRuntimeDbContextFactory<ApplicationDbContext>>();
            return factory.CreateDbContext(tentantBase);
        });

        services.AddDbContext<ApplicationDbContextMaster>(options => options.UseNpgsql(Configuration.GetConnectionString("ConnectionMaster")));
        services.AddHttpContextAccessor();
        services.AddDistributedMemoryCache();

        services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromMinutes(30);
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
        });

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

        services.AddControllers(options =>
        {
            var policy = new AuthorizationPolicyBuilder()
                             .RequireAuthenticatedUser()
                             .Build();
            options.Filters.Add(new AuthorizeFilter(policy));

            options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
        });

        services.AddSwaggerGen();
        services.AddSpaStaticFiles(configuration =>
        {
            configuration.RootPath = "ClientApp/build";
        });


        services.Configure<JwtSettings>(Configuration.GetSection(JwtSettings.SectionName));

        #region Injeção de dependência

        services.AddScoped<IRuntimeDbContextFactory<ApplicationDbContext>, RuntimeDbContextFactory>();

        services.AddScoped<IProduto, ProdutoService>();
        services.AddScoped<IEstoque, EstoqueService>();
        services.AddScoped<IUsuario, UsuarioService>();
        services.AddScoped<IRevendedor, RevendedorService>();
        services.AddScoped<ITenantProvider, TenantProvider>();
        services.AddScoped<IAutenticacao, AutenticacaoService>();
        services.AddScoped<IJwtTokenService, JwtTokenService>();
        services.AddScoped<IFuncionalidade, FuncionalidadeService>();
        services.AddScoped<IPermissao, PermissaoService>();
        services.AddScoped<IPerfil, PerfilService>();

        services.AddScoped<IProdutoRepository, ProdutoRepository>();
        services.AddScoped<IProdutoRepositorySQL, ProdutoRepositorySQL>();
        services.AddScoped<IUsuarioRepository, UsuarioRepository>();
        services.AddScoped<IEstoqueRepository, EstoqueRepository>();
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

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();

        app.UseAuthentication();
        app.UseMiddleware<TenantMiddleware>();
        app.UseAuthorization();

        app.UseSession();

        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

        app.UseSpa(spa =>
        {
            spa.Options.SourcePath = "ClientApp";
            if (env.IsDevelopment()) { spa.UseReactDevelopmentServer(npmScript: "start"); }
        });
    }
} 