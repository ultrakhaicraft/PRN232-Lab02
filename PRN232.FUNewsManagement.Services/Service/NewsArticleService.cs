using FUNewsManagementSystemRepository.Models;
using Microsoft.EntityFrameworkCore;
using PRN232.FUNewsManagement.Repo.Interface;
using PRN232.FUNewsManagement.Services.DTO;
using PRN232.FUNewsManagement.Services.DTO.Request;
using PRN232.FUNewsManagement.Services.DTO.Response;
using PRN232.FUNewsManagement.Services.Interface;
using PRN232.FUNewsManagement.Services.Utility;
using PRN232.FUNewsManagement.Services.Utility.CustomException;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN232.FUNewsManagement.Services.Service
{
	public class NewsArticleService : INewsArticleService
	{


		private readonly IUnitOfWork _unitOfWork;
		private readonly IGenericRepository<NewsArticle> _newsArticleRepository;

		public NewsArticleService(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
			_newsArticleRepository = _unitOfWork.Repository<NewsArticle>();
		}	

		public async Task CreateNewsArticleAsync(CreateNewsArticleDTO dto)
		{
			try
			{
				NewsArticle newArticle = new NewsArticle
				{
					CategoryId = dto.CategoryId,
					CreatedById = dto.CreatedById,
					CreatedDate = DateTime.UtcNow,
					Headline = dto.Headline,
					NewsArticleId = dto.NewsArticleId,
					NewsContent = dto.NewsContent,
					NewsSource = dto.NewsSource,
					NewsStatus = dto.NewsStatus,
					NewsTitle = dto.NewsTitle,
				};

				await _newsArticleRepository.AddAsync(newArticle);
				await _unitOfWork.SaveChangesAsync();
			}
			catch (Exception)
			{

				throw;
			}
		}

		/// <summary>
		/// Method can perform search, sort, filter and pagination. Search are limited to one data field only.
		/// </summary>
		/// <returns></returns>
		public async Task<PagedResult<ViewNewsArticleDTO>> SearchNewsArticle(NewsPagedRequest request)
		{
			//Search + Filter (Filter IsActive)
			IQueryable<NewsArticle> news = _newsArticleRepository.Find(p =>
				(string.IsNullOrEmpty(request.SearchKeyword) ||
				p.NewsTitle.Contains(request.SearchKeyword)) &&
				(!request.CategoryId.HasValue || p.CategoryId == request.CategoryId));

			if (news == null || !news.Any())
			{
				throw new NotFoundException("No News Articles found");
			}

			var totalCount = await news.CountAsync();


			// Sorting when needed, sort by CategoryName alphabetically
			if (request.IsAscending.HasValue)
			{
				news = request.IsAscending.Value ? news.OrderBy(t => t.NewsTitle) :
					news.OrderByDescending(t => t.NewsTitle);
			}

			// Pagination
			var pagedData = await news
			   .Skip((request.Page - 1) * request.PageSize)
			   .Take(request.PageSize)
			   .ToListAsync();



			var items = pagedData.Select(entity =>
			{
				var dto = new ViewNewsArticleDTO();
				var fields = ParseFields(request.Fields);

				if (fields.Contains("newsArticleId")) dto.NewsArticleId = entity.NewsArticleId;
				if (fields.Contains("newsTitle")) dto.NewsTitle = entity.NewsTitle;
				if (fields.Contains("headline")) dto.Headline = entity.Headline;
				if (fields.Contains("createdDate")) dto.CreatedDate = entity.CreatedDate;
				if (fields.Contains("newsContent")) dto.NewsContent = entity.NewsContent;
				if (fields.Contains("newsSource")) dto.NewsSource = entity.NewsSource;
				if (fields.Contains("categoryId")) dto.CategoryId = entity.CategoryId;
				if (fields.Contains("newsStatus")) dto.NewsStatus = entity.NewsStatus;
				if (fields.Contains("createdById")) dto.CreatedById = entity.CreatedById;
				if (fields.Contains("updatedById")) dto.UpdatedById = entity.UpdatedById;
				if (fields.Contains("modifiedDate")) dto.ModifiedDate = entity.ModifiedDate;

				return dto;
			}).ToList();

			int totalPage = (int)Math.Ceiling((double)totalCount / request.PageSize);

			return new PagedResult<ViewNewsArticleDTO>(
				Page: request.Page,
				PageSize: request.PageSize,
				TotalPage: totalPage,
				TotalItems: totalCount,
				Items: items
			);
		}

		public async Task DeleteNewsArticleAsync(int articleId)
		{
			try
			{
				string stringArticleId = articleId.ToString();
				NewsArticle newsArticle = await _newsArticleRepository.GetByIdAsync(stringArticleId);

				if (newsArticle == null)
				{
					throw new NotFoundException("News Article not found");
				}

				_newsArticleRepository.Delete(newsArticle);
				await _unitOfWork.SaveChangesAsync();
			}
			catch (Exception)
			{

				throw;
			}
		}

		

		public async Task<ViewNewsArticleDTO> GetNewsArticleById(int articleId)
		{
			string stringArticleId = articleId.ToString();
			NewsArticle article = await _newsArticleRepository.GetByIdAsync(stringArticleId);

			if (article == null)
			{
				return null;
			}

			ViewNewsArticleDTO viewNewsArticleDTO = new ViewNewsArticleDTO
			{
				CategoryId = article.CategoryId,
				CreatedById = article.CreatedById,
				CreatedDate = article.CreatedDate,
				Headline = article.Headline,
				ModifiedDate = article.ModifiedDate,
				NewsArticleId = article.NewsArticleId,
				NewsContent = article.NewsContent,
				NewsSource = article.NewsSource,
				NewsStatus = article.NewsStatus,
				NewsTitle = article.NewsTitle,
				UpdatedById = article.UpdatedById
			};

			return viewNewsArticleDTO;
		}

		public async Task UpdateNewsArticleAsync(UpdateNewsArticleDTO dto, int articleId)
		{
			try
			{
				string stringArticleId = articleId.ToString();
				NewsArticle article = await _newsArticleRepository.FirstOrDefaultAsync(p=>p.NewsArticleId==stringArticleId);

				if (article == null)
				{
					throw new NotFoundException("News Article not found");
				}

				article.CategoryId = dto.CategoryId;
				article.Headline = dto.Headline;
				article.ModifiedDate = DateTime.UtcNow;
				article.NewsContent = dto.NewsContent;
				article.NewsSource = dto.NewsSource;
				article.NewsStatus = dto.NewsStatus;
				article.NewsTitle = dto.NewsTitle;
				article.UpdatedById = dto.UpdatedById;

				_newsArticleRepository.Update(article);
				await _unitOfWork.SaveChangesAsync();
			}
			catch (Exception)
			{

				throw;
			}
		}

		private HashSet<string> ParseFields(string? fields)
		{
			if (string.IsNullOrWhiteSpace(fields))
			{
				// Return all fields by default
				return new HashSet<string>(StringComparer.OrdinalIgnoreCase)
				{
					"newsArticleId", "newsTitle", "headline", "createdDate", "newsContent", "newsSource", 
					"categoryId", "newsStatus", "createdById", "updatedById", "modifiedDate"
				};
			}

			return new HashSet<string>(
				fields.Split(',').Select(f => f.Trim()),
				StringComparer.OrdinalIgnoreCase
			);
		}
	}
}
