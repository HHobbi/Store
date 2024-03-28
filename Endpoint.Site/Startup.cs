using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Store.Application.Interfaces;
using Store.Application.Interfaces.Contexts;
using Store.Application.Services.Users.Commands;
using Store.Application.Services.Users.Commands.RegisterUser;
using Store.Application.Services.Users.Commands.RemoveUser;
using Store.Application.Services.Users.Queries.GetRoles;
using Store.Application.Services.Users.Queries.GetUsers;
using Store.Application.Services.Users.Commands;
using Store.Persistance.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Store.Application.Services.Users.Commands.EditeUser;
using Bugeto_Store.Common.Roles;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Store.Application.Services.Users.Commands.UserLogin;
using Store.Application.Interfaces.FacadPatterns;
using Store.Application.Services.Products.FacadPattern;
using Store.Application.Services.Common.Queries.GetMenuItem;
using Store.Application.Services.Common.Queries.GetCategory;
using Store.Application.Services.Common.Commands.AddNewSlider;
using Store.Application.Services.Common.Queries.GetSlider;
using Store.Application.Services.HomePage.AddHomePageImages;
using Store.Application.Services.Common.Queries.GetHomePageImages;
using Store.Application.Services.Carts;
using Endpoint.Site.Utilities;
using Store.Application.Services.Finance.Commands;
using Store.Application.Services.Finance.Queries.GetRequestPayService;
using Store.Application.Services.Orders.Commands.AddNewOrder;
using Store.Application.Services.Orders.Queries.GetUserOrders;
using Store.Application.Services.Orders.Queries.GetOrdersForAdmin;
using Store.Application.Services.Fainances.Queries.GetRequestPayForAdmin;

namespace Endpoint.Site
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddAuthorization(options =>
            {
                options.AddPolicy(UserRoles.Admin, policy => policy.RequireRole(UserRoles.Admin));
                options.AddPolicy(UserRoles.Customer, policy => policy.RequireRole(UserRoles.Customer));
                options.AddPolicy(UserRoles.Operator, policy => policy.RequireRole(UserRoles.Operator));
            });

            services.AddAuthentication(options =>
            {
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            }).AddCookie(options =>
            {
                options.LoginPath = new PathString("/Authentication/Signin");
                options.ExpireTimeSpan = TimeSpan.FromMinutes(5.0);
                options.AccessDeniedPath = new PathString("/Authentication/Signin");
            });


            string contectionString = @"Data Source=.; Initial Catalog=Store; Integrated Security=True;";
            services.AddEntityFrameworkSqlServer().AddDbContext<DatabaseContext>(opttion => opttion.UseSqlServer(contectionString));
            
            services.AddControllersWithViews();


            services.AddScoped(typeof(IDatabaseContext), typeof(DatabaseContext));


            services.AddScoped(typeof(IGetUsersService), typeof(GetUsersService));
            services.AddScoped(typeof(IGetRolesService), typeof(GetRolesService));
            services.AddScoped(typeof(IRegisterUserService), typeof(  RegisterUserService));
            services.AddScoped(typeof(IRemoveUserService), typeof(RemoveUserService));
            services.AddScoped(typeof(IUserStatusChangeService), typeof(UserStatusChangeService));
            services.AddScoped(typeof(IEditUserService), typeof(EditUserService));
            services.AddScoped(typeof(IUserLoginService), typeof(UserLoginService));

            services.AddScoped(typeof(IProductFacad), typeof(ProductFacade)); 
            
            
            //---------------------------------------------

            services.AddScoped(typeof(IGetMenuItemService), typeof(GetMenuItemService));
            services.AddScoped(typeof(IGetCategoryService), typeof(GetCategoryService));
            services.AddScoped(typeof(IAddNewSliderService), typeof(AddNewSliderService));
            services.AddScoped(typeof(IGetSliderService), typeof(GetSliderService));
            services.AddScoped(typeof(IAddHomePageImagesService), typeof(AddHomePageImagesService));
            services.AddScoped(typeof(IGetHomePageImagesService), typeof(GetHomePageImagesService));
            services.AddScoped(typeof(ICartService), typeof(CartService));
            services.AddScoped(typeof(CookiMangment), typeof(CookiMangment));
            services.AddScoped(typeof(IAddRequestPayService), typeof(AddRequestPayService));
            services.AddScoped(typeof(IGetRequestPayService), typeof(GetRequestPayService));
            services.AddScoped(typeof(IAddNewOrderService), typeof(AddNewOrderService));
            services.AddScoped(typeof(IGetUsersOrdersService), typeof(GetUsersOrdersService));
            services.AddScoped(typeof(IGetOrdersForAdminSevice), typeof(GetOrdersForAdminSevice));
            services.AddScoped(typeof(IGetRequestPayForAdminService), typeof(GetRequestPayForAdminService));


            services.AddScoped(typeof(CookiMangment),typeof(CookiMangment));

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
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

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "areas",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
