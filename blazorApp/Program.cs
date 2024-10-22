using blazorApp.Components;
using BlazingPizza.Data;
using BlazingPizza.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

//AddHttpClient permet à l’application d’accéder aux commandes HTTP. L’application utilise un HttpClient pour obtenir le JSON pour les pizzas spéciales
builder.Services.AddHttpClient(); 

// inscrit le nouveau PizzaStoreContext et fournit le nom de fichier de la base de données SQLite
builder.Services.AddSqlite<PizzaStoreContext>("Data Source=pizza.db");
builder.Services.AddSingleton<PizzaService>();
builder.Services.AddControllers(); 

builder.Services.AddScoped<OrderState>();


var app = builder.Build();

// Initialize the database
var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
using (var scope = scopeFactory.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<PizzaStoreContext>();
    if (db.Database.EnsureCreated())
    {
        SeedData.Initialize(db);
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();


