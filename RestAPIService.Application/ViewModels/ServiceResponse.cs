using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestAPIService.Application.ViewModels
{
    public class ServiceReponse<T>
    {
        public T? Data { get; set; }
        public bool Success { get; set; } = false;
        public string? Message { get; set; } = null;
        public string? Error { get; set; } = null;
        public List<string>? ErrorMessages { get; set; } = null;
    }
    public class PaginationModel<T>
    {
        public int Page { get; set; }
        public int TotalPage { get; set; }
        public int TotalRecords { get; set; }
        public IEnumerable<T> ListData { get; set; } = new List<T>();
    }
}
