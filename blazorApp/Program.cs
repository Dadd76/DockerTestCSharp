using blazorApp.Components;
using BlazingPizza.Data;
using BlazingPizza.Services;
using BlazingPizza;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
// Ajouter les variables d'environnement
builder.Configuration.AddEnvironmentVariables();


builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Debug);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

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

// Initialize the database
var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();

if (app.Environment.IsDevelopment())
{
    Environment.SetEnvironmentVariable("INIT_DB","true");
}

var initDb = Environment.GetEnvironmentVariable("INIT_DB");

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


