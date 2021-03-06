﻿using Api.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace Api
{
	public class Startup
	{
		private IConfiguration Configuration { get; }

		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddSingleton(db =>
				new MongoClient(Configuration["Token"]).GetDatabase(Configuration["DBName"]));

			services.AddScoped<BotsRepository>();
			services.AddScoped<EventsRepository>();
			services.AddScoped<InlineKeysRepository>();
			services.AddScoped<InlineUrlKeysRepository>();
			services.AddScoped<InterviewAnswersRepository>();
			services.AddScoped<InterviewsRepository>();
			services.AddScoped<TextMessageAnswersRepository>();
			services.AddScoped<UsersRepository>();
			services.AddScoped<SystemUserRepository>();

			services.AddSession();
			services.AddMvc();
		}

		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
			}

			app.UseStaticFiles();

			app.UseSession();
			
			app.UseMvc(routes =>
			{
				routes.MapRoute(
					name: "default",
					template: "{controller=Index}/{action=Index}");
			});
		}
	}
}