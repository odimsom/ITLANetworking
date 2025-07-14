using ITLANetworking.Core.Application;
using ITLANetworking.Infrastructure.Identity;
using ITLANetworking.Infrastructure.Identity.Service.Interfaces;
using ITLANetworking.Infrastructure.Persistence;
using ITLANetworking.Infrastructure.Persistence.Seeds;
using ITLANetworking.Infrastructure.Shared;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(3000);
});

// Add services to the container.
builder.Services.AddApplicationLayer();
builder.Services.AddIdentityInfrastructure(builder.Configuration);
builder.Services.AddPersistenceInfrastructure(builder.Configuration);
builder.Services.AddSharedInfrastructure(builder.Configuration);

builder.Services.AddControllersWithViews();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var identitySeeder = scope.ServiceProvider.GetRequiredService<IIdentitySeedService>();
    await identitySeeder.SeedAllAsync();

    var domainSyncService = scope.ServiceProvider.GetRequiredService<IdentityToDomainUserSyncService>();
    await domainSyncService.SyncAsync();
}


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

//app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
