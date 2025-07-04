using BookStore.IRepository;
using BookStore.Models;
using BookStore.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BookStore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

          
            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<Context>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("conn"));
            });


            builder.Services.AddIdentity<ApplicationUser, IdentityRole<int>>(option =>
            {
                option.Password.RequiredLength = 6;
                option.Password.RequireDigit = true;
                option.Password.RequireNonAlphanumeric = false;
                option.Password.RequireUppercase =true;
                option.Password.RequireLowercase = true;
            }).AddEntityFrameworkStores<Context>();

            builder.Services.AddScoped<IApplicationUserRepo, ApplicationUserRepo>();
            builder.Services.AddScoped<IBookRepo, BookRepo>();
            builder.Services.AddScoped<ICartItemRepo,CartItemRepo>();
            builder.Services.AddScoped<ICartRepo, CartRepo>();
            builder.Services.AddScoped<ICategoryRepo,CategoryRepo>();
          
            builder.Services.AddScoped<IOrderItemRepo, OrderItemRepo>();
            builder.Services.AddScoped<IOrderRepo,OrderRepo>();
            builder.Services.AddScoped<IPaymentRepo,PaymentRepo>();
            builder.Services.AddScoped<IReviewRepo,ReviewRepo>();
            builder.Services.AddScoped<IWishlistItemRepo, WishlistItemRepo>();
            builder.Services.AddScoped<IWishlistRepo, WishlistRepo>();

            var app = builder.Build();

          
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
