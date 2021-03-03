using System.Collections.Generic;

namespace ProData.Infrastructure.Common.Client.Api
{
	public class SearchResult<T> where T : class
	{
		public SearchResult()
		{
			this.Items = new List<T>();
			this.Messages = new List<string>();
		}

		public List<T> Items { get; set; }

		public int PageNumber { get; set; }

		public int PageSize { get; set; }

		public int TotalRowCount { get; set; }

		public List<string> Messages { get; set; }

		public int Count => this.Items?.Count ?? 0;

		public int TotalNumberOfPages => this.PageSize == 0 ? 0 : (this.TotalRowCount / this.PageSize) + ((this.TotalRowCount % this.PageSize) > 0 ? 1 : 0);
	}
}