internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllersWithViews();


        //adding the necessary cookie service handler
        builder.Services.AddAuthentication("RequiredCookies").AddCookie("RequiredCookies", options =>
        {
            options.Cookie.Name = "RequiredCookies";
            options.ExpireTimeSpan = TimeSpan.FromDays(365);
            options.Cookie.MaxAge = TimeSpan.FromDays(365);
        });

        builder.Services.AddAuthentication("PersonalizationCookies").AddCookie("PersonalizationCookies", options =>
        {
            options.Cookie.Name = "PersonalizationCookies";
            options.ExpireTimeSpan = TimeSpan.FromDays(365);
            options.Cookie.MaxAge = TimeSpan.FromDays(365);
        });

        builder.Services.AddAuthentication("ThirdPartyCookies").AddCookie("ThirdPartyCookies", options =>
        {
            options.Cookie.Name = "ThirdPartyCookies";
            options.ExpireTimeSpan = TimeSpan.FromDays(365);
            options.Cookie.MaxAge = TimeSpan.FromDays(365);
        });
        builder.Services.AddAuthentication("UserRejectedAll").AddCookie("UserRejectedAll", options =>
        {
            options.Cookie.Name = "UserRejectedAll"; //not stating expiretimespan/maxage so that cookie is session only. 
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