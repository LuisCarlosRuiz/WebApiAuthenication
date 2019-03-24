using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using WebApiPaises.Models;

namespace WebApiPaises
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
			services.AddDbContext<AplicationDbContext>(options =>
			options.UseSqlServer(Configuration.GetConnectionString("defaultConnection")));

			services.AddIdentity<AplicationUser, IdentityRole>()
				.AddEntityFrameworkStores<AplicationDbContext>()
				.AddDefaultTokenProviders();

			services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
				.AddJwtBearer(options =>
				 options.TokenValidationParameters = new TokenValidationParameters
				 {
					 ValidateIssuer = true,
					 ValidateAudience = true,
					 ValidateLifetime = true,
					 ValidateIssuerSigningKey = true,
					 ValidIssuer = "yourdomain.com",
					 ValidAudience = "yourdomain.com",
					 IssuerSigningKey = new SymmetricSecurityKey(
					Encoding.UTF8.GetBytes(Configuration["mykeytoken"])),
					 ClockSkew = TimeSpan.Zero
				 });

			services.AddMvc().AddJsonOptions(ConfigureJSon);
		}

		private void ConfigureJSon(MvcJsonOptions obj)
		{
			obj.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			app.UseAuthentication();

			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseMvc();
		}
	}
}
