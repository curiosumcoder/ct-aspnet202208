using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//builder.Services.AddDistributedMemoryCache(); // Non-sticky
builder.Services.AddMemoryCache(); // Sticky
builder.Services.AddHttpContextAccessor();

builder.Services.AddResponseCaching();

//builder.Services.AddControllersWithViews();
var mvc = builder.Services.AddControllersWithViews(options =>
{
    options.CacheProfiles.Add("Basic", new CacheProfile()
    {
        Duration = 10
    });
    options.CacheProfiles.Add("NoCaching", new CacheProfile()
    {
        NoStore = true,
        Location = ResponseCacheLocation.None
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
