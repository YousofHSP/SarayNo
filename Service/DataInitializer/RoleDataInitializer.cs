using Data.Contracts;
using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Services.DataInitializer;

public class RoleDataInitializer(IRepository<Role> repository, RoleManager<Role> roleManager): IDataInitializer
{
    public async Task InitializerData()
    {
        var newRoles = new List<Role>()
        {
            new() { Name = "Admin", Title = "مدیرسیستم" },
            new() { Name = "Guest", Title = "مهمان" }
        };
        var roles = await roleManager.Roles.ToListAsync();
        newRoles = newRoles.Where(r => roles.All(i => i.Name != r.Name)).ToList();
        foreach (var role in newRoles)
            await roleManager.CreateAsync(role);
        
    }
    
}