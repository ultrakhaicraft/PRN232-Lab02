using AutoMapper;
using FUNewsManagementSystemRepository.Models;
using Microsoft.EntityFrameworkCore;
using PRN232.FUNewsManagement.Repo.Enum;
using PRN232.FUNewsManagement.Repo.Interface;
using PRN232.FUNewsManagement.Services.DTO;
using PRN232.FUNewsManagement.Services.DTO.Request;
using PRN232.FUNewsManagement.Services.DTO.Response;
using PRN232.FUNewsManagement.Services.Interface;
using PRN232.FUNewsManagement.Services.Utility.CustomException;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN232.FUNewsManagement.Services.Service
{
	public class CommentService : ICommentService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IGenericRepository<Comment> _commentRepository;
		private readonly IGenericRepository<NewsArticle> _newsArticleRepository;
		private readonly IGenericRepository<SystemAccount> _accountRepository;
		private readonly IMapper _mapper;

		public CommentService(IUnitOfWork unitOfWork, IGenericRepository<Comment> commentRepository, IGenericRepository<NewsArticle> newsArticleRepository, IGenericRepository<SystemAccount> accountRepository, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_commentRepository = commentRepository;
			_newsArticleRepository = newsArticleRepository;
			_accountRepository = accountRepository;
			_mapper = mapper;
		}




		/// <summary>
		/// Method can perform search, sort, filter and pagination. Search are limited to one data field only.
		/// </summary>
		/// <returns></returns>
		public async Task<PagedResult<ViewCommentDTO>> SearchComment(CommentPagedRequest request)
		{
			//Search + Filter
			// Start with all comments
			IQueryable<Comment> comments = _commentRepository.GetQueryable();

			// Apply search filter if provided
			if (!string.IsNullOrEmpty(request.SearchKeyword))
			{
				comments = comments.Where(p => p.Content.Contains(request.SearchKeyword));
			}

			// Apply NewsArticleId filter if provided
			if (!string.IsNullOrEmpty(request.NewsArticleId))
			{
				comments = comments.Where(p => p.NewsArticleId == request.NewsArticleId);
			}

			if (comments == null || !comments.Any())
			{
				throw new NotFoundException("No comments found matching the criteria.");
			}

			var totalCount = await comments.CountAsync();


			// Sorting created date by default
			if (request.IsAscending.HasValue)
			{
				comments=request.IsAscending.Value ?
					comments.OrderBy(p => p.CreatedDate) :
					comments.OrderByDescending(p => p.CreatedDate);
			}



			// Pagination
			var pagedData = await comments
			   .Skip((request.Page - 1) * request.PageSize)
			   .Take(request.PageSize)
			   .ToListAsync();



			var items = pagedData.Select(entity =>
			{
				var dto = new ViewCommentDTO();
				var fields = ParseFields(request.Fields);

				if (fields.Contains("commentId")) dto.CommentId = entity.CommentId;
				if (fields.Contains("content")) dto.Content = entity.Content;
				if (fields.Contains("likes")) dto.Likes = entity.Likes;
				if (fields.Contains("createdDate")) dto.CreatedDate = entity.CreatedDate;
				if (fields.Contains("NewsArticleId")) dto.NewsArticleId = entity.NewsArticleId;
				if (fields.Contains("CreatedByAccountId")) dto.CreatedByAccountId = entity.CreatedByAccountId;


				return dto;
			}).ToList();

			int totalPage = (int)Math.Ceiling((double)totalCount / request.PageSize);

			return new PagedResult<ViewCommentDTO>(
				Page: request.Page,
				PageSize: request.PageSize,
				TotalPage: totalPage,
				TotalItems: totalCount,
				Items: items
			);
		}

		public async Task<ViewCommentDTO> GetCommentById(int commentId)
		{
			Comment comment = await _commentRepository.GetByIdAsync(commentId);

			if (comment == null)
			{
				throw new NotFoundException("Comment not found");
			}

			ViewCommentDTO viewCommentDTO = new ViewCommentDTO
			{
				Content = comment.Content,
				Likes = comment.Likes,
				CreatedDate = comment.CreatedDate,
				NewsArticleId = comment.NewsArticleId,
				CreatedByAccountId = comment.CreatedByAccountId
			};

			return viewCommentDTO;
		}
		public async Task CreateCommentAsync(CreatedCommentDTO dto)
		{

			//Check if the news article and account Id exists can be added here
			var newsArticle = await _newsArticleRepository.GetByIdAsync(dto.NewsArticleId);
			if (newsArticle == null)
			{
				throw new NotFoundException("News Article not found to post comment");
			}

			var account = await _accountRepository.GetByIdAsync(dto.CreatedByAccountId);
			if (account == null)
			{
				throw new NotFoundException("Account not found to post comment");
			}

			Comment comment = new Comment
			{
				Content = dto.Content ?? "Just a comment",
				Likes = 0,
				CreatedDate = DateTime.UtcNow,
				NewsArticleId = dto.NewsArticleId ?? "0",
				CreatedByAccountId = dto.CreatedByAccountId
			};

			await _commentRepository.AddAsync(comment);
			await _unitOfWork.SaveChangesAsync();


		}

		public async Task UpdateCommentAsync(UpdatedCommentDTO dto, int commentId)
		{

			Comment comment = await _commentRepository.GetByIdAsync(commentId);

			if (comment == null)
			{
				throw new NotFoundException("Comment not found");
			}

			comment.Content = dto.Content;
			comment.Likes = dto.Likes.HasValue ? dto.Likes.Value : 0;

			_commentRepository.Update(comment);
			await _unitOfWork.SaveChangesAsync();


		}

		public async Task DeleteCommentAsync(int commentId)
		{

			Comment comment = await _commentRepository.GetByIdAsync(commentId);

			if (comment == null)
			{
				throw new NotFoundException("comment not found");
			}

			_commentRepository.Delete(comment);
			await _unitOfWork.SaveChangesAsync();


		}

		private HashSet<string> ParseFields(string? fields)
		{
			if (string.IsNullOrWhiteSpace(fields))
			{
				// Return all fields by default
				return new HashSet<string>(StringComparer.OrdinalIgnoreCase)
				{
					"commentId", "content", "likes", "createdDate", "newsArticleId", "createdByAccountId"
				};
			}

			return new HashSet<string>(
				fields.Split(',').Select(f => f.Trim()),
				StringComparer.OrdinalIgnoreCase
			);
		}


	}
}

