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
	public interface ITagService
	{
	
		public Task<ViewTagDTO> GetTagById(int tagId);
		public Task CreateTagAsync(CreateTagDTO dto);
		public Task UpdateTagAsync(UpdateTagDTO dto, int tagId);
		public Task DeleteTagAsync(int tagId);
		public Task<PagedResult<ViewTagDTO>> SearchTag(TagPagedRequest request);

	}
}
