using Microsoft.Extensions.DependencyInjection;
using PRN232.FUNewsManagement.Repo;
using PRN232.FUNewsManagement.Repo.Interface;
using PRN232.FUNewsManagement.Repo.Repository;
using PRN232.FUNewsManagement.Services.Interface;
using PRN232.FUNewsManagement.Services.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN232.FUNewsManagement.Services.Utility
{
	public static class DependencyInjections
	{
		public static IServiceCollection AddInfrastructure(this IServiceCollection services)
		{
			services.AddScoped<IUnitOfWork, UnitOfWork>();
			services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
			services.AddScoped<ITagService, TagService>();
			services.AddScoped<INewsArticleService, NewsArticleService>();
			services.AddScoped<IAccountService, AccountService>();
			services.AddScoped<ICategoryService, CategoryService>();
			services.AddScoped<IAuthenticationService, AuthenticationService>();
			services.AddScoped<ICommentService, CommentService>();
			services.AddSingleton<TokenProvider>();
			return services;

		}
	}
}
