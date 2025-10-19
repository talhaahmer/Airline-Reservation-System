using airlines.Controllers;
using airlines.Models;
using airlines.Services;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
//extra
builder.Services.AddDbContext<AirlinesContext>();
builder.Services.AddScoped<ReservationService>();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(
                op =>
                {
                    op.LoginPath = "/cookies/Login";
                    op.AccessDeniedPath = "/cookies/Login";
                    op.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                }
                );

//end
var app = builder.Build();

//extra
var timer = new Timer(_ =>
{
    using (var scope = app.Services.CreateScope())
    {
        var service = scope.ServiceProvider.GetRequiredService<ReservationService>();
        service.AutoUnblockExpiredReservations();
    }
}, null, TimeSpan.Zero, TimeSpan.FromMinutes(15));


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
app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=cookies}/{action=Login}/{id?}");

app.Run();
