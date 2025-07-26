using Common;
using Data.Contracts;
using Data.Repositories;
using Data.Reprositories;
using DTO.CustomMapping;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.FileProviders;
using Service.Model;
using Service.Model.Contracts;
using Services.DataInitializer;
using WebFramework.Configuration;

var builder = WebApplication.CreateBuilder(args);

var siteSettings = builder.Configuration.GetSection(nameof(SiteSettings)).Get<SiteSettings>();
builder.Services.Configure<SiteSettings>(builder.Configuration.GetSection(nameof(SiteSettings)));
builder.Services.AddControllersWithViews(options => { options.Filters.Add(new AuthorizeFilter());});
builder.Services.AddDbContext(builder.Configuration);
builder.Services.AddCustomIdentity(siteSettings.IdentitySettings);
builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUploadedFileService, UploadedFileService>();

builder.Services.AddScoped<IDataInitializer, RoleDataInitializer>();
builder.Services.AddScoped<IDataInitializer, UserDataInitializer>();

builder.Services.InitializeAutoMapper();

var app = builder.Build();

await app.DataSeeder(app.Environment);
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

app.MapStaticAssets();
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}")
   .WithStaticAssets();
app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();



app.Run();