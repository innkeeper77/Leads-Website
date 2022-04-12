using LeadsData.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Website services:
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<LeadsService>();
// API services:
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Configure API
app.UseHttpsRedirection();
app.MapControllers();

app.Run();
