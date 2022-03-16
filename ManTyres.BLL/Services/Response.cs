using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace ManTyres.BLL.Services
{
	public class Response<T>
	{
		public Response(T content, long count, HttpStatusCode code, string? message)
		{
			Content = content;
			Count = count;
			Code = code;
			Message = message;
		}

		public Response()
		{
			Content = default;
		}
		
		public T? Content { get; set; }
		public long Count { get; set; }
		public HttpStatusCode Code { get; set; }
		public string? Message { get; set; }
	}
}
