using blazorApp.Components;
using BlazingPizza.Data;
using BlazingPizza.Services;
using BlazingPizza;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;


var builder = WebApplication.CreateBuilder(args);
// Ajouter les variables d'environnement
builder.Configuration.AddEnvironmentVariables();
builder.Logging.AddConsole();

// Ajout de SignalR avec Redis comme backplane
// builder.Services.AddSignalR()
//     .AddStackExchangeRedis(options =>
//     {
//         var redisConnectionString = builder.Configuration.GetConnectionString("Redis");
//         Console.WriteLine("redisConnectionString :" + redisConnectionString);
//         if (redisConnectionString != null)
//         {
//             options.Configuration = StackExchange.Redis.ConfigurationOptions.Parse(redisConnectionString);
//         }
//         else
//         {
//             throw new InvalidOperationException("Redis connection string is not configured.");
//         }
//         options.Configuration.ClientName = "SignalRBackplane:";
//     });


builder.Logging.SetMinimumLevel(LogLevel.Debug);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// app.MapBlazorHub(); // Configure le hub Blazor pour les communications temps réel
// app.MapHub<PizzaOrderHub>("/orderHub"); // Route personnalisée pour un Hub SignalR (ex : gestion des commandes)
// app.MapFallbackToPage("/_Host");


//AddHttpClient permet à l’application d’accéder aux commandes HTTP. L’application utilise un HttpClient pour obtenir le JSON pour les pizzas spéciales
builder.Services.AddHttpClient();

builder.Services.AddHttpClient("MyController", client =>
{
    // Lire l'adresse de base à partir de la configuration
    var baseAddress = builder.Configuration.GetValue<string>("BaseAddress") ?? "http://localhost:5000";
    client.BaseAddress = new Uri(baseAddress);  // Définir la BaseAddress ici
    Console.WriteLine("BaseAddress :" + builder.Configuration.GetValue<string>("BaseAddress"));
});


//var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD");
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
// Configure le DbContext pour utiliser SQL Server
builder.Services.AddDbContext<PizzaStoreContext>(options => options.UseSqlServer(connectionString));
Console.WriteLine("connectionString :" + connectionString);

// inscrit le nouveau PizzaStoreContext et fournit le nom de fichier de la base de données SQLite
// builder.Services.AddSqlite<PizzaStoreContext>("Data Source=pizza.db");

builder.Services.AddSingleton<PizzaService>();
builder.Services.AddControllers(); 
builder.Services.AddScoped<OrderState>();

var app = builder.Build();

app.MapHub<OrderUpdatesHub>("/orderUpdatesHub");

// Initialize the database
var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();

if (app.Environment.IsDevelopment())
{
    Environment.SetEnvironmentVariable("INIT_DB","true");
}

var initDb = Environment.GetEnvironmentVariable("INIT_DB");
Console.WriteLine($"Environment: {builder.Environment.EnvironmentName}");

using (var scope = scopeFactory.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<PizzaStoreContext>();

    if (db.Database.EnsureCreated() && initDb?.ToLower() == "true")
    {
        SeedData.Initialize(db);
    }
}
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    Console.WriteLine("Production !!");
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else 
{
    Console.WriteLine("Développement !!");
}

app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();