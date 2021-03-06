using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DICOMService.Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DICOMService
{
   public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
			AppUser.ConnectionString = configuration.GetConnectionString("DbConnection");
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddControllersWithViews();
			 services.Configure<IISServerOptions>(options =>
             {
               options.AllowSynchronousIO = true;
              });
			services.Configure<CookiePolicyOptions>(options =>
			{
				// This lambda determines whether user consent for non-essential cookies is needed for a given request.
				options.CheckConsentNeeded = context => true;
				options.MinimumSameSitePolicy = SameSiteMode.None;
			});
            services.AddCors(O=>O.AddPolicy("MyPolicy",builder=>
			                     {
									 builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
								 }));
							 
			services.AddSingleton<IHttpContextAccessor,HttpContextAccessor>();
			int sessiontimeout = Convert.ToInt32(Configuration["ApplicationSettings:SessionTimeOutInMinutes"]);
		    
			services.AddSession(options =>
			{
				options.IdleTimeout = TimeSpan.FromMinutes(sessiontimeout);
				options.Cookie.IsEssential = true;
			});
			services.Configure<CookiePolicyOptions>(options =>
			{
				options.CheckConsentNeeded = context => false;
				options.MinimumSameSitePolicy = SameSiteMode.None;
			});
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
            AppUser.WebRootPath = env.WebRootPath;
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseSession();
            app.UseRouting();
            app.UseCors("MyPolicy");			
            app.UseEndpoints(endpoints =>
            {
				
				endpoints.MapControllers();
			    endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Account}/{action=LogIn}/{id?}");
            });

		}
	}
}
