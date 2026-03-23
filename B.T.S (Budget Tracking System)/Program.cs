namespace StudentBudgetTracker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services
            builder.Services.AddControllersWithViews();
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {   
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true; 
                options.Cookie.IsEssential = true;
            });

            var app = builder.Build();

            app.UseStaticFiles();
            app.UseRouting();
            app.UseSession();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Account}/{action=Login}/{id?}");

            app.Run();
        }
    }
}
