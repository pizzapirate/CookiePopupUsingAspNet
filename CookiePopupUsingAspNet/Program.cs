internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllersWithViews();


        //adding the necessary cookie service handler
        builder.Services.AddAuthentication("CookiePopup").AddCookie("CookiePopup", options =>
        {
            options.Cookie.Name = "CookiePopup";
            options.ExpireTimeSpan = TimeSpan.FromDays(365);
            options.Cookie.MaxAge = TimeSpan.FromDays(365);
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

        //calls the authentication handler and instantiates it so that the middleware for authentication runs (to decrypt the cookies)
        app.UseAuthentication();

        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.Run();
    }
}