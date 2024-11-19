using AppTDS.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using AppTDS.Models;
using Microsoft.EntityFrameworkCore;
using AppTDS.Services;
using System.Net.Http;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    .Replace("localhost", "host.docker.internal")
    .Replace("Trusted_Connection=True;", "User ID=apptds;Password=apptds123;");

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

// Registrar MyDbContext no cont�iner de servi�os
builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<RegistrationSystem>();

// Adicionar HttpClient ao cont�iner de servi�os
var apiBaseUrl = builder.Configuration["ApiBaseUrl"] ?? "http://host.docker.internal:53879";
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(apiBaseUrl) });


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.MapControllers();

try
{
    // Testar a conex�o com o banco de dados durante a inicializa��o do aplicativo
    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<MyDbContext>();
        context.Database.OpenConnection();
        context.Database.CloseConnection();
        Console.WriteLine("Conex�o com o banco de dados bem-sucedida.");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Erro ao conectar ao banco de dados: {ex.Message}");
}

app.Run();
