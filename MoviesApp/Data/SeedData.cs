using System;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MoviesApp.Models;

namespace MoviesApp.Data
{
	public static class SeedData
	{
		public static void Initialize(IServiceProvider serviceProvider)
		{
			using (var context = new MoviesContext(
				serviceProvider.GetRequiredService<
					DbContextOptions<MoviesContext>>()))
			{
				if (context.Movies.Any())
				{
					return;
				}

				context.Movies.AddRange(
					new Movie
					{
						Title = "When Harry Met Sally",
						ReleaseDate = DateTime.Parse("1989-2-12"),
						Genre = "Romantic Comedy",
						Price = 7.99M
					},


					new Movie
					{
						Title = "Ghostbusters ",
						ReleaseDate = DateTime.Parse("1984-3-13"),
						Genre = "Comedy",
						Price = 8.99M
					},

					new Movie
					{
						Title = "Ghostbusters 2",
						ReleaseDate = DateTime.Parse("1986-2-23"),
						Genre = "Comedy",
						Price = 9.99M
					},

					new Movie
					{
						Title = "Rio Bravo",
						ReleaseDate = DateTime.Parse("1959-4-15"),
						Genre = "Western",
						Price = 3.99M
					}
				);

				context.SaveChanges();
			}

			var userManager = serviceProvider.GetService<UserManager<ApplicationUser>>();
			var roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();

			if (!roleManager.RoleExistsAsync("Admin").Result)
				roleManager.CreateAsync(new IdentityRole { Name = "Admin" }).Wait();

			if (userManager.FindByEmailAsync("js@test.com").Result == null)
			{
				var user = new ApplicationUser
				{
					FirstName = "Root",
					LastName = "Admin",
					UserName = "js@test.com",
					Email = "js@test.com",
				};

				IdentityResult result = userManager.CreateAsync(user, "qwerty").Result;

				if (result.Succeeded)
					userManager.AddToRoleAsync(user, "Admin").Wait();
			}
		}
	}
}