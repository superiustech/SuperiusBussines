using Business.Middlewares;
using Business.Services;
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

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// ==================================================================
// 1. CORS CONFIG
// ==================================================================
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy
            .WithOrigins(
                "http://localhost:3000",
                "http://localhost",
                "http://127.0.0.1"
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

// ==================================================================
// 2. SESSÃO E CACHE
// ==================================================================
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// ==================================================================
// 3. JWT CONFIG
// ==================================================================
var jwtSettings = configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]);

builder.Services.AddAuthentication(options =>
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

builder.Services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));

// ==================================================================
// 4. CONTROLLERS E AUTH GLOBAL
// ==================================================================
builder.Services.AddControllers(options =>
{
    var policy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
    options.Filters.Add(new AuthorizeFilter(policy));
    options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
});

builder.Services.AddHttpContextAccessor();

// ==================================================================
// 5. SWAGGER CONFIG
// ==================================================================
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "SUPERIUS API", Version = "v1" });
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

// ==================================================================
// 6. DB CONTEXTS
// ==================================================================
builder.Services.AddDbContext<ApplicationDbContextMaster>(options =>
    options.UseNpgsql(configuration.GetConnectionString("ConnectionMaster")));

builder.Services.AddScoped<ApplicationDbContext>(provider =>
{
    var tenantProvider = provider.GetRequiredService<ITenantProvider>();
    var tenantBase = tenantProvider.ConsultarTenantBase();
    var factory = provider.GetRequiredService<IRuntimeDbContextFactory<ApplicationDbContext>>();
    return factory.CreateDbContext(tenantBase);
});

// ==================================================================
// 7. DEPENDENCY INJECTION
// ==================================================================
builder.Services.AddScoped<IRuntimeDbContextFactory<ApplicationDbContext>, RuntimeDbContextFactory>();
builder.Services.AddScoped<ITenantProvider, TenantProvider>();
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();

builder.Services.AddScoped<IAutenticacao, AutenticacaoService>();
builder.Services.AddScoped<IProduto, ProdutoService>();
builder.Services.AddScoped<IEstoque, EstoqueService>();
builder.Services.AddScoped<IUsuario, UsuarioService>();
builder.Services.AddScoped<IRevendedor, RevendedorService>();
builder.Services.AddScoped<IFuncionalidade, FuncionalidadeService>();
builder.Services.AddScoped<IPermissao, PermissaoService>();
builder.Services.AddScoped<IPerfil, PerfilService>();
builder.Services.AddScoped<IDashboard, DashboardService>();

builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();
builder.Services.AddScoped<IProdutoRepositorySQL, ProdutoRepositorySQL>();
builder.Services.AddScoped<IEstoqueRepository, EstoqueRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IRevendedorRepository, RevendedorRepository>();
builder.Services.AddScoped<IAutenticacaoRepository, AutenticacaoRepository>();
builder.Services.AddScoped<IFuncionalidadeRepository, FuncionalidadeRepository>();
builder.Services.AddScoped<IPermissaoRepository, PermissaoRepository>();
builder.Services.AddScoped<IPerfilRepository, PerfilRepository>();
builder.Services.AddScoped<IEntidadeLeituraRepository, EntidadeLeituraRepository>();
builder.Services.AddScoped<IDashboardRepository, DashboardRepository>();

// ==================================================================
// BUILD APP
// ==================================================================
var app = builder.Build();

// ==================================================================
// 8. APLICAR MIGRAÇÕES AUTOMÁTICAS 
// ==================================================================
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var contextMaster = services.GetRequiredService<ApplicationDbContext>();
        contextMaster.Database.Migrate();
        Console.WriteLine("Migrações DEFAULT aplicadas com sucesso!");
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Erro ao aplicar migrações do banco de dados DEFAULT.");
    }
}

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var contextMaster = services.GetRequiredService<ApplicationDbContextMaster>();
        contextMaster.Database.Migrate();
        Console.WriteLine("Migrações MASTER aplicadas com sucesso!");
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Erro ao aplicar migrações do banco de dados MASTER.");
    }
}

// ==================================================================
// 9. MIDDLEWARE PIPELINE
// ==================================================================
app.UseCors("AllowFrontend");

// Swagger sempre ativo (útil em containers)
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "SUPERIUS API v1");
    c.RoutePrefix = string.Empty; // Swagger na raiz
});

// app.UseHttpsRedirection(); 
app.UseSession();
app.UseAuthentication();
app.UseMiddleware<TenantMiddleware>();
app.UseAuthorization();
app.UseMiddleware<FuncionalidadeMiddleware>();

app.MapControllers();

app.Run();
