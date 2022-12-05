using EduHome.DAL;
using EduHome.DAL.Entities;
using EduHome.Data;
using EduHome.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace EduHome
{
    public class Program
    {
        public async static Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddSession(x => x.IdleTimeout = TimeSpan.FromMinutes(10));
            builder.Services.AddDbContext<AppDbContext>(
               opt => opt.UseSqlServer(builder.Configuration
               .GetConnectionString("DefaultConnection")));
            ModelBuilder modelBuilder = new();
            modelBuilder.Entity<Setting>().HasIndex(x => x.Key).IsUnique();
            builder.Services.AddScoped<LayoutService>();

            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
                    builder =>
                    {
                        builder.MigrationsAssembly("EduHome");
                    });
            });

            builder.Services.AddIdentity<User, IdentityRole>(options =>
            {
                options.Lockout.MaxFailedAccessAttempts = 2;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);

                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;

                options.User.RequireUniqueEmail = true;
            }).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();



            Constans.RootPath = builder.Environment.WebRootPath;
            Constans.AppPath = Path.Combine(Constans.RootPath, "assets", "img");
            Constans.EventPath = Path.Combine(Constans.RootPath, "assets", "img", "event");
            Constans.TeacherPath = Path.Combine(Constans.RootPath, "assets", "img", "teacher");
            Constans.BlogPath = Path.Combine(Constans.RootPath, "assets", "img", "blog");
            Constans.CoursePath = Path.Combine(Constans.RootPath, "assets", "img", "course");
            Constans.SliderPath = Path.Combine(Constans.RootPath, "assets", "img", "slider");
            Constans.SpeakerPath = Path.Combine(Constans.RootPath, "assets", "img", "speaker");
            Constans.SettingPath = Path.Combine(Constans.RootPath, "assets", "img", "setting");


            var app = builder.Build();
            app.UseStatusCodePagesWithReExecute("/ErrorPage/Error1", "?code={0}");


            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                var dataInit = new DataInitializer(dbContext, userManager, roleManager);
                await dataInit.SeedData();
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                  name: "areas",
                  pattern: "{area:exists}/{controller=dashboard}/{action=Index}/{id?}"
                );

                endpoints.MapControllerRoute(
                  name: "default",
                  pattern: "{controller=Home}/{action=Index}/{id?}"
                );
            });

            await app.RunAsync();
        }
    }
}