using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using SGEI.Components;
using SGEI.Data;
using SGEI.Services;

var builder = WebApplication.CreateBuilder(args);

// --- 1. CONFIGURAÇÃO DE SERVIÇOS DO SISTEMA ---
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// --- 2. SERVIÇOS DE NEGÓCIO (Injeção de Dependência) ---
builder.Services.AddScoped<ClienteService>();
builder.Services.AddScoped<ProcedimentoService>();
builder.Services.AddScoped<AgendamentoService>();
builder.Services.AddScoped<ConfiguracaoService>();
builder.Services.AddScoped<AuthService>();

// --- 3. INFRAESTRUTURA E SEGURANÇA (Identity Profissional) ---
builder.Services.AddSingleton<UserSession>(); 

// CONFIGURAÇÃO DE AUTENTICAÇÃO COM REDIRECIONAMENTO CORRETO
builder.Services.AddAuthentication("Cookies")
    .AddCookie("Cookies", options =>
    {
        options.LoginPath = "/login";             // Define sua tela de login real
        options.AccessDeniedPath = "/acesso-negado"; // Define sua tela de erro real
        options.ExpireTimeSpan = TimeSpan.FromDays(7); // Mantém logado por 7 dias
    });

builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<AuthenticationStateProvider, SimpleAuthStateProvider>();

// --- 4. CONFIGURAÇÃO DO BANCO DE DADOS (SQL Server) ---
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// --- 5. ATUALIZAÇÃO AUTOMÁTICA DO BANCO (Docker/Dev) ---
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        context.Database.EnsureCreated(); 
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Erro ao garantir a criação do banco de dados.");
    }
}

// --- 6. CONFIGURAÇÃO DO PIPELINE HTTP ---
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();  