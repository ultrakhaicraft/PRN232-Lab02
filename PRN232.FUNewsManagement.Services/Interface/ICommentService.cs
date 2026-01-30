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
	public interface ICommentService
	{
		public Task<PagedResult<ViewCommentDTO>> SearchComment(CommentPagedRequest request);
		public Task<ViewCommentDTO> GetCommentById(int commentId);
		public Task CreateCommentAsync(CreatedCommentDTO dto);
		public Task UpdateCommentAsync(UpdatedCommentDTO dto, int commentId);
		public Task DeleteCommentAsync(int commentId);

	}
}
