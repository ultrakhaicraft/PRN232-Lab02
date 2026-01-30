using Azure.Core;
using Microsoft.EntityFrameworkCore;
using PRN232.FUNewsManagement.Services.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN232.FUNewsManagement.Services.Utility
{
	public static class PageConverter<T>
	{
		public async static Task<PagedResult<T>> ToPagedResult(
			int page, int pageSize, int totalItems, IQueryable<T> queryableData)
		{
			var  pagedItems = await queryableData.Skip((page - 1) * pageSize)
							   .Take(pageSize)
							   .ToListAsync();

			int totalPage = (int)Math.Ceiling((double)totalItems / pageSize);


			return new PagedResult<T>(page, pageSize, totalPage, totalItems, pagedItems);

		}

		
	}
}
