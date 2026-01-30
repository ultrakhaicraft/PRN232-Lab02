using AutoMapper;
using FUNewsManagementSystemRepository.Models;
using PRN232.FUNewsManagement.Services.DTO.Request;
using PRN232.FUNewsManagement.Services.DTO.Response;


namespace PRN232.FUNewsManagement.Services
{
	/// <summary>
	/// Mapper profile for AutoMapper configurations. Mainly to convert Business DTO to API DTO and vice versa.
	/// </summary>
	public class MapperProfile : Profile
	{
		public MapperProfile()
		{

			//Tag
			CreateMap<Tag, ViewTagDTO>();
			CreateMap<CreateTagDTO, Tag>();
			CreateMap<UpdateTagDTO, Tag>();

			//Category
			CreateMap<Category, ViewCategoryDTO>();
			CreateMap<CreateCategoryDTO, Category>();
			CreateMap<UpdateCategoryDTO, Category>();

			//News
			CreateMap<NewsArticle, ViewNewsArticleDTO>();
			CreateMap<CreateNewsArticleDTO, NewsArticle>();
			CreateMap<UpdateNewsArticleDTO, NewsArticle>();

			//Account
			CreateMap<SystemAccount, ViewAccountDTO>();
			CreateMap<UpdateAccountDTO, SystemAccount>();
			CreateMap<CreateAccountDTO, SystemAccount>();


			//Comment
			CreateMap<Comment, ViewCommentDTO>();
			CreateMap<CreatedCommentDTO, Comment>();
			CreateMap<UpdatedCommentDTO, Comment>();

		}
	}
}
