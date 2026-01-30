using AutoMapper;
using FUNewsManagementSystemRepository.Models;
using Microsoft.EntityFrameworkCore;
using PRN232.FUNewsManagement.Repo.Enum;
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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace PRN232.FUNewsManagement.Services.Service
{
	public class TagService : ITagService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IGenericRepository<Tag> _tagRepository;
		private readonly IMapper _mapper;

		public TagService(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
			_tagRepository = _unitOfWork.Repository<Tag>();
		}

		

			/// <summary>
			/// Method can perform search, sort, filter and pagination. Search are limited to one data field only.
			/// </summary>
			/// <returns></returns>
			public async Task<PagedResult<ViewTagDTO>> SearchTag(TagPagedRequest request)
			{

				//Search + Filter
				IQueryable<Tag> tags = _tagRepository.Find(p =>
					(string.IsNullOrEmpty(request.SearchKeyword) ||
					p.TagName.Contains(request.SearchKeyword)) &&
					(!request.TagStatus.HasValue || p.TagStatus == request.TagStatus.Value.ToString()));

				if (tags == null || !tags.Any())
				{
					throw new NotFoundException("No tags found matching the criteria.");
				}

				var totalCount = await tags.CountAsync();


				// Sorting when needed
				if (request.IsAscending.HasValue)
				{
					tags  = request.IsAscending.Value ? tags.OrderBy(t=>t.TagName) :
						tags.OrderByDescending(t=>t.TagName);
				}

			

				// Pagination
				var pagedData = await tags
				   .Skip((request.Page - 1) * request.PageSize)
				   .Take(request.PageSize)
				   .ToListAsync();



				var items = pagedData.Select(entity =>
				{
					var dto = new ViewTagDTO();
					var fields = ParseFields(request.Fields);

					if (fields.Contains("tagId")) dto.TagId = entity.TagId;
					if (fields.Contains("tagName")) dto.TagName = entity.TagName;
					if (fields.Contains("note")) dto.Note = entity.Note;
					if (fields.Contains("tagStatus")) dto.TagStatus = entity.TagStatus;

					return dto;
				}).ToList();

				int totalPage = (int)Math.Ceiling((double)totalCount / request.PageSize);	

				return new PagedResult<ViewTagDTO>(
					Page: request.Page,
					PageSize: request.PageSize,
					TotalPage: totalPage,
					TotalItems: totalCount,
					Items: items
				);
			}

		public async Task<ViewTagDTO> GetTagById(int tagId)
		{
			Tag tag = await _tagRepository.GetByIdAsync(tagId);

			if(tag == null)
			{
				throw new NotFoundException("Tag not found");
			}

			ViewTagDTO viewTagDTO = new ViewTagDTO
			{
				TagId = tag.TagId,
				TagName = tag.TagName,
				Note = tag.Note
			};

			return viewTagDTO;
		}
		public async Task CreateTagAsync(CreateTagDTO dto)
		{
			
				Tag newTag = new Tag		
				{
					TagId = dto.TagId,
					TagName = dto.TagName,
					Note = dto.Note,
					TagStatus= Status.Active.ToString()
				};

				await _tagRepository.AddAsync(newTag);
				await _unitOfWork.SaveChangesAsync();
			

		}

		public async Task UpdateTagAsync(UpdateTagDTO dto, int tagId)
		{
			
				Tag tag = await _tagRepository.GetByIdAsync(tagId);

				if (tag == null)
				{
					throw new NotFoundException("Tag not found");
				}

				tag.TagName = dto.TagName;
				tag.Note = dto.Note;
				tag.TagStatus = dto.TagStatus;
				_tagRepository.Update(tag);
				await _unitOfWork.SaveChangesAsync();
			

		}

		public async Task DeleteTagAsync(int tagId)
		{
			
				Tag tag = await _tagRepository.GetByIdAsync(tagId);

				if (tag == null)
				{
					throw new NotFoundException("Tag not found");
				}

				_tagRepository.Delete(tag);
				await _unitOfWork.SaveChangesAsync();
			

		}

		private HashSet<string> ParseFields(string? fields)
		{
			if (string.IsNullOrWhiteSpace(fields))
			{
				// Return all fields by default
				return new HashSet<string>(StringComparer.OrdinalIgnoreCase)
				{
					"tagId", "tagName", "note", "tagStatus"
				};
			}

			return new HashSet<string>(
				fields.Split(',').Select(f => f.Trim()),
				StringComparer.OrdinalIgnoreCase
			);
		}

		
	}
}
