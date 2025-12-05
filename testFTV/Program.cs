using testFTV.Services;

var builder = WebApplication.CreateBuilder(args);

// --------------------------------------
// Dependency Injection
// --------------------------------------
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<P_APIInfo>();
builder.Services.AddHttpClient<F_httpPost>();
builder.Services.AddScoped<HomeService>();
builder.Services.AddScoped<FooterService>();
builder.Services.AddSingleton<Now_date>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

// --------------------------------------
// Middlewares
// --------------------------------------
if (!app.Environment.IsDevelopment())
{
  app.UseExceptionHandler("/Home/Error");
  app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.UseStaticFiles();


// --------------------------------------
// Default Route
// --------------------------------------
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
