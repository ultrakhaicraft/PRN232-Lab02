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
	public class CategoryService : ICategoryService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IGenericRepository<Category> _categoryService;

		public CategoryService(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
			_categoryService = _unitOfWork.Repository<Category>();
		}

		

		public async Task<ViewCategoryDTO> GetCategoryById(short categoryId)
		{
			Category category = await _categoryService.GetByIdAsync(categoryId);

			if (category == null)
			{
				throw new NotFoundException($"Category with {categoryId} not found");
			}

			ViewCategoryDTO viewCategoryDTO = new ViewCategoryDTO
			{
				CategoryDesciption = category.CategoryDesciption,
				CategoryId = category.CategoryId,
				CategoryName = category.CategoryName,
				ParentCategoryId = category.ParentCategoryId,
				IsActive = category.IsActive
			};

			return viewCategoryDTO;
		}
		public async Task CreateCategoryAsync(CreateCategoryDTO dto)
		{
			
				Category newCategory = new Category
				{
					CategoryDesciption = dto.CategoryDesciption,
					CategoryName = dto.CategoryName,
					ParentCategoryId = dto.ParentCategoryId,
					IsActive = dto.IsActive
				};

				await _categoryService.AddAsync(newCategory);
				await _unitOfWork.SaveChangesAsync();
			
		}

		/// <summary>
		/// Method can perform search, sort, filter and pagination. Search are limited to one data field only.
		/// </summary>
		/// <returns></returns>
		public async Task<PagedResult<ViewCategoryDTO>> SearchCategory(CategoryPagedRequest request)
		{
			//Search + Filter (Filter IsActive)
			IQueryable<Category> categories = _categoryService.Find(p =>
				(string.IsNullOrEmpty(request.SearchKeyword) ||
				p.CategoryName.Contains(request.SearchKeyword)) &&
				(!request.IsActive.HasValue || p.IsActive == request.IsActive));

			if (categories == null || !categories.Any())
			{
				throw new NotFoundException("No categories found");
			}

			var totalCount = await categories.CountAsync();


			// Sorting when needed, sort by CategoryName alphabetically
			if (request.IsAscending.HasValue)
			{
				categories = request.IsAscending.Value ? categories.OrderBy(t => t.CategoryName) :
					categories.OrderByDescending(t => t.CategoryName);
			}


			// Pagination
			var pagedData = await categories
			   .Skip((request.Page - 1) * request.PageSize)
			   .Take(request.PageSize)
			   .ToListAsync();



			var items = pagedData.Select(entity =>
			{
				var dto = new ViewCategoryDTO();
				var fields = ParseFields(request.Fields);

				if (fields.Contains("categoryId")) dto.CategoryId = entity.CategoryId;
				if (fields.Contains("categoryName")) dto.CategoryName = entity.CategoryName;
				if (fields.Contains("categoryDescription")) dto.CategoryDesciption = entity.CategoryDesciption;
				if (fields.Contains("isActive")) dto.IsActive = entity.IsActive;
				if (fields.Contains("parentCategoryId")) dto.ParentCategoryId = entity.ParentCategoryId.HasValue ? entity.ParentCategoryId : 0;
				return dto;
			}).ToList();

			int totalPage = (int)Math.Ceiling((double)totalCount / request.PageSize);

			return new PagedResult<ViewCategoryDTO>(
				Page: request.Page,
				PageSize: request.PageSize,
				TotalPage: totalPage,
				TotalItems: totalCount,
				Items: items
			);
		}

		public async Task UpdateCategoryAsync(UpdateCategoryDTO dto, short categoryId)
		{
			

				Category category = await _categoryService.GetByIdAsync(categoryId);

				if (category == null)
				{
					throw new NotFoundException($"Category with {categoryId} not found");
				}

				category.CategoryDesciption = dto.CategoryDesciption;
				category.CategoryName = dto.CategoryName;
				category.ParentCategoryId = dto.ParentCategoryId;
				category.IsActive = dto.IsActive;

				_categoryService.Update(category);
				await _unitOfWork.SaveChangesAsync();
	

		}

		public async Task DeleteCategoryAsync(short categoryId)
		{
			

				Category category = await _categoryService.GetByIdAsync(categoryId);

				if (category == null)
				{
					throw new NotFoundException($"Category with {categoryId} not found");	
				}

				_categoryService.Delete(category);
				await _unitOfWork.SaveChangesAsync();
			

		}

		private HashSet<string> ParseFields(string? fields)
		{
			if (string.IsNullOrWhiteSpace(fields))
			{
				// Return all fields by default
				return new HashSet<string>(StringComparer.OrdinalIgnoreCase)
				{
					"categoryId", "categoryName", "categoryDescription", "parentCategoryId", "isActive"
				};
			}

			return new HashSet<string>(
				fields.Split(',').Select(f => f.Trim()),
				StringComparer.OrdinalIgnoreCase
			);
		}
	}
}
