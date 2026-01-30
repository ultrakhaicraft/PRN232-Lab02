using PRN232.FUNewsManagement.Services.DTO;
using PRN232.FUNewsManagement.Services.DTO.Request;
using PRN232.FUNewsManagement.Services.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN232.FUNewsManagement.Services.Interface
{
	public interface ICategoryService
	{
		public Task<ViewCategoryDTO> GetCategoryById(short categoryId);
		public Task CreateCategoryAsync(CreateCategoryDTO dto);
		public Task UpdateCategoryAsync(UpdateCategoryDTO dto, short categoryId);
		public Task DeleteCategoryAsync(short categoryId);
		public Task<PagedResult<ViewCategoryDTO>> SearchCategory(CategoryPagedRequest request);


	}
}
